// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Hidden/TerrainEngine/Splatmap/Diffuse-Base" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _ColorMap("ColorMap (RGB)", 2D) = "white"
    _OffsetSize("Offset / Size", Vector) = (0,0,0,0)   
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 200

CGPROGRAM
#pragma surface surf Lambert vertex:SplatmapVert addshadow fullforwardshadows
#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd

#define TERRAIN_BASE_PASS
#include "WCTerrainSplatmapCommon.cginc"

sampler2D _MainTex;
fixed4 _Color;

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.tc.xy * _OffsetSize.zw + _OffsetSize.xy) * _Color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
}
ENDCG

UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
}

Fallback "Legacy Shaders/VertexLit"
}
