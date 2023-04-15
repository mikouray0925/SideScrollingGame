Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Texture", 2D) = "white" {}
        _HighlightTex("Texture", 2D) = "white" {}
        _dudvmapTex("Texture", 2D) = "white" {}
        _BlendPara ("Blending Paramete", Range(0,1)) = 0.25
        _BlendColor ("Blending Color", Color) = (0,0,1,1)
        _PixelSize ("Pixel Size", float) = 0.25
        _speed ("Flow Speed", float) = 0.5
        _scale ("Water Scale", Int) = 1
        _opacity ("Water Opacity", Range(0, 0.1)) = 0.05
        _distortion ("Distortion", Range(0, 0.1)) = 0.05
        [MaterialToggle] _Pre_Pixelated("Pre_Pixelated", Int) = 0
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
            sampler2D _NoiseTex;
            sampler2D _HighlightTex;
            sampler2D _dudvmapTex;

            float4 _MainTex_ST;
            uniform float _BlendPara;
            uniform float4 _BlendColor;
            uniform float _PixelSize;
            uniform float _Pre_Pixelated;
            uniform float _distortion;
            // Flow Speed, increase to make the water flow faster.
            uniform float _speed = 1.0;
    
            // Water Scale, scales the water, not the background.
            uniform float _scale = 0.8;
    
            // Water opacity, higher opacity means the water reflects more light.
            uniform float _opacity = 0.5;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float avg(float4 color) {
                return (color.r + color.g + color.b)/3.0;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                v2f flipi = i;
                flipi.uv.y = 1.0 - flipi.uv.y;
                // sample the texture
                if(_Pre_Pixelated){
                    float dw = _PixelSize / _ScreenParams.x;
                    float dh = _PixelSize / _ScreenParams.y;
                    flipi.uv = float2(dw*floor(flipi.uv.x/dw),dh*floor(flipi.uv.y/dh));
                }

                float2 offsetuv = (tex2D(_dudvmapTex, flipi.uv + _Time.z*0.02).rg *2.0f - 1.0) * _distortion;
                //offsetuv.x -= 1.0f;
                //offsetuv.y -= 1.0f;
                //offsetuv *= _distortion;

                //float2 offseteduv = (flipi.uv.x , flipi.uv.y);
                fixed2 offseteduv = flipi.uv + offsetuv;
                fixed4 col = tex2D(_MainTex, offseteduv);
                
                //fixed4 col = tex2D(_MainTex, flipi.uv);
                // blend with Water surface color
                col = lerp(col, _BlendColor, _BlendPara);
                
                // Normalized pixel coordinates (from 0 to 1)
                float2 scaledUv = flipi.uv*_scale;
                
                // Water layers, layered on top of eachother to produce the reflective effect
                // Add 0.1 to both uv vectors to avoid the layers stacking perfectly and creating a huge unnatural highlight
                float4 water1 = tex2D(_NoiseTex, scaledUv + _Time.z*0.02*_speed - 0.1);
                float4 water2 = tex2D(_NoiseTex, scaledUv + _Time.z*_speed*float2(-0.02, -0.02) + 0.1);
                
                // Water highlights
                float4 highlights1 = tex2D(_HighlightTex, scaledUv + _Time.z*_speed / float2(-10, 100));
                float4 highlights2 = tex2D(_HighlightTex, scaledUv + _Time.y*_speed / float2(10, 100));
                
                // Background image - Replaced with col
                // float4 background = tex2D(iChannel1, float2(uv) + avg(water1) * 0.05);
                
                // Average the colors of the water layers (convert from 1 channel to 4 channel
                float avg1 = avg(water1);
                float avg2 = avg(water2);
                water1.rgb = float3(avg1, avg1, avg1);
                water2.rgb = float3(avg2, avg2, avg2);
                
                // Average and smooth the colors of the highlight layers
                float avg3 = avg(highlights1)/1.5;
                float avg4 = avg(highlights2)/1.5;
                highlights1.rgb = float3(avg3, avg3, avg3);
                highlights2.rgb = float3(avg4, avg4, avg4);
                
                float alpha = _opacity;
                
                if(avg(water1 + water2) > 0.3) {
                    alpha = 0.0;
                }
                
                if(avg(water1 + water2 + highlights1 + highlights2) > 0.75) {
                    alpha = 5.0 * _opacity;
                }

                // Output to screen
                fixed4 retColor = (water1 + water2) * alpha + col;
                //fixed4 retColor = col;
                return retColor;
            }


            ENDCG
        }
    }
}
