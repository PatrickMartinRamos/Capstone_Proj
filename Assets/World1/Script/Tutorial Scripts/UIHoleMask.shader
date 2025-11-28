Shader "UI/HoleMask"
{
    Properties
    {
        _Center ("Hole Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Hole Radius", Float) = 0.2
        _Color ("Tint", Color) = (0,0,0,0.7)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _Center;
            float _Radius;
            float4 _Color;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float4 vertex : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Center.xy);

                if (dist < _Radius)
                    discard;   // <-- creates the hole

                return _Color;
            }
            ENDCG
        }
    }
}
