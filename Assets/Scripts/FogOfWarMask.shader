Shader "Custom/FogOfWarMask" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SecTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader{
		Tags { "Queue"="Transparent" "RenderType" = "Transparent" "LightMode" = "ForwardBase" }
		Blend SrcAlpha OneMinusSrcAlpha
		Lighting Off
		LOD 200

		CGPROGRAM

		#pragma surface surf NoLighting noambient alpha:fade

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten)
		{
			fixed4 color;
			color.rgb = s.Albedo;
			color.a = s.Alpha;
			return color;
				
		}

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _SecTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			//sample secondary texture, make it the base color of the plane
			half4 baseColor = tex2D(_SecTex, IN.uv_SecTex);
			o.Albedo = _Color.rgb * baseColor;
			//sample maintexture (aperture mask) green value, and make it the alpha value. 
			o.Alpha = _Color.a - tex2D(_MainTex, IN.uv_MainTex).g; // green - color of aperture mask
		}
		ENDCG
	}
	FallBack "Diffuse"
}
