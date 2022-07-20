float3 uColor;
float opacity;
sampler samplerTex : register(s1);

float2 offset;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float alpha = tex2D(samplerTex, coords).a;

	if(alpha == 0.0)
		return float4(uColor, opacity);

	return float4(0.0, 0.0, 0.0, 0.0);
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}