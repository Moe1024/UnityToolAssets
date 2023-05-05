Shader "Custom/ToonShader"//NPR 卡通渲染
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WholeLight("WholeLight", Range(0,1)) = 0.3//整体光照强度
        _HighLight("HighLight", Range(0,1)) = 0.5//高光强度
        _Color("Color", COLOR) = (1,1,1,1)
        _Detail("Detail", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal: NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WholeLight;
            float _HighLight;
            float4 _Color;
            float _Detail;

            float Toon(float3 normal, float3 lightDir) //卡通渲染方法，传入法线及光照强度
            {
                float NdotL = max(0, dot(normalize(normal), normalize(lightDir)));//计算得出被光照着的表面，俩向量点乘返回一个 float 值

                return floor(NdotL / (1 - _Detail));//这个分数中，分母越小分层越多，分母越大分层越少。难道是计算不同大小的光照表面值的除法分出来的部分，而这个部分用floor()作为最高值而控制一块区域的颜色值？
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) * _HighLight * _Color + _WholeLight;//_WorldSpaceLightPos0 为Unity内置（"UnityCG.cginc"）的光照方向变量
                return col;
            }
            ENDCG
        }
    }
}

