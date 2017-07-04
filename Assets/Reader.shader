Shader "Hidden/Reader"
{
    Properties
    {
        _BufferTex("", 2DArray) = "" {}
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

            UNITY_DECLARE_TEX2DARRAY(_BufferTex);

            fixed4 frag(v2f_img i) : SV_Target
            {
                float3 uvw = float3(i.uv, frac(_Time.y) * 8);
                float br = UNITY_SAMPLE_TEX2DARRAY(_BufferTex, uvw).r;
                return br;
            }

            ENDCG
        }
    }
}
