using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public sealed class CheckoutBackWallVideoPlayback : MonoBehaviour
{
    public RenderTexture outputTexture;
    public Renderer screenRenderer;
    public string videoAssetPath =
        "Assets/Video/ObsidianProtocol_StoreScreen_Sample.mp4";

    private VideoPlayer player;
    private Renderer parentRenderer;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        if (player == null)
        {
            Debug.LogError("[CHECKOUT VIDEO] VideoPlayer missing.", this);
            enabled = false;
            return;
        }

        if (screenRenderer == null)
        {
            foreach (Renderer candidate in
                GetComponentsInChildren<Renderer>(true))
            {
                if (candidate.gameObject.name == "Video Surface")
                {
                    screenRenderer = candidate;
                    break;
                }
            }
        }

        if (screenRenderer == null || screenRenderer.sharedMaterial == null)
        {
            Debug.LogError(
                "[CHECKOUT VIDEO] Video Surface renderer/material missing.",
                this);
            enabled = false;
            return;
        }

        parentRenderer = GetComponent<Renderer>();
        if (parentRenderer != null && parentRenderer != screenRenderer)
        {
            parentRenderer.enabled = false;
            Debug.Log(
                "[CHECKOUT VIDEO] Temporarily disabled parent renderer " +
                "to test for screen occlusion.", this);
        }

        Material screenMaterial = screenRenderer.sharedMaterial;
        string textureProperty = screenMaterial.HasProperty("_BaseMap")
            ? "_BaseMap"
            : screenMaterial.HasProperty("_MainTex")
                ? "_MainTex"
                : null;

        if (textureProperty == null)
        {
            Debug.LogError(
                "[CHECKOUT VIDEO] Material has neither _BaseMap nor _MainTex: " +
                screenMaterial.shader.name, this);
            enabled = false;
            return;
        }

        player.Stop();
        player.source = VideoSource.Url;
        player.url = new Uri(Path.GetFullPath(videoAssetPath)).AbsoluteUri;
        player.clip = null;

        // Let VideoPlayer feed decoded frames directly to the screen material.
        player.renderMode = VideoRenderMode.MaterialOverride;
        player.targetTexture = null;
        player.targetMaterialRenderer = screenRenderer;
        player.targetMaterialProperty = textureProperty;

        player.isLooping = true;
        player.playOnAwake = false;
        player.waitForFirstFrame = true;
        player.audioOutputMode = VideoAudioOutputMode.None;

        player.errorReceived += OnError;
        player.prepareCompleted += OnPrepared;
        player.Prepare();

        Debug.Log(
            "[CHECKOUT VIDEO] Direct material output preparing." +
            " | Renderer=" + screenRenderer.name +
            " | Shader=" + screenMaterial.shader.name +
            " | Property=" + textureProperty +
            " | ParentRendererDisabled=" +
            (parentRenderer != null && !parentRenderer.enabled), this);
    }

    private void OnPrepared(VideoPlayer source)
    {
        if (source != player)
            return;

        source.Play();
        Debug.Log(
            "[CHECKOUT VIDEO] Direct material playback started." +
            " | " + source.width + "x" + source.height, this);
    }

    private void OnError(VideoPlayer source, string message)
    {
        Debug.LogError("[CHECKOUT VIDEO] " + message, this);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.errorReceived -= OnError;
            player.prepareCompleted -= OnPrepared;
        }

        // The parent renderer was disabled only for this runtime test.
        if (parentRenderer != null)
            parentRenderer.enabled = true;
    }
}
