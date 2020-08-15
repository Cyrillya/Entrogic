sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity : register(C0);
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	if (!any(color))
		return color;
	float4 white = float4(1, 1, 1, 1);
	float4 dis = float4(white.r - color.r, white.g - color.g, white.b - color.b, 1);
	return float4(color.r + dis.r * uOpacity, color.g + dis.g * uOpacity, color.b + dis.b * uOpacity, 1);
}

technique Technique1
{
    pass WhiteBlur
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}