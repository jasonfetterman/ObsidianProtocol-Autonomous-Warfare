// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/TerrainEngine/Splatmap/WCSpecular-AddPass" {
    Properties {
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        [PowerSlider(5.0)] _Shininess ("Shininess", Range (0.03, 1)) = 0.078125
                _ColorMap("ColorMap (RGB)", 2D) = "white"
        _OffsetSize("Offset / Size", Vector) = (0,0,0,0)
    }
   

    SubShader {
        Tags {
            "Queue" = "Geometry-99"
            "IgnoreProjector"="True"
            "RenderType" = "Opaque"
        }

        CGPROGRAM
        #pragma surface surf BlinnPhong decal:add vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer fullforwardshadows nometa
        #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
        #pragma multi_compile_fog
        #pragma multi_compile_local __ _NORMALMAP
        #pragma target 3.0
        // needs more than 8 texcoords
        #pragma exclude_renderers gles

        #define TERRAIN_SPLAT_ADDPASS
        #include "WCTerrainSplatmapCommon.cginc"

        half _Shininess;
        sampler2D _ColorMap;

        void surf(Input IN, inout SurfaceOutput o)
        {
            half4 splat_control;
            half weight;
            fixed4 mixedDiffuse;
            SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);
            o.Albedo = mixedDiffuse.rgb * tex2D(_ColorMap, IN.tc.xy * _OffsetSize.zw + _OffsetSize.xy).rgb;
            o.Alpha = weight;
            o.Gloss = mixedDiffuse.a;
            o.Specular = _Shininess;
        }
        ENDCG
    }

    Fallback "Hidden/TerrainEngine/Splatmap/Diffuse-AddPass"
}
