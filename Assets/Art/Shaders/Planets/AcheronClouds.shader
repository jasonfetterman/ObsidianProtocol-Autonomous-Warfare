Shader "Obsidian/AcheronClouds"
{
    Properties
    {
        _CloudColor ("Cloud Color", Color) = (0.65, 0.7, 0.75, 1)
        _Coverage ("Cloud Coverage", Range(0,1)) = 0.52
        _Softness ("Cloud Softness", Range(0.01,0.5)) = 0.16
        _Scale ("Cloud Detail", Range(2,30)) = 12
        _Opacity ("Cloud Opacity", Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _CloudColor;
                float _Coverage;
                float _Softness;
                float _Scale;
                float _Opacity;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionOS : TEXCOORD0;
            };

            float hash31(float3 p)
            {
                p = frac(p * 0.1031);
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float noise3D(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);

                return lerp(
                    lerp(lerp(hash31(i), hash31(i + float3(1,0,0)), f.x),
                         lerp(hash31(i + float3(0,1,0)), hash31(i + float3(1,1,0)), f.x), f.y),
                    lerp(lerp(hash31(i + float3(0,0,1)), hash31(i + float3(1,0,1)), f.x),
                         lerp(hash31(i + float3(0,1,1)), hash31(i + float3(1,1,1)), f.x), f.y),
                    f.z);
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionOS = input.positionOS.xyz;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 p = normalize(input.positionOS) * _Scale;
                float n = noise3D(p) * 0.7 + noise3D(p * 2.1) * 0.3;
                float clouds = smoothstep(_Coverage, _Coverage + _Softness, n);
                return half4(_CloudColor.rgb, clouds * _Opacity);
            }
            ENDHLSL
        }
    }
}
