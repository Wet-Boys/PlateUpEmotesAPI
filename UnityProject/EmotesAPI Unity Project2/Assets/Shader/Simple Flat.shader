Shader "Simple Flat" {
	Properties {
		[HideInInspector] _EmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff ("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin] _Colour2 ("Colour 2", Vector) = (0,0,0,0)
		_Color0 ("Colour 1", Vector) = (0,0,0,0)
		[Toggle] _Highlight ("Highlight", Range(0, 1)) = 0
		_Overlay ("Overlay", 2D) = "white" {}
		[Toggle(_TEXTUREBYUV_ON)] _TextureByUV ("TextureByUV", Float) = 0
		[Toggle(_VARYBYUV_ON)] _VaryByUV ("VaryByUV", Float) = 0
		[Toggle(_HASTEXTUREOVERLAY_ON)] _HasTextureOverlay ("HasTextureOverlay", Float) = 0
		_Shininess ("Shininess", Range(0, 1)) = 0
		_OverlayLowerBound ("Overlay Lower Bound", Range(0, 1)) = 0.6
		_OverlayUpperBound ("Overlay Upper Bound", Range(0, 1)) = 0.9
		_OverlayScale ("Overlay Scale", Range(0, 1000)) = 10
		_OverlayMin ("Overlay Min", Range(0, 1)) = 0
		_OverlayMax ("Overlay Max", Range(0, 1)) = 0.3
		_OverlayOffset ("Overlay Offset", Vector) = (0,0,0,0)
		_OverlayTextureScale ("Overlay Texture Scale", Vector) = (10,10,0,0)
		_OverlayColour ("Overlay Colour", Vector) = (0,0,0,0)
		[ASEEnd] _Flatness ("Flatness", Range(0, 1)) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Hidden/InternalErrorShader"
	//CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
}