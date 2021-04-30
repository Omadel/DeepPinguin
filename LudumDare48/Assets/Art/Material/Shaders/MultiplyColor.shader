Shader "Mobile/Etienne/FadeColor"
{
	Properties
	{
		_Color("Color", Color) = (0,0,1,1)
		[HideInInspector] __dirty("", Int) = 1
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma target 2.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			half filler;
		};

		uniform float4 _Color;

		inline half4 LightingUnlit(SurfaceOutput s, half3 lightDir, half atten)
		{
			return half4 (0, 0, 0, s.Alpha);
		}

		void surf(Input i , inout SurfaceOutput o)
		{
			o.Emission = _Color.rgb;
			float temp_output_4_4 = _Color.a;
			o.Alpha = temp_output_4_4;
		}

		ENDCG
	}
}