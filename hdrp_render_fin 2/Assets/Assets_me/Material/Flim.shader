Shader "Film" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0, 1)) = 0.1
        _HoleSize ("Hole Size", Range(0, 1)) = 0.1
    }

    SubShader {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        float _Radius;
        float _HoleSize;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            float2 center = float2(0.5, 0.5);
            float2 dist = IN.uv_MainTex - center;
            float radius = length(dist);

            float edge = fwidth(radius);
            float soft = smoothstep(_Radius - edge, _Radius + edge, radius);

            float2 hole1 = float2(0.2, 0.2);
            float2 hole2 = float2(0.8, 0.8);
            float2 hole3 = float2(0.2, 0.8);
            float2 hole4 = float2(0.8, 0.2);

            float hole = 1.0 - max(max(
                smoothstep(hole1.x - _HoleSize, hole1.x + _HoleSize, IN.uv_MainTex.x) * 
                smoothstep(hole1.y - _HoleSize, hole1.y + _HoleSize, IN.uv_MainTex.y),
                smoothstep(hole2.x - _HoleSize, hole2.x + _HoleSize, IN.uv_MainTex.x) * 
                smoothstep(hole2.y - _HoleSize, hole2.y + _HoleSize, IN.uv_MainTex.y)
            ), max(
                smoothstep(hole3.x - _HoleSize, hole3.x + _HoleSize, IN.uv_MainTex.x) * 
                smoothstep(hole3.y - _HoleSize, hole3.y + _HoleSize, IN.uv_MainTex.y),
                smoothstep(hole4.x - _HoleSize, hole4.x + _HoleSize, IN.uv_MainTex.x) * 
                smoothstep(hole4.y - _HoleSize, hole4.y + _HoleSize, IN.uv_MainTex.y)
            ));

            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a * hole * soft;
        }
        ENDCG
    }
    FallBack "Diffuse"
}