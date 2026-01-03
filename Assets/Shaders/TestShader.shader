Shader "Custom/TestShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _WaveAmplitude ("Wave Amplitude", Float) = 0.2
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveFrequency ("Wave Frequency", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

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
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            float _WaveAmplitude;
            float _WaveSpeed;
            float _WaveFrequency;

            v2f vert (appdata v)
            {
                // Initialize output structure
                v2f o;

                // Calculate wave distortion based on time
                float time = _Time.y * _WaveSpeed;
                float wave =
                    sin(v.vertex.x * _WaveFrequency + time) *
                    sin(v.vertex.z * _WaveFrequency + time);

                // Apply wave distortion to vertex position
                v.vertex.y += wave * _WaveAmplitude;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color; // Get the color as well
            }
            ENDCG
        }
    }
}
