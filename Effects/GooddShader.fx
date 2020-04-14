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
	coords.y = abs(uProgress - coords.y);
	coords.x = 1.0 - coords.x;
	float4 color = tex2D(uImage0, coords);
	color.r *= (1 + uColor.r * uOpacity);
	color.g *= (1 + uColor.g * uOpacity);
	color.b *= (1 + uColor.b * uOpacity);
	return color;
}


technique Technique1
{
	pass GooddShader
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}