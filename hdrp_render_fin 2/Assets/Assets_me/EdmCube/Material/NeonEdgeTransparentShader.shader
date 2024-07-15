Shader "Custom/NeonEdgeTransparentShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _EdgeThickness("Edge Thickness", Range(0, 1)) = 0.05
        _Alpha("Alpha", Range(0, 1)) = 0.5
        _EdgeIntensity("Edge Intensity", Range(0, 1)) = 0.5
        _Speed("Speed", Range(0, 10)) = 2
        _MinIntensity("Minimum Intensity", Range(0, 1)) = 0.8
        _ColorSpeed("Color Speed", Range(0, 10)) = 5
        _Color("Edge Color", Color) = (1, 1, 1, 1)
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            float _EdgeThickness;
            float _Alpha;
            float _EdgeIntensity;
            float _Speed;
            float _MinIntensity;
            float _ColorSpeed = 0.1;
            float4 _Color;

            struct Input {
                float2 uv_MainTex;
            };

            float4 HSVtoRGB(float4 hsv) {
                float4 rgb = (1.0 - abs(2.0 * hsv.z - 1.0)) * hsv.y;
                float h = hsv.x * 6.0 + rgb.w;
                rgb.rgb = abs(h % 2.0 - 1.0);
                return ((rgb - 0.5) * min(hsv.z, rgb.w) * 2.0 + hsv.z).rgba;
            }

            void surf(Input IN, inout SurfaceOutput o) {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                float edge = min(IN.uv_MainTex.x, IN.uv_MainTex.y);
                edge = min(edge, 1.0 - IN.uv_MainTex.x);
                edge = min(edge, 1.0 - IN.uv_MainTex.y);
                edge = smoothstep(_EdgeThickness, 2 * _EdgeThickness, edge);
                float intensity = _MinIntensity + (_EdgeIntensity - _MinIntensity) * (0.5 + 0.5 * sin(_Time.y * _Speed));

                // Calculate the changing edge color over time
                float hue = fmod(_Time.y * _ColorSpeed, 1.0);
                float4 edgeColorHSV = float4(hue, 1, 1, 1);
                float4 edgeColorRGB = _Color * HSVtoRGB(edgeColorHSV);

                o.Emission = edgeColorRGB.rgb * intensity * (1 - edge) * 2;
                o.Alpha = _Alpha;
            }
            ENDCG
        }
            FallBack "Diffuse"
}


/* ¿§Áö¿¡ »ö±ò¸¸ ³ÖÀº ÄÚµå
Shader "Custom/NeonEdgeTransparentShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
        _EdgeThickness("Edge Thickness", Range(0, 1)) = 0.05
        _Alpha("Alpha", Range(0, 1)) = 0.5
        _EdgeIntensity("Edge Intensity", Range(0, 1)) = 0.5
        _Speed("Speed", Range(0, 10)) = 0.5
        _MinIntensity("Minimum Intensity", Range(0, 1)) = 0.2
    }

        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            float _EdgeThickness;
            float _Alpha;
            float4 _EdgeColor;
            float _EdgeIntensity;
            float _Speed;
            float _MinIntensity;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                float edge = min(IN.uv_MainTex.x, IN.uv_MainTex.y);
                edge = min(edge, 1.0 - IN.uv_MainTex.x);
                edge = min(edge, 1.0 - IN.uv_MainTex.y);
                edge = smoothstep(_EdgeThickness, 2 * _EdgeThickness, edge);
                float intensity = _MinIntensity + (_EdgeIntensity - _MinIntensity) * (0.5 + 0.5 * sin(_Time.y * _Speed));
                o.Emission = _EdgeColor.rgb * intensity * (1 - edge);
                o.Alpha = _Alpha;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
*/