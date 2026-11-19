// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Nature/Terrain/WC Specular" {
    Properties {
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        [PowerSlider(5.0)] _Shininess ("Shininess", Range (0.03, 1)) = 0.078125

        // used in fallback on old cards & base map
        [HideInInspector] _MainTex ("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _Color ("Main Color", Color) = (1,1,1,1)
        _ColorMap("ColorMap (RGB)", 2D) = "white"
        _OffsetSize("Offset / Size", Vector) = (0,0,0,0)
    }

    SubShader {
        Tags {
            "Queue" = "Geometry-100"
            "RenderType" = "Opaque"
        }

        CGPROGRAM
        #pragma surface surf BlinnPhong vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer addshadow fullforwardshadows
        #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
        #pragma multi_compile_fog
        #pragma multi_compile_local __ _NORMALMAP
        #pragma target 3.0
        // needs more than 8 texcoords
        #pragma exclude_renderers gles

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

        UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
        UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
    }

    Dependency "AddPassShader"    = "Hidden/TerrainEngine/Splatmap/WCSpecular-AddPass"
    Dependency "BaseMapShader"    = "Hidden/TerrainEngine/Splatmap/WCSpecular-Base"
    Dependency "BaseMapGenShader" = "Hidden/TerrainEngine/Splatmap/WCDiffuse-BaseGen"

    Fallback "Nature/Terrain/Diffuse"
}
