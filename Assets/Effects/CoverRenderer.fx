sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float2 leftTopInScreen; // 表示被覆盖物左上角在屏幕中的位置
float2 rightBottomInScreen; // 表示被覆盖物右下角在屏幕中的位置
float2 screenSize;


float2 Lerp(float2 value1, float2 value2, float2 amount)
{
    return float2(value1.x + (value2.x - value1.x) * amount.x, value1.y + (value2.y - value1.y) * amount.y);
}

//下面是描边shader改的，抄自fs49.org  Shader简介篇

float4 cover(float2 coords : TEXCOORD0) : COLOR0
{
    // 将uImage0坐标投影成cover坐标
    float2 coordInScreen = Lerp(leftTopInScreen, rightBottomInScreen, coords);
    float4 coverColor = tex2D(uImage1, coordInScreen);

    if (any(coverColor)) // 如果在cover上有颜色就直接返回指定颜色
        return coverColor; // 如果要背景图的动态效果可以写在这里（color = tex2D(uTex, float2(coords.x+uTime*0.01,coords.y+uTime*0.01));,uTime传时间）

    // 否则返回原本的颜色
    return tex2D(uImage0, coords);
}

technique Technique1
{
    pass CoverRenderer
    {
        PixelShader = compile ps_2_0 cover();
    }
}
