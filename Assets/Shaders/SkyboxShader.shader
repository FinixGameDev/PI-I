Shader "Custom/SkyboxShader"
{
    Properties
    {
        _cube("display name", Cube) = "defaulttexture" {}
    }

        SubShader
    {
     Cull off
        CGPROGRAM
            #pragma surface surf Lambert 


        struct Input {
        float3 worldRefl;
        };


        samplerCUBE _cube;
        void surf(Input IN, inout SurfaceOutput o) {
          o.Albedo = texCUBE(_cube,IN.worldRefl).xyz;
        }

        ENDCG
    }
        FallBack "Diffuse"
}
