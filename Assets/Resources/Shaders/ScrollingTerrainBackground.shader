Shader "Custom/Scrolling Terrain Background"
{
    Properties
    {
        [MainTexture] _MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _UseTextureNoise ("Use Texture Noise", Float) = 0
        _NoiseScale ("Noise Scale", Float) = 6
        _Pixelation ("Pixelation Amount", Float) = 256
        _Brightness ("Brightness", Range(0, 1)) = 0.82
        _Saturation ("Saturation", Range(0, 2)) = 1
        _ScrollSpeed ("Scroll Speed", Vector) = (0.02, 0.0, 0.0, 0.0)

        _DeepSeaThreshold ("Deep Sea Threshold", Range(0, 1)) = 0.4
        _SeaThreshold ("Sea Threshold", Range(0, 1)) = 0.47
        _ShallowSeaThreshold ("Shallow Sea Threshold", Range(0, 1)) = 0.54
        _SandThreshold ("Sand Threshold", Range(0, 1)) = 0.555
        _GrassThreshold ("Grass Threshold", Range(0, 1)) = 0.6
        _ForestThreshold ("Forest Threshold", Range(0, 1)) = 0.65
        _MountainThreshold ("Mountain Threshold", Range(0, 1)) = 0.71
        _SnowThreshold ("Snow Threshold", Range(0, 1)) = 0.9

        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"

            #pragma vertex TerrainVertex
            #pragma fragment TerrainFragment
            #pragma multi_compile_instancing
            #pragma multi_compile _ DEBUG_DISPLAY SKINNED_SPRITE

            struct Attributes
            {
                COMMON_2D_INPUTS
                half4 color : COLOR;
                UNITY_SKINNED_VERTEX_INPUTS
            };

            struct Varyings
            {
                COMMON_2D_OUTPUTS
                half4 color : COLOR;
            };

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/2DCommon.hlsl"

            TEXTURE2D(_AlphaTex);
            SAMPLER(sampler_AlphaTex);
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half4 _RendererColor;
                float4 _MainTex_ST;
                float4 _NoiseTex_ST;
                float4 _ScrollSpeed;
                float _UseTextureNoise;
                float _NoiseScale;
                float _Pixelation;
                float _Brightness;
                float _Saturation;
                float _EnableExternalAlpha;
                float _DeepSeaThreshold;
                float _SeaThreshold;
                float _ShallowSeaThreshold;
                float _SandThreshold;
                float _GrassThreshold;
                float _ForestThreshold;
                float _MountainThreshold;
                float _SnowThreshold;
            CBUFFER_END

            static const half4 DEEP_SEA = half4(0.5647h, 0.8784h, 0.9373h, 1.0h);
            static const half4 SEA = half4(0.678h, 0.910h, 0.957h, 1.0h);
            static const half4 SHALLOW_SEA = half4(0.792h, 0.941h, 0.976h, 1.0h);
            static const half4 SAND = half4(1.0h, 0.953h, 0.690h, 1.0h);
            static const half4 GRASS = half4(0.654h, 0.788h, 0.341h, 1.0h);
            static const half4 FOREST = half4(0.416h, 0.600h, 0.306h, 1.0h);
            static const half4 MOUNTAIN = half4(0.424h, 0.459h, 0.493h, 1.0h);
            static const half4 SNOW = half4(1.0h, 1.0h, 1.0h, 1.0h);

            float Hash21(float2 uv)
            {
                uv = frac(uv * float2(123.34, 456.21));
                uv += dot(uv, uv + 45.32);
                return frac(uv.x * uv.y);
            }

            float ValueNoise(float2 uv)
            {
                float2 cell = floor(uv);
                float2 local = frac(uv);
                float2 smooth = local * local * (3.0 - 2.0 * local);

                float a = Hash21(cell);
                float b = Hash21(cell + float2(1.0, 0.0));
                float c = Hash21(cell + float2(0.0, 1.0));
                float d = Hash21(cell + float2(1.0, 1.0));

                return lerp(lerp(a, b, smooth.x), lerp(c, d, smooth.x), smooth.y);
            }

            float FractalNoise(float2 uv)
            {
                float value = 0.0;
                float amplitude = 0.5;

                value += ValueNoise(uv) * amplitude;
                uv *= 2.03;
                amplitude *= 0.5;

                value += ValueNoise(uv) * amplitude;
                uv *= 2.01;
                amplitude *= 0.5;

                value += ValueNoise(uv) * amplitude;
                uv *= 2.02;
                amplitude *= 0.5;

                value += ValueNoise(uv) * amplitude;
                return saturate(value / 0.9375);
            }

            Varyings TerrainVertex(Attributes input)
            {
                UNITY_SKINNED_VERTEX_COMPUTE(input);
                SetUpSpriteInstanceProperties();
                input.positionOS = UnityFlipSprite(input.positionOS, unity_SpriteProps.xy);

                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformObjectToHClip(input.positionOS);
#if defined(DEBUG_DISPLAY)
                output.positionWS = TransformObjectToWorld(input.positionOS);
#endif
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color * _Color * _RendererColor * unity_SpriteColor;
                return output;
            }

            half4 GetBiomeColor(float heightValue)
            {
                if (heightValue < _DeepSeaThreshold)
                {
                    return DEEP_SEA;
                }

                if (heightValue < _SeaThreshold)
                {
                    return SEA;
                }

                if (heightValue < _ShallowSeaThreshold)
                {
                    return SHALLOW_SEA;
                }

                if (heightValue < _SandThreshold)
                {
                    return SAND;
                }

                if (heightValue < _GrassThreshold)
                {
                    return GRASS;
                }

                if (heightValue < _ForestThreshold)
                {
                    return FOREST;
                }

                if (heightValue < _MountainThreshold)
                {
                    return MOUNTAIN;
                }

                return SNOW;
            }

            half4 TerrainFragment(Varyings input) : SV_Target
            {
                float pixelation = max(_Pixelation, 1.0);
                float2 scrolledUv = input.uv + (_Time.y * _ScrollSpeed.xy);
                float2 gridUv = round(scrolledUv * pixelation) / pixelation;
                float proceduralNoise = FractalNoise(gridUv * max(_NoiseScale, 0.001));
                float textureNoise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, gridUv).r;
                float heightValue = lerp(proceduralNoise, textureNoise, step(0.5, _UseTextureNoise));
                half4 biomeColor = GetBiomeColor(heightValue);
                half4 spriteSample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                half alphaSample = spriteSample.a;

                if (_EnableExternalAlpha > 0.5)
                {
                    alphaSample = SAMPLE_TEXTURE2D(_AlphaTex, sampler_AlphaTex, input.uv).r;
                }

                biomeColor.rgb *= _Brightness;
                half luminance = dot(biomeColor.rgb, half3(0.299h, 0.587h, 0.114h));
                biomeColor.rgb = lerp(luminance.xxx, biomeColor.rgb, _Saturation);
                biomeColor.rgb *= input.color.rgb;
                biomeColor.a *= alphaSample * input.color.a;

#if defined(DEBUG_DISPLAY)
                SurfaceData2D surfaceData;
                InputData2D inputData;
                half4 debugColor = 0;

                InitializeSurfaceData(biomeColor.rgb, biomeColor.a, surfaceData);
                InitializeInputData(input.uv, inputData);
                SETUP_DEBUG_TEXTURE_DATA_2D_NO_TS(inputData, input.positionWS, input.positionCS, _MainTex);

                if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
                {
                    return debugColor;
                }
#endif

                return biomeColor;
            }
            ENDHLSL
        }
    }
}