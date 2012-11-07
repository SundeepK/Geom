sampler texSampler : register(s0);

float bloomThreshold;

float4 getOnlyBrightPixels(float2 texCoords : TEXCOORD0) : COLOR0
{

	float4 color = tex2D(texSampler, texCoords);

	return saturate((color-bloomThreshold)/(1-bloomThreshold));

}


technique BloomExtract
{

	pass pass1
	{
		PixelShader = compile ps_2_0 getOnlyBrightPixels();
	}
}