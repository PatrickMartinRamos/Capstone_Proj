Shader "UI/MaskArea"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}   // required for UI
        _Color ("Color", Color) = (1,1,1,1)     // required for UI
    }

    SubShader
    {
        Tags { "Queue"="Transparent" }
        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
        }
    }
}