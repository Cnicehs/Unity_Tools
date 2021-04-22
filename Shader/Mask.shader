Shader "Hidden/Mask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Texture",2D) = "white"{}
        _Cutoff("Alpha cutoff",Range(0,1)) = 0
    }
    SubShader
    {
        // No culling or depth
        // Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { 
			"Queue"="Transparent" 
		}
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Mask;
            float _Cutoff;

            fixed4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half4 mask = tex2D(_Mask, i.uv);
                clip(mask.a-_Cutoff);
                return col;
            }
            ENDCG
        }
    }
}
