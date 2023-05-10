Shader "Highlight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HighlightStrength ("Strength", range(0, 0.2)) = 0.025
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _HighlightStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 oldVertex : TEXCOORD1;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.oldVertex = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.x - _HighlightStrength < 0 || i.uv.x + _HighlightStrength > 1 || i.uv.y - _HighlightStrength < 0 || i.uv.y + _HighlightStrength > 1)
                    return float4(1.,1.,1.,1.);
                return i.color;
            }
            ENDCG
        }
    }
}
