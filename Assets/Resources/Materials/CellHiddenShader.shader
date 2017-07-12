// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CellHiddenShader" {
    Properties {
        _Color ("Color", Color) = (1,0,0,0)
        _ColorBackground ("ColorBackground", Color) = (1,0,0,0)
        _OuterRadius("OuterRadius", Range(0.0,0.5)) = 0.5
        _InnerRadius("InnerRadius", Range(0.0, 0.5)) = 0.4
        _Dropoff("Dropoff", Range(0.01, 0.5)) = 0.1
        _Rays("Rays", int) = 2
        _Space("Space", Range(0, 1)) = 0.5
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
            fixed4 _ColorBackground;
            float _OuterRadius;
            float _InnerRadius;
            float _Dropoff;
            int _Rays;
            float _Space;

            struct fragmentInput {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fragmentInput vert (appdata_base v)
            {
                fragmentInput o;

                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = v.texcoord.xy - fixed2(0.5,0.5);

                return o;
            }

            fixed4 frag(fragmentInput i) : SV_Target {

                float a = atan(i.uv.y/i.uv.x);
                float distance = sqrt(pow(i.uv.x, 2) + pow(i.uv.y,2));

                // float alpha = _Color.a * antialias(_InnerRadius, distance, _OuterRadius, _Dropoff) ;

                if ( distance > _InnerRadius && distance < _OuterRadius && abs(sin(a * _Rays)) > _Space )
                    return fixed4(_Color.r, _Color.g, _Color.b, _Color.a );
                else if ( distance < (_OuterRadius + _Dropoff) )
                    return fixed4(_ColorBackground.r, _ColorBackground.g, _ColorBackground.b, _ColorBackground.a);
                else
                    return fixed4(1,1,1,0);
                }
            ENDCG
        }
    }
}