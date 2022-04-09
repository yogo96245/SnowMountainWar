﻿Shader "FullScreen/FullScreenCustomPass"
{
    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    // The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
    // struct PositionInputs
    // {
    //     float3 positionWS;  // World space position (could be camera-relative)
    //     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
    //     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
    //     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
    //     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
    //     float  linearDepth; // View space Z coordinate                              : [Near, Far]
    // };

    // To sample custom buffers, you have access to these functions:
    // But be careful, on most platforms you can't sample to the bound color buffer. It means that you
    // can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
    // float4 SampleCustomColor(float2 uv);
    // float4 LoadCustomColor(uint2 pixelCoords);
    // float LoadCustomDepth(uint2 pixelCoords);
    // float SampleCustomDepth(float2 uv);

    // There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
    // you can check them out in the source code of the core SRP package.
#define c45 0.707107
#define c225 0.9238795
#define s225 0.3826834

#define MAXSAMPLES 16
		static float2 offsets[MAXSAMPLES] = {
			float2(1, 0),
			float2(-1, 0),
			float2(0, 1),
			float2(0, -1),

			float2(c45, c45),
			float2(c45, -c45),
			float2(-c45, c45),
			float2(-c45, -c45),

			float2(c225, s225),
			float2(c225, -s225),
			float2(-c225, s225),
			float2(-c225, -s225),
			float2(s225, c225),
			float2(s225, -c225),
			float2(-s225, c225),
			float2(-s225, -c225)
	};

	Properties
	{
		_SamplePrecision("Sampling Precision", Range(1, 3)) = 1
		_OutlineWidth("Outline Width", Float) = 1
		_OuterColor("Outer Color", Color) = (1, 1, 0, 1)

		_InnerColor("Inner Color", Color) = (1, 1, 0, 1)
		_Texture("Texture", 2D) = "black" { }
		_TextureSize("Texture Pixels Size", float) = 32
	}

	float4 FullScreenPass(Varyings varyings) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);
		float depth = LoadCameraDepth(varyings.positionCS.xy);
		PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);

		//CustomColorBuffer中本次处理的像素的颜色值
		float4 c = LoadCustomColor(posInput.positionSS);

		//调整采样次数以4, 8, 16幂次增长
		int sampleCount = min(2 * pow(2, _SamplePrecision), MAXSAMPLES);

		//计算每像素之间的uv差
		float2 uvOffsetPerPixel = 1.0 / _ScreenSize.xy;

		float4 outline = 0;
		for (uint i = 0; i < sampleCount; ++i)
		{
			//取sampleCount次采样中的最大值
			outline = max(SampleCustomColor(posInput.positionNDC + uvOffsetPerPixel * _OutlineWidth * offsets[i]), outline);
		}

		//去掉原本纯色块的部分
		outline *= _OuterColor * (1 - c.a);

		float d = LoadCustomDepth(posInput.positionSS);

		//进行深度的判断，如果判定为被遮挡则用我们设置的透明度，反之则为0。
		//0.000001为bias，避免浮点数的精度问题导致的误差。
		float alphaFactor = (depth > d + 0.000001) ? _BehindFactor : 0;
		//对InnerColorTexture进行采样
		float4 innerColor = SAMPLE_TEXTURE2D(_Texture, s_trilinear_repeat_sampler, posInput.positionSS / _TextureSize) * _InnerColor;

		innerColor.a *= alphaFactor;

		float4 output = 0;
		//将描边赋值给output
		output = lerp(output, _OuterColor * float4(outline.rgb, 1), outline.a);
		//将纯色色块覆盖的区域以InnerColor替代
		output = lerp(output, innerColor * float4(c.rgb, 1), c.a);

		return output;
	}

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "Custom Pass 0"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
