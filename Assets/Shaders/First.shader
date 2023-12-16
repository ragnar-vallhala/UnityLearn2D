Shader "Unlit/First"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Freq ("Frequency",Range(0,5)) = 1
        _Color ("Color" ,Color) = (1,1,1,1)
        _Speed("Speed",Float) = 1
        _Phase("Phase", Range(0,3.14)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 normal: NORMAL;
            };

            struct Interpolater
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 normal: NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Freq;
            float4 _Color;
            float _Speed;
            float _Phase;

            
            Interpolater vert (MeshData v)
            {
             Interpolater o;
                v.vertex.y  = 0.4*sin(v.vertex.x*_Freq +_Speed* _Time.y) + 0.1*sin(v.vertex.z*_Freq +_Speed* _Time.y + _Phase);
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.normal.x = 0.4*_Freq*cos(v.vertex.x*_Freq +_Speed* _Time.y)*0.5+0.5;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag  (Interpolater i): SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return i.normal.x*_Color;
            }
            ENDCG
        }
    }
}
