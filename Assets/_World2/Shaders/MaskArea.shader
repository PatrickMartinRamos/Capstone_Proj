Shader "UI/MaskArea"
{
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
