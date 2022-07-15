sampler uImage0 : register(s0);
bool uReverseAlpha;

// 反色
float4 reverse(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    color.rgb = 1 - color.rgb;
    if (uReverseAlpha)
        color.a = 1 - color.a;
    return color;
}

technique Technique1
{
    pass ColorReverse
    {
        PixelShader = compile ps_2_0 reverse();
    }
}
