// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/Etienne/SolidTexture"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord("", 2D) = "white" {}
		[HideInInspector] __dirty("", Int) = 1
	}

		SubShader
		{
			Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
			Cull Back
			CGPROGRAM
			#pragma target 2.0
			#pragma surface surf Lambert keepalpha addshadow fullforwardshadows 
			struct Input
			{
				float2 uv_texcoord;
			};

			uniform sampler2D _Texture;
			uniform float4 _Texture_ST;
			uniform float4 _Color;

			void surf(Input i, inout SurfaceOutput o)
			{
				float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
				o.Albedo = (tex2D(_Texture, uv_Texture) * _Color).rgb;
				o.Alpha = 1;
			}

			ENDCG
		}
			Fallback "Diffuse"
}