sampler bloomSampler : register(s0);
sampler baseSampler : register(s1);

float bloomInt;
float baseInt;

float bloomSat;
float baseSat;

float4 AdjustSaturation(float4 color, float saturation)
{
    // The constants 0.3, 0.59, and 0.11 are chosen because the
    // human eye is more sensitive to green light, and less to blue.
    float grey = dot(color, float3(0.3, 0.59, 0.11));

    return lerp(grey, color, saturation);
}

float4 ComputeOverallBloom(float2 texCoord: TEXCOORD0) : COLOR0
{
	float4 bloom = tex2D(bloomSampler, texCoord);
    float4 base = tex2D(baseSampler, texCoord);
    

    bloom = AdjustSaturation(bloom, bloomSat) * bloomInt;
    base = AdjustSaturation(base, baseSat) * baseInt;


    base *= (1 - saturate(bloom));
    
    // Combine the two images.
    return base + bloom;
}


technique BloomEffect
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 ComputeOverallBloom();
    }
}