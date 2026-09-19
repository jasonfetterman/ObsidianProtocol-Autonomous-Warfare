using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    public class Water : MonoBehaviour
    {
        public enum WaterMode
        {
            Simple = 0,
            Reflective = 1,
            Refractive = 2,
        };

        public WaterMode waterMode = WaterMode.Refractive;
        public bool disablePixelLights = true;
        public int textureSize = 256;
        public float clipPlaneOffset = 0.07f;
        public LayerMask reflectLayers = -1;
        public LayerMask refractLayers = -1;

        private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();
        private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();
        private RenderTexture m_ReflectionTexture;
        private RenderTexture m_RefractionTexture;
        private WaterMode m_HardwareWaterSupport = WaterMode.Refractive;
        private int m_OldReflectionTextureSize;
        private int m_OldRefractionTextureSize;
        private static bool s_InsideWater;

        public void OnWillRenderObject()
        {
            if (!enabled || !GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial ||
                !GetComponent<Renderer>().enabled)
            {
                return;
            }

            Camera cam = Camera.current;
            if (!cam)
            {
                return;
            }

            if (s_InsideWater)
            {
                return;
            }

            s_InsideWater = true;

            m_HardwareWaterSupport = FindHardwareWaterSupport();
            WaterMode mode = GetWaterMode();

            Camera reflectionCamera, refractionCamera;
            CreateWaterObjects(cam, out reflectionCamera, out refractionCamera);

            Vector3 pos = transform.position;
            Vector3 normal = transform.up;

            int oldPixelLightCount = QualitySettings.pixelLightCount;
            if (disablePixelLights)
            {
                QualitySettings.pixelLightCount = 0;
            }

            UpdateCameraModes(cam, reflectionCamera);
            UpdateCameraModes(cam, refractionCamera);

            if (mode >= WaterMode.Reflective)
            {
                float d = -Vector3.Dot(normal, pos) - clipPlaneOffset;
                Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

                Matrix4x4 reflection = Matrix4x4.zero;
                CalculateReflectionMatrix(ref reflection, reflectionPlane);

                Vector3 oldpos = cam.transform.position;
                Vector3 newpos = reflection.MultiplyPoint(oldpos);

                reflectionCamera.worldToCameraMatrix = cam.worldToCameraMatrix * reflection;

                Vector4 clipPlane = CameraSpacePlane(
                    reflectionCamera,
                    pos,
                    normal,
                    1.0f
                );

                reflectionCamera.projectionMatrix =
                    cam.CalculateObliqueMatrix(clipPlane);

                reflectionCamera.cullingMatrix =
                    cam.projectionMatrix * cam.worldToCameraMatrix;

                reflectionCamera.cullingMask =
                    ~(1 << 4) & reflectLayers.value;

                reflectionCamera.targetTexture = m_ReflectionTexture;

                bool oldCulling = GL.invertCulling;
                GL.invertCulling = !oldCulling;

                reflectionCamera.transform.position = newpos;

                Vector3 euler = cam.transform.eulerAngles;
                reflectionCamera.transform.eulerAngles =
                    new Vector3(-euler.x, euler.y, euler.z);

                reflectionCamera.Render();

                reflectionCamera.transform.position = oldpos;
                GL.invertCulling = oldCulling;

                GetComponent<Renderer>().sharedMaterial.SetTexture(
                    "_ReflectionTex",
                    m_ReflectionTexture
                );
            }

            if (mode >= WaterMode.Refractive)
            {
                refractionCamera.worldToCameraMatrix =
                    cam.worldToCameraMatrix;

                Vector4 clipPlane = CameraSpacePlane(
                    refractionCamera,
                    pos,
                    normal,
                    -1.0f
                );

                refractionCamera.projectionMatrix =
                    cam.CalculateObliqueMatrix(clipPlane);

                refractionCamera.cullingMatrix =
                    cam.projectionMatrix * cam.worldToCameraMatrix;

                refractionCamera.cullingMask =
                    ~(1 << 4) & refractLayers.value;

                refractionCamera.targetTexture =
                    m_RefractionTexture;

                refractionCamera.transform.position =
                    cam.transform.position;

                refractionCamera.transform.rotation =
                    cam.transform.rotation;

                refractionCamera.Render();

                GetComponent<Renderer>().sharedMaterial.SetTexture(
                    "_RefractionTex",
                    m_RefractionTexture
                );
            }

            if (disablePixelLights)
            {
                QualitySettings.pixelLightCount = oldPixelLightCount;
            }

            switch (mode)
            {
                case WaterMode.Simple:
                    Shader.EnableKeyword("WATER_SIMPLE");
                    Shader.DisableKeyword("WATER_REFLECTIVE");
                    Shader.DisableKeyword("WATER_REFRACTIVE");
                    break;

                case WaterMode.Reflective:
                    Shader.DisableKeyword("WATER_SIMPLE");
                    Shader.EnableKeyword("WATER_REFLECTIVE");
                    Shader.DisableKeyword("WATER_REFRACTIVE");
                    break;

                case WaterMode.Refractive:
                    Shader.DisableKeyword("WATER_SIMPLE");
                    Shader.DisableKeyword("WATER_REFLECTIVE");
                    Shader.EnableKeyword("WATER_REFRACTIVE");
                    break;
            }

            s_InsideWater = false;
        }

        void OnDisable()
        {
            if (m_ReflectionTexture)
            {
                DestroyImmediate(m_ReflectionTexture);
                m_ReflectionTexture = null;
            }

            if (m_RefractionTexture)
            {
                DestroyImmediate(m_RefractionTexture);
                m_RefractionTexture = null;
            }

            foreach (var kvp in m_ReflectionCameras)
            {
                if (kvp.Value != null)
                {
                    DestroyImmediate(kvp.Value.gameObject);
                }
            }

            m_ReflectionCameras.Clear();

            foreach (var kvp in m_RefractionCameras)
            {
                if (kvp.Value != null)
                {
                    DestroyImmediate(kvp.Value.gameObject);
                }
            }

            m_RefractionCameras.Clear();
        }

        void Update()
        {
            if (!GetComponent<Renderer>())
            {
                return;
            }

            Material mat = GetComponent<Renderer>().sharedMaterial;

            if (!mat)
            {
                return;
            }

            Vector4 waveSpeed = mat.GetVector("WaveSpeed");
            float waveScale = mat.GetFloat("_WaveScale");

            Vector4 waveScale4 = new Vector4(
                waveScale,
                waveScale,
                waveScale * 0.4f,
                waveScale * 0.45f
            );

            double t = Time.timeSinceLevelLoad / 20.0;

            Vector4 offsetClamped = new Vector4(
                (float)Math.IEEERemainder(
                    waveSpeed.x * waveScale4.x * t,
                    1.0
                ),
                (float)Math.IEEERemainder(
                    waveSpeed.y * waveScale4.y * t,
                    1.0
                ),
                (float)Math.IEEERemainder(
                    waveSpeed.z * waveScale4.z * t,
                    1.0
                ),
                (float)Math.IEEERemainder(
                    waveSpeed.w * waveScale4.w * t,
                    1.0
                )
            );

            mat.SetVector("_WaveOffset", offsetClamped);
            mat.SetVector("_WaveScale4", waveScale4);
        }

        void UpdateCameraModes(Camera src, Camera dest)
        {
            if (dest == null)
            {
                return;
            }

            dest.clearFlags = src.clearFlags;
            dest.backgroundColor = src.backgroundColor;

            if (src.clearFlags == CameraClearFlags.Skybox)
            {
                Skybox sky = src.GetComponent<Skybox>();
                Skybox mysky = dest.GetComponent<Skybox>();

                if (!sky || !sky.material)
                {
                    mysky.enabled = false;
                }
                else
                {
                    mysky.enabled = true;
                    mysky.material = sky.material;
                }
            }

            dest.farClipPlane = src.farClipPlane;
            dest.nearClipPlane = src.nearClipPlane;
            dest.orthographic = src.orthographic;
            dest.fieldOfView = src.fieldOfView;
            dest.aspect = src.aspect;
            dest.orthographicSize = src.orthographicSize;
        }

        void CreateWaterObjects(
            Camera currentCamera,
            out Camera reflectionCamera,
            out Camera refractionCamera)
        {
            WaterMode mode = GetWaterMode();

            reflectionCamera = null;
            refractionCamera = null;

            int waterId = GetEntityId().GetHashCode();

            if (mode >= WaterMode.Reflective)
            {
                if (!m_ReflectionTexture ||
                    m_OldReflectionTextureSize != textureSize)
                {
                    if (m_ReflectionTexture)
                    {
                        DestroyImmediate(m_ReflectionTexture);
                    }

                    m_ReflectionTexture =
                        new RenderTexture(textureSize, textureSize, 16);

                    m_ReflectionTexture.name =
                        "__WaterReflection" + waterId;

                    m_ReflectionTexture.isPowerOfTwo = true;
                    m_ReflectionTexture.hideFlags = HideFlags.DontSave;
                    m_OldReflectionTextureSize = textureSize;
                }

                m_ReflectionCameras.TryGetValue(
                    currentCamera,
                    out reflectionCamera
                );

                if (!reflectionCamera)
                {
                    int cameraId = currentCamera.GetEntityId().GetHashCode();

                    GameObject go = new GameObject(
                        "Water Refl Camera id" +
                        waterId +
                        " for " +
                        cameraId,
                        typeof(Camera),
                        typeof(Skybox)
                    );

                    reflectionCamera = go.GetComponent<Camera>();
                    reflectionCamera.enabled = false;
                    reflectionCamera.transform.position = transform.position;
                    reflectionCamera.transform.rotation = transform.rotation;
                    reflectionCamera.gameObject.AddComponent<FlareLayer>();

                    go.hideFlags = HideFlags.HideAndDontSave;

                    m_ReflectionCameras[currentCamera] =
                        reflectionCamera;
                }
            }

            if (mode >= WaterMode.Refractive)
            {
                if (!m_RefractionTexture ||
                    m_OldRefractionTextureSize != textureSize)
                {
                    if (m_RefractionTexture)
                    {
                        DestroyImmediate(m_RefractionTexture);
                    }

                    m_RefractionTexture =
                        new RenderTexture(textureSize, textureSize, 16);

                    m_RefractionTexture.name =
                        "__WaterRefraction" + waterId;

                    m_RefractionTexture.isPowerOfTwo = true;
                    m_RefractionTexture.hideFlags = HideFlags.DontSave;
                    m_OldRefractionTextureSize = textureSize;
                }

                m_RefractionCameras.TryGetValue(
                    currentCamera,
                    out refractionCamera
                );

                if (!refractionCamera)
                {
                    int cameraId = currentCamera.GetEntityId().GetHashCode();

                    GameObject go = new GameObject(
                        "Water Refr Camera id" +
                        waterId +
                        " for " +
                        cameraId,
                        typeof(Camera),
                        typeof(Skybox)
                    );

                    refractionCamera = go.GetComponent<Camera>();
                    refractionCamera.enabled = false;
                    refractionCamera.transform.position = transform.position;
                    refractionCamera.transform.rotation = transform.rotation;
                    refractionCamera.gameObject.AddComponent<FlareLayer>();

                    go.hideFlags = HideFlags.HideAndDontSave;

                    m_RefractionCameras[currentCamera] =
                        refractionCamera;
                }
            }
        }

        WaterMode GetWaterMode()
        {
            if (m_HardwareWaterSupport < waterMode)
            {
                return m_HardwareWaterSupport;
            }

            return waterMode;
        }

        WaterMode FindHardwareWaterSupport()
        {
            if (!GetComponent<Renderer>())
            {
                return WaterMode.Simple;
            }

            Material mat = GetComponent<Renderer>().sharedMaterial;

            if (!mat)
            {
                return WaterMode.Simple;
            }

            string mode = mat.GetTag("WATERMODE", false);

            if (mode == "Refractive")
            {
                return WaterMode.Refractive;
            }

            if (mode == "Reflective")
            {
                return WaterMode.Reflective;
            }

            return WaterMode.Simple;
        }

        Vector4 CameraSpacePlane(
            Camera cam,
            Vector3 pos,
            Vector3 normal,
            float sideSign)
        {
            Vector3 offsetPos =
                pos + normal * clipPlaneOffset;

            Matrix4x4 m =
                cam.worldToCameraMatrix;

            Vector3 cpos =
                m.MultiplyPoint(offsetPos);

            Vector3 cnormal =
                m.MultiplyVector(normal).normalized *
                sideSign;

            return new Vector4(
                cnormal.x,
                cnormal.y,
                cnormal.z,
                -Vector3.Dot(cpos, cnormal)
            );
        }

        static void CalculateReflectionMatrix(
            ref Matrix4x4 reflectionMat,
            Vector4 plane)
        {
            reflectionMat.m00 =
                (1F - 2F * plane[0] * plane[0]);

            reflectionMat.m01 =
                (-2F * plane[0] * plane[1]);

            reflectionMat.m02 =
                (-2F * plane[0] * plane[2]);

            reflectionMat.m03 =
                (-2F * plane[3] * plane[0]);

            reflectionMat.m10 =
                (-2F * plane[1] * plane[0]);

            reflectionMat.m11 =
                (1F - 2F * plane[1] * plane[1]);

            reflectionMat.m12 =
                (-2F * plane[1] * plane[2]);

            reflectionMat.m13 =
                (-2F * plane[3] * plane[1]);

            reflectionMat.m20 =
                (-2F * plane[2] * plane[0]);

            reflectionMat.m21 =
                (-2F * plane[2] * plane[1]);

            reflectionMat.m22 =
                (1F - 2F * plane[2] * plane[2]);

            reflectionMat.m23 =
                (-2F * plane[3] * plane[2]);

            reflectionMat.m30 = 0F;
            reflectionMat.m31 = 0F;
            reflectionMat.m32 = 0F;
            reflectionMat.m33 = 1F;
        }
    }
}




