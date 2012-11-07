sampler texSampler : register(s0);

float2 offsetForPixels[15];
float weights[15];


float4 blurPixels(float2 texCoord : TEXCOORD0) : COLOR0
{

	float4 color = 0;

	for(int i = 0; i < 15; i++)
	{
		color += tex2D(texSampler, texCoord + offsetForPixels[i]) * weights[i];
	}
	return color;
}


technique ApplyBlur
{
    pass Pass1
    {

        PixelShader = compile ps_2_0 blurPixels();
    }
}
