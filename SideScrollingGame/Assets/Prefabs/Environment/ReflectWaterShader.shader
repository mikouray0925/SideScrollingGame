Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlendPara ("Blending Paramete", Range(0,1)) = 0.25
        _BlendColor ("Blending Color", Color) = (0,0,1,1)
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float _BlendPara;
            uniform float4 _BlendColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                v2f flipi = i;
                flipi.uv.y = 1.0 - flipi.uv.y;
                // sample the texture
                fixed4 col = tex2D(_MainTex, flipi.uv);
                // blend with Water surface color
                col = lerp(col, _BlendColor, _BlendPara);
                
                return col;
            }
            ENDCG
        }
    }
}
