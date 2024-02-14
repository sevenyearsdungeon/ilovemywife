Shader "Custom/My First Lighting Shader" {

	Properties{

		[KeywordEnum(Classic, Periodic, Simplex)] _NoiseType("Noise Type", Float) = 0
		[KeywordEnum(None, Numerical, Analytical)] _Gradient("Gradient Method", Float) = 0
		[Toggle(THREED)] _ThreeD("3D", Float) = 0
		[Toggle(FRACTAL)] _Fractal("Fractal", Float) = 0
		_ScrollVector("Scroll Speed", Vector) = (1,0,0,0)
		_WarpSpeed("Warp Speed", Float) = 1
		_Density("Density", Float) = 1

		_MainTex("Albedo", 2D) = "white" {}


		_AlphaCutoff("Alpha Cutoff", Range(0, 1)) = 0.5


	}

		CGINCLUDE

#pragma multi_compile _NOISETYPE_CLASSIC _NOISETYPE_PERIODIC _NOISETYPE_SIMPLEX
#pragma multi_compile _GRADIENT_NONE _GRADIENT_NUMERICAL _GRADIENT_ANALYTICAL
#pragma multi_compile _ THREED
#pragma multi_compile _ FRACTAL

#include "UnityCG.cginc"
#include "ClassicNoise2D.hlsl"
#include "ClassicNoise3D.hlsl"
#include "SimplexNoise2D.hlsl"
#include "SimplexNoise3D.hlsl"

#define BINORMAL_PER_FRAGMENT
#if defined(_NOISETYPE_CLASSIC) || defined(_NOISETYPE_PERIODIC)

#if defined(_GRADIENT_ANALYTICAL)
#define NOISE_FUNC(coord, period) 0
#elif defined(_NOISETYPE_CLASSIC)
#define NOISE_FUNC(coord, period) ClassicNoise(coord)
#else // PNOISE
#define NOISE_FUNC(coord, period) PeriodicNoise(coord, period)
#endif

#endif

#if defined(_NOISETYPE_SIMPLEX)

#if defined(_GRADIENT_ANALYTICAL)
#define NOISE_FUNC(coord, period) SimplexNoiseGrad(coord)
#else
#define NOISE_FUNC(coord, period) SimplexNoise(coord)
#endif

#endif


			ENDCG

			SubShader{


					Pass {
						Tags {
							"LightMode" = "ShadowCaster"
						}

						CGPROGRAM

						#pragma target 3.0

						#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
						#pragma shader_feature _SMOOTHNESS_ALBEDO

						#pragma multi_compile_shadowcaster

						#pragma vertex ShadowVertexProgram
						#pragma fragment CustomFragmentShader

	#if defined(_RENDERING_FADE) || defined(_RENDERING_TRANSPARENT)
		#define SHADOWS_SEMITRANSPARENT 1
	#endif

	#if SHADOWS_SEMITRANSPARENT || defined(_RENDERING_CUTOUT)
		#if !defined(_SMOOTHNESS_ALBEDO)
			#define SHADOWS_NEED_UV 1
		#endif
	#endif

	float4 _Tint;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _AlphaCutoff;

	sampler3D _DitherMaskLOD;

	struct VertexData {
		float4 position : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
	};

	struct InterpolatorsVertex {
		float4 position : SV_POSITION;
			float2 uv : TEXCOORD0;
		#if defined(SHADOWS_CUBE)
			float3 lightVec : TEXCOORD1;
		#endif
	};

	struct Interpolators {
		#if SHADOWS_SEMITRANSPARENT
			UNITY_VPOS_TYPE vpos : VPOS;
		#else
			float4 positions : SV_POSITION;
		#endif
			float2 uv : TEXCOORD0;
		#if defined(SHADOWS_CUBE)
			float3 lightVec : TEXCOORD1;
		#endif
	};

	float GetAlpha(Interpolators i) {
		float alpha = _Tint.a;
			alpha *= tex2D(_MainTex, i.uv.xy).a;
		return alpha;
	}

	float4 _ScrollVector;
	float _WarpSpeed, _Density;

				float Fragment(float2 uv)
				{
					const float epsilon = 0.0001;

					uv = uv * 4 + _ScrollVector.xy * _Time.y;

					#if defined(_GRADIENT_ANALYTICAL) || defined(_GRADIENT_NUMERICAL)
						#if defined(THREED)
							float3 o = 0.5;
						#else
							float2 o = 0.5;
						#endif
					#else
						float o = 0.5;
					#endif

					float s = .2;
					float w = 1;

					#ifdef FRACTAL
					for (int i = 0; i < 8; i++)
					#endif
					{
						#if defined(THREED)
							float3 coord = float3(uv * s, _Time.y * _WarpSpeed);
							float3 period = float3(s, s, 1.0) * 2.0;
						#else
							float2 coord = uv * s;
							float2 period = s * 2.0;
						#endif

						#if defined(_GRADIENT_NUMERICAL)
							#if defined(THREED)
								float v0 = NOISE_FUNC(coord, period);
								float vx = NOISE_FUNC(coord + float3(epsilon, 0, 0), period);
								float vy = NOISE_FUNC(coord + float3(0, epsilon, 0), period);
								float vz = NOISE_FUNC(coord + float3(0, 0, epsilon), period);
								o += w * float3(vx - v0, vy - v0, vz - v0) / epsilon;
							#else
								float v0 = NOISE_FUNC(coord, period);
								float vx = NOISE_FUNC(coord + float2(epsilon, 0), period);
								float vy = NOISE_FUNC(coord + float2(0, epsilon), period);
								o += w * float2(vx - v0, vy - v0) / epsilon;
							#endif
						#else
							o += NOISE_FUNC(coord, period) * w;
						#endif

						s *= 2.0;
						w *= 0.5;
					}

					o = 0.75*saturate((o - (1 - _Density)) / _Density * (o > (1 - _Density)));
					#if defined(_GRADIENT_ANALYTICAL) || defined(_GRADIENT_NUMERICAL)
						#if defined(THREED)
							return float4(o, 1);
						#else
							return float4(o, 1, 1);
						#endif
					#else
						return float4(o,o,o,1);
					#endif
				}

				InterpolatorsVertex ShadowVertexProgram(VertexData v) {
					InterpolatorsVertex i;
	#if defined(SHADOWS_CUBE)
					i.position = UnityObjectToClipPos(v.position);
					i.lightVec =
						mul(unity_ObjectToWorld, v.position).xyz - _LightPositionRange.xyz;
	#else
					i.position = UnityClipSpaceShadowCasterPos(v.position.xyz, v.normal);
					i.position = UnityApplyLinearShadowBias(i.position);
	#endif

					i.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return i;
				}
	float4 CustomFragmentShader(Interpolators i) : SV_TARGET {
		float alpha = Fragment(i.uv * 4 + float2(0.2, 1) * 0 * _Time.y);
		#if defined(_RENDERING_CUTOUT)
			clip(alpha - _AlphaCutoff);
		#endif

		#if SHADOWS_SEMITRANSPARENT
			float dither =
				tex3D(_DitherMaskLOD, float3(i.vpos.xy * 0.25, alpha * 0.9375)).a;
			clip(dither - 0.01);
		#endif

		#if defined(SHADOWS_CUBE)
			float depth = length(i.lightVec) + unity_LightShadowBias.x;
			depth *= _LightPositionRange.w;
			return UnityEncodeCubeShadowDepth(depth);
		#else
			return 0;
		#endif
	}

						ENDCG
					}
		}
}