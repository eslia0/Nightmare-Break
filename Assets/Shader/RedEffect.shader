Shader "Exam/RedEffect" {
	Properties {
		_MainTex("Base (RGB)",2D) = "white"{}
		_LuminosityAmount("redScale Amount", Range(0.0,1)) = 1.0
	}
	SubShader {
		Pass
		{
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		fixed _LuminosityAmount;

		fixed4 frag(v2f_img i) : COLOR
		{
		fixed4 renderTex = tex2D(_MainTex,i.uv);

		float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g+ 0.114 * renderTex.b;

		 renderTex.r = lerp(renderTex.r,luminosity,_LuminosityAmount);
		 renderTex.g = 0.0f;
		 renderTex.b = 0.0f; 

		return renderTex;
		}
		ENDCG
	}
	}
	FallBack off
}
