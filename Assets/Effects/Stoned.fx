sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float2 uImageSize0;
float2 uImageSize1;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
        // 灰度 = r*0.3 + g*0.59 + b*0.11
    float gs = dot(float3(0.3, 0.59, 0.11), color.rgb);
    // 获取每个像素映射到石化贴图上的像素位置
    float d1x = floor(coords.x * uImageSize0.x) % uImageSize1.x;
    float d1y = floor(coords.y * uImageSize0.y) % uImageSize1.y;
    // 获取每个像素的正确大小
    float dx = 1 / uImageSize1.x;
    float dy = 1 / uImageSize1.y;
    if (gs > 0.26f)
        gs = tex2D(uImage1, float2(d1x * dx, d1y * dy)).r;
    else
        gs = 0.19f;
    return float4(gs, gs, gs, color.a);
}

technique Technique1
{
    pass Stoned
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}