Shader "Hidden/Reader"
{
    Properties
    {
        _TextureArray("", 2DArray) = "" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            UNITY_DECLARE_TEX2DARRAY(_TextureArray);

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv * float2(4, 2);
                float3 uvw = float3(frac(uv), dot(floor(uv), float2(1, 4)));
                return UNITY_SAMPLE_TEX2DARRAY(_TextureArray, uvw);
            }

            ENDCG
        }
    }
}
