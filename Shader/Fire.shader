Shader "Myshader/Fire"
{
	Properties
	{
		//�������������ٶ�
		_SpeedY("SpeedY", Range(-10,10)) = 1
 
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_MainColor("MainColor",Color) = (1,1,1,1)

		_Height("Height", Range(-1,1)) = 1

	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		//Zwrite Off 
		//Cull Off
		Blend SrcAlpha OneMinusSrcAlpha// ��ͳ͸����
		
 
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
 
		#include "UnityCG.cginc"
 

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};
 
	struct v2f
	{
		float2 uv : TEXCOORD1;
		float4 vertex : SV_POSITION;
 
	};
 
	float _SpeedY;
	fixed _Height;
	sampler2D  _NoiseTex;
	float4 _NoiseTex_ST,_MainColor;
 
	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
		return o;
	}
 
	fixed4 frag(v2f i) : SV_Target
	{
	    float4 gradientBlend = lerp(float4(1,1,1,1), float4(0, 0.3, 0.5, 0), i.uv.y * _Height);//��һ�� fixed ֵ��Ϊһ���߽�������ס uv ��ֱ�����ϵ�������ɫֵ
		float4 result = gradientBlend  * tex2D(_NoiseTex, fixed2(i.uv.x , (i.uv.y + _SpeedY) - _Time.x)) * 3;
		return result * _MainColor;
	}
		ENDCG
	   }
	}
}