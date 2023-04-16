// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _NoiseTex ("Noise Texture", 2D) = "white" {}
    _offset_color ("Offset Color", Vector) = (0.1180392,0.08,-0.3098039,1.0)
    _min_value ("Min Value", Range(0.0, 1.0)) = 0.0
    _max_value ("Max Value", Range(0.0, 1.0)) = 1.0
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _min_value;
            float _max_value;
            float4 _MainTex_ST;
            float4 _offset_color;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float noisex = floor(i.texcoord.x * 64.0) / 64.0;
                float2 noiseuv = (noisex, noisex);
                // i.texcoord.x *= 64;
                fixed4 col = tex2D(_MainTex, i.texcoord);
                fixed4 noise = tex2D(_NoiseTex,noiseuv);
                if(noise.r < _min_value || noise.r > _max_value)
                    col.a = 0;
                //offset 0.1180392,0.08,-0.3098039,1.0
                col = col + _offset_color;
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
        ENDCG
    }
}

}