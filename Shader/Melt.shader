Shader "ShaderTest/Melt"
{
    Properties
    {
    	_MainTex ("Texture", 2D) = "white" {}//������
    	_NoiseTex("Noise", 2D) = "white" {}//��������
    	_Threshold("Threshold", Range(0.0, 1.0)) = 0.5//���ڷ�ֵ
    	_EdgeLength("Edge Length", Range(0.0, 0.2)) = 0.1//��Ե���
    	_EdgeFirstColor("First Edge Color", Color) = (1,1,1,1)//��Ե��ɫֵ1
    	_EdgeSecondColor("Second Edge Color", Color) = (1,1,1,1)//��Ե��ɫֵ2
		[Toggle]_IsVerse("IsVerser",float) = 0
    }    
   SubShader
   {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }//��ǩ
        Pass
        {
		    Cull Off //Ҫ��Ⱦ���汣֤Ч����ȷ 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            

            struct a2v//������ɫ������ṹ��
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
            			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uvMainTex : TEXCOORD0;
				float2 uvNoiseTex : TEXCOORD1;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float _Threshold;
			float _EdgeLength;
			fixed4 _EdgeFirstColor;
			fixed4 _EdgeSecondColor;
			float _IsVerse;
			
			v2f vert (a2v v)//������ɫ��
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);//����������仯����������ϵ
				o.uvMainTex = TRANSFORM_TEX(v.uv, _MainTex);//��������������任
				o.uvNoiseTex = TRANSFORM_TEX(v.uv, _NoiseTex);//����������������任
				return o;
			}
			
	        fixed4 frag (v2f i) : SV_Target//ƬԪ��ɫ��
			{
				fixed cutout = tex2D(_NoiseTex, i.uvNoiseTex).r;//��ȡ�Ҷ�ͼ��Rͨ��
				if(_IsVerse==1)//����ѡ�з�ת
				    cutout = 1- cutout;
				clip(cutout - _Threshold);//�������ڷ�ֵ�ü�ƬԪ

				float degree = saturate((cutout - _Threshold) / _EdgeLength);//�淶��
				fixed4 edgeColor = lerp(_EdgeFirstColor, _EdgeSecondColor, degree);//����ɫֵ���в�ֵ

				fixed4 col = tex2D(_MainTex, i.uvMainTex);//����������в���

				fixed4 finalColor = lerp(edgeColor, col, degree);//�Ա�Ե��ɫ��ƬԪ��ɫ���в�ֵ
				return fixed4(finalColor.rgb, 1);
			}
            ENDCG
        }
    }
}

