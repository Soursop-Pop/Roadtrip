Shader "Unlit/PixelSky_Flat"
{
    Properties
    {
        _MainTex ("Sky Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Opaque" }
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert(uint id : SV_VertexID)
            {
                float2 pos = float2((id << 1) & 2, id & 2);
                v2f o;
                o.uv = pos;
                o.pos = float4(pos * 2.0 - 1.0, 0, 1);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 texelSize = _MainTex_TexelSize.xy;
                float2 uv = floor(i.uv / texelSize) * texelSize;
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
