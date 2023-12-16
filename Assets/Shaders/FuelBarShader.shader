Shader "Unlit/FuelBarShader"
{
    Properties
    {
        _ColorA("ColorLeft",Color) = (0,1,0,1)
        _ColorB("ColorRight",Color) = (0,1,0,1)
        _FuelLeft("LeftFuel",float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolator
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            
            float4 _ColorA;
            float4 _ColorB;
            float _FuelLeft;
            

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolator i) : SV_Target
            {
                float4 col = (1-_FuelLeft)*_ColorA + _ColorB * _FuelLeft;
                return col;
            }

            ENDCG
        }
    }
}
