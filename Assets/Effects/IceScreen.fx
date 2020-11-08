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
	/*float2 testCoords = { coords.x, coords.y };
	testCoords.y -= 60 / uScreenResolution.y;
	if (testCoords.y < 0 || testCoords.y > 1)
	{
		float4 black = { 0, 0, 0, 1 };
		return black;
	}
	testCoords.y += 120 / uScreenResolution.y;
	if (testCoords.y < 0 || testCoords.y > 1)
	{
		float4 black = { 0, 0, 0, 1 };
		return black;
	}*/
	float4 color = tex2D(uImage0, coords);
	if (color.r > 0.7)
		color.r *= (1 - 0.4 * uProgress); // 0.6
	else
		color.r *= (1 - 0.7 * uProgress); // 0.3
	color.g *= (1 - 0.1 * uProgress); // 0.9
	color.b *= (1 + 0.8 * uProgress); // 1.8
	return color;
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
	pass IceScreen
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}