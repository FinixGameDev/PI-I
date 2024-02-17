Shader "Unlit/MatCapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Border ("Border", Range(0.1, 0.5)) = 0.4
        _Color ("Color Tint", Color) = (0.0, 0.0, 0.0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags {"LightMode" = "Always"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            //struct appdata
            //{
            //    float4 vertex : POSITION;
            //    float2 uv : TEXCOORD0;
            //};

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 cap : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            uniform float _Border;

            v2f vert (appdata_base v) // appdata v) 
            {
                v2f o;
                
                o.position = UnityObjectToClipPos(v.vertex);

                half2 capCoordinates;

                capCoordinates.x = dot(UNITY_MATRIX_IT_MV[0].xyz, v.normal);
                capCoordinates.y = dot(UNITY_MATRIX_IT_MV[1].xyz, v.normal);

                //float3 posView = mul(UNITY_MATRIX_MV, v.vertex).xyz;
                //float3 normalView = mul(UNITY_MATRIX_IT_MV, v.normal).xyz;
                //float2 capCoordinates = cross(posView, normalView).xy;
                //capCoordinates.xy = capCoordinates.yx;
                //capCoordinates.x = -capCoordinates.x;

                o.cap = capCoordinates * _Border + 0.5f;

                float4 p = float4(v.vertex);

                float3 e = normalize(mul(UNITY_MATRIX_MV, p));
                float3 n = normalize(mul(UNITY_MATRIX_MV, float4(v.normal, 0)));

                float3 r = reflect(e, n);
                float m = 2. * sqrt(pow(r.x, 2) + pow(r.y, 2) + pow(r.z + 1, 2));

                capCoordinates = r.xy / m + 0.5;

                o.cap = capCoordinates;

                return o;
            }

            uniform sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 frag (v2f i) : COLOR
            {
                // sample the texture
                float3 result = tex2D(_MainTex, i.cap).rgb;

                return float4(result, 1);

               
            }
            ENDCG
        }
    }
}
