// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NewSurfaceShader"{
    Properties{
        _bg ("Background", 2D) = "white" {}
        _bg_blur_param("Blur Param", Range(0, 0.01)) = 0.01
    }
        SubShader{
            pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "unitycg.cginc"
                
                sampler2D _bg;
                float _bg_blur_param;


                struct v2f {
                    float4 pos: SV_POSITION;
                    float2 uv:TEXCOORD0;
                };

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    return o;
                }
                fixed4 frag(v2f i) :COLOR
                {

                    i.uv.x = i.uv.x * 2 - _Time.x * 0.1;
                    fixed4 sum = fixed4(0.0, 0.0, 0.0, 0.0);
                    sum += tex2D(_bg, half2(i.uv.x - _bg_blur_param, i.uv.y - _bg_blur_param));
                    sum += tex2D(_bg, half2(i.uv.x - _bg_blur_param, i.uv.y));
                    sum += tex2D(_bg, half2(i.uv.x - _bg_blur_param , i.uv.y + _bg_blur_param));
                    sum += tex2D(_bg, half2(i.uv.x, i.uv.y - _bg_blur_param));
                    sum += tex2D(_bg, half2(i.uv.x, i.uv.y));
                    sum += tex2D(_bg, half2(i.uv.x, i.uv.y + _bg_blur_param));
                    sum += tex2D(_bg, half2(i.uv.x + _bg_blur_param, i.uv.y - _bg_blur_param));
                    sum += tex2D(_bg, half2(i.uv.x + _bg_blur_param, i.uv.y));
                    sum += tex2D(_bg, half2(i.uv.x + _bg_blur_param, i.uv.y + _bg_blur_param));
                    sum /= 9.0;
                    return sum;
                }
                ENDCG
            }
        }
}