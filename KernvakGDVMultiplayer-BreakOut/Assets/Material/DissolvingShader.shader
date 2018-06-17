Shader "Custom/DissolvingShader" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white"
		_DissolveMap("Dissolve Shape", 2D) = "white"{}
		_DissolveValue("Dissolve Value", Range(-0.2, 1.2)) = 1.2
		_LineWidth("Line Width", Range(0.0, 0.2)) = 0.1	
		_LineColor("Line Color", Color) = (1.0, 1.0, 1.0, 1.0)
		
        _HitPos ("Collision point", Vector) = (0, 0, 0, 1)

	}
	SubShader {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DissolveMap;

		float _DissolveValue;
		float _LineWidth;
		float4 _LineColor;
		float4 _HitPos;


		struct Input {
			float2 uv_MainTex;
			half2 uv_DissolveMap; 
			float3 worldPos;

		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			half4 dissolve = tex2D(_DissolveMap, IN.uv_DissolveMap);		
			half4 clear = half4(0.0, 0.0, 0.0, 0.0);

			float dist = max(0, 1 - distance(_HitPos.xyz, IN.worldPos)) ;
			dist = min(1, dist * dissolve.r);


			int isClear = int(dist - (_DissolveValue + _LineWidth) + 0.99);
			int isLine = int(dist - (_DissolveValue) + 0.99);

			half4 altCol = lerp(_LineColor, clear, isClear);
	
			o.Albedo = lerp(o.Albedo, altCol, isLine);
			o.Alpha = lerp(1.0, 0.0, isClear);

		}
		ENDCG
	}
	FallBack "Diffuse"
}


//https://www.youtube.com/watch?v=LLYPFTYJafQ&index=4&list=PLLH3mUGkfFCWHABIk1I_VWEwdcXO5Gura
//http://www.trifox-game.com/2017/06/15/dissolving-the-world-part-2/
//https://www.gamasutra.com/blogs/SamanthaStahlke/20180102/312388/The_Great_Disappearing_Act_Creating_Dissolve_VFX_with_Unity_Surface_Shaders.php
//dissoling shader die op impact werkt. de impact is de start positie waar hij begint de verdwijnen.