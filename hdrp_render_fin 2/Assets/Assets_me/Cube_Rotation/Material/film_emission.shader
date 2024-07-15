Shader "CustomRenderTexture/film_emission"
{
    Properties{
        _MainTex("Video Texture", 2D) = "white" {}
        _EmissionThreshold("Emission Threshold", Range(0, 1)) = 0.9
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            float _EmissionThreshold;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                half3 c = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Albedo = c.rgb;
                o.Emission = c * step(_EmissionThreshold, c.r);
            }
            ENDCG
        }
            FallBack "Diffuse"

}