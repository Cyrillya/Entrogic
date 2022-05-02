sampler uImage0 : register(s0);

// 反色

float4 reverse(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    color.r = 1 - color.r;
    color.g = 1 - color.g;
    color.b = 1 - color.b;
    return color;
}

technique Technique1
{
    pass ColorReverse
    {
        PixelShader = compile ps_2_0 reverse();
    }
}
