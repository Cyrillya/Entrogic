sampler uImage0 : register(s0); // The contents of the screen.
sampler uImage1 : register(s1); // Up to three extra textures you can use for various purposes (for instance as an overlay).
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // The position of the camera.
float2 uTargetPosition; // The "target" of the shader, what this actually means tends to vary per shader.
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect; // Doesn't seem to be used, but included for parity.
float2 uZoom;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
	float crgb = (color.r + color.g + color.b) / 3;
	if (crgb <= 0.38)
	{
		float4 black = { 0, 0, 0, 1 };
		return black;
	}
	float4 white = { 1, 1, 1, 1 };
	return white;
}

/*
float4 color = tex2D(uImage0, coords);
	float a = color.r + color.g + color.b;
	a /= 3;
	color = (a, a, a, a);
	return color;
*/

technique Technique1
{
	pass ReallyDark
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}