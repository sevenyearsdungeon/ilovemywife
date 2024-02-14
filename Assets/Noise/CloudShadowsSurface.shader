Shader "Custom/CloudShadowsSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        //[KeywordEnum(Classic, Periodic, Simplex)] _NoiseType("Noise Type", Float) = 0
        //[KeywordEnum(None, Numerical, Analytical)] _Gradient("Gradient Method", Float) = 0
        //[Toggle(THREED)] _ThreeD("3D", Float) = 0 
        //[Toggle(FRACTAL)] _Fractal("Fractal", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows alpha:fade


            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };

            fixed4 _Color;
            half _LowEnd;
            half _HighEnd;

            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                half cloud = clamp((c.r - _LowEnd) / (_HighEnd - _LowEnd), 0, 1);
                o.Albedo = cloud * _Color;

                o.Alpha = cloud * _Color.a;
            }
            ENDCG


    }
    FallBack "Diffuse"
}
