Shader "Obsidian/AcheronSurfaceDetail"
{
    Properties
    {
        _DetailScale ("Detail Scale", Float) = 180
        _DetailStrength ("Detail Strength", Range(0,1)) = 0.25
        _DetailColor ("Detail Color", Color) = (0.22,0.16,0.12,1)
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
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _DetailScale;
                float _DetailStrength;
                float4 _DetailColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs pos = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionHCS = pos.positionCS;
                output.positionWS = pos.positionWS;
                return output;
            }

            float hash31(float3 p)
            {
                p = frac(p * 0.1031);
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float noise3(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);

                float a = hash31(i);
                float b = hash31(i + float3(1,0,0));
                float c = hash31(i + float3(0,1,0));
                float d = hash31(i + float3(1,1,0));
                float e = hash31(i + float3(0,0,1));
                float f1 = hash31(i + float3(1,0,1));
                float g = hash31(i + float3(0,1,1));
                float h = hash31(i + float3(1,1,1));

                return lerp(
                    lerp(lerp(a,b,f.x),lerp(c,d,f.x),f.y),
                    lerp(lerp(e,f1,f.x),lerp(g,h,f.x),f.y),
                    f.z
                );
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 p = normalize(input.positionWS) * _DetailScale;

                float n1 = noise3(p);
                float n2 = noise3(p * 2.7);
                float detail = saturate((n1 * 0.7 + n2 * 0.3 - 0.42) * 2.0);

                float alpha = detail * _DetailStrength;

                return half4(_DetailColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}