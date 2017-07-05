// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CellTranslateDownShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            fixed4 _Color; // low precision type is usually enough for colors

            struct fragmentInput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fragmentInput vert (appdata_base v)
            {
                fragmentInput o;

                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.texcoord.xy; // - fixed2(0.5,0.5);

                return o;
            }

            fixed4 frag(fragmentInput i) : SV_Target {

                return fixed4(_Color.r, _Color.g, _Color.b, (sin(i.pos.y/30 + (_Time[1]*5)) - 0.5) * 6);
            }

            ENDCG
        }
    }
}