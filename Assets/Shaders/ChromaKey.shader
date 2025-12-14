Shader "Custom/ChromaKey"
{
    Properties
    {
        _MainTex ("Video Texture", 2D) = "white" {}
        _KeyColor ("Key Color", Color) = (0, 1, 0, 1) // Green by default
        _Threshold ("Threshold", Range(0, 1)) = 0.4
        _Smoothness ("Smoothness", Range(0, 1)) = 0.1
        _Despill ("Despill Strength", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float4 _MainTex_ST;
            float4 _KeyColor;
            float _Threshold;
            float _Smoothness;
            float _Despill;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate color distance from key color
                float colorDist = distance(col.rgb, _KeyColor.rgb);

                // Create smooth alpha mask
                float alpha = smoothstep(_Threshold, _Threshold + _Smoothness, colorDist);

                // Green spill removal (despill)
                // Remove green color cast from semi-transparent edges
                float greenScreen = col.g - max(col.r, col.b);
                if (greenScreen > 0)
                {
                    // Reduce green channel based on despill strength
                    col.g = col.g - (greenScreen * _Despill);
                }

                // Apply alpha
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}
