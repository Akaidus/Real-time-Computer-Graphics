Shader "Unlit/TestShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Enchance("Enchance", Range(0,10)) = 0.5
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Transparent" }
        Tags { "Queue"="Transparent" }
        LOD 100

        Pass
        {
            //Cull Front

            ZWrite Off // For transparency (z-buffer writing)

            Blend SrcAlpha OneMinusSrcAlpha // For transparency (alpha blending)

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float4 posObjC : TEXCOORD2;

                float3 viewDir : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Enchance;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                unity_ObjectToWorld;

                //o.normal = v.normal;
                o.normal = mul(v.normal, (float3x3)unity_WorldToObject).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                
                o.posObjC = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 color = tex2D(_MainTex, i.uv) * _Color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);
                
                // float maskDiscard = i.posObjC.y < 0.0;
                // clip (maskDiscard - 0.001);

                float grayscale = (i.posObjC.x + i.posObjC.y + i.posObjC.z) / 3.0;
                float4 graycolor = float4(grayscale, grayscale, grayscale, 1.0);

                float4 dotProd = abs(dot((i.normal), (i.viewDir)));

                dotProd = pow(dotProd, _Enchance);

                float newOpacity = min(1.0, _Color.a / dotProd);
                // if(i.posObjC.y < 0)
                // {
                //     discard;
                // }

                //return color;
                //return i.posObjC;
                return _Color * graycolor * newOpacity;
            }
            ENDCG
        }
    }
}
