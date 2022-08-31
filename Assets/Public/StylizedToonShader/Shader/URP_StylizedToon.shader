Shader "URPStylizedToon"
{
    // The _BaseMap variable is visible in the Material's Inspector, as a field
    // called Base Map.
    Properties
    { 
       [Header(Main Map)]
        [Space(10)]

        _MainTex ("Main Map / Albedo ", 2D) = "white" {}
        _LightAttenuation("Light attenuation", Range(0,10)) = 1


        [Space(10)]
        [Header(Light Map)]
        [Space(10)]

        _LightMap ("Light Map", 2D) = "white" {}

        _LightMapAttenuation("Light Map attenuation", Range(0,3)) = 1


        [Space(10)]
        [Header(Toon Ramp)]
        [Space(10)]

        _ToonRampColor1("Toon Ramp Light Tones", Color) = (1.0,1.0,1.0)

		_ToonRampColor2("Toon Ramp Dark Tones", Color) = (0.5,0.5,1.0)

        _ToonRampColor1Range("Toon Ramp Light Range", Range(0,1)) = 0.4

		_ToonRampColor2Range("Toon Ramp Dark Range", Range(0,1)) = 0.3 


        [Space(10)]
        [Header(Dark Tones)]
        [Space(10)]

        _DarkTonesColor1("Dark Tones Color 1", Color) = (1.0,1.0,1.0)

		_DarkTonesColor2("Dark Tones Color 2", Color) = (0.0,0.0,0.0)

		_DarkTonesColor1Range("Sharpness", Range(0,1)) = 0.181

        _DarkTonesColor2Range("Range", Range(0,1)) = 0.052


        [Space(10)]
        [Header(Light Tones)]
        [Space(10)]

        _LightIntesity("Light Intensity", Range(0,10)) = 1

        _LightTonesColor1("Lights Tones Color 1", Color) = (1.0,1.0,1.0)

        _LightTonesColor2("Lights Tones Color 2", Color) = (0.0,0.0,0.0)

		_LightTonesColor1Range("Sharpness", Range(0,1)) = 0.042

        _LightTonesColor2Range("Range", Range(0,1)) = 0.063

        [Space(10)]
        [Header(Rim Light)]
        [Space(10)]

        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Thickness", Range(0, 1)) = 0.716


        [Space(10)]
        [Header(Outline)]
        [Space(10)]

        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_OutlineThickness ("Outline Thickness", Range (0, 0.1)) = .01
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

             #pragma multi_compile_fog
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma shader_feature _ALPHATEST_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal: NORMAL;
                float4 tangentOS : TANGENT;  
                float2 uvLight : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 viewDir  : TEXCOORD3;
                float2 uvLight:TEXCOORD2;
                float3 normal:NORMAL;
                float4 shadowCoord : TEXCOORD4;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_LightMap);
            SAMPLER(sampler_LightMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _LightMap_ST;

                // Light Map
                float _LightIntesity;
                float _LightMapAttenuation;
                

                float4 _ToonRampColor1;
			    float4 _ToonRampColor2;

                float _ToonRampColor1Range;
			    float _ToonRampColor2Range;

                // Dark Tones
			    float4 _DarkTonesColor1;
			    float4 _DarkTonesColor2;

			    float _DarkTonesColor1Range;
			    float _DarkTonesColor2Range;

                // Light Tones
                float _LightAttenuation;

                float4 _LightTonesColor1;
			    float4 _LightTonesColor2;

			    float _LightTonesColor1Range;
                float _LightTonesColor2Range;

                 // Rim Color
                float4 _RimColor;
                float _RimAmount;
            CBUFFER_END

            float3 GetWorldSpaceViewDirCustom(float3 positionWS) {
                if (unity_OrthoParams.w == 0) {
                // Perspective
                    return _WorldSpaceCameraPos - positionWS;
                } else {
                // Orthographic
                    float4x4 viewMat = GetWorldToViewMatrix();
                    return viewMat[2].xyz;
                }
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.worldPos = vertexInput.positionWS;
                OUT.viewDir = GetWorldSpaceViewDirCustom(vertexInput.positionWS);

                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normal, IN.tangentOS);
                OUT.normal = normalInput.normalWS;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.uvLight =  TRANSFORM_TEX(IN.uvLight, _LightMap);

                OUT.shadowCoord = GetShadowCoord(vertexInput);

                return OUT;
            }

            float4 ColorGradientRamp(half cel_factor, float colorRange0,float colorRange1, float colorRange2,float4 color0, float4 color1,float4 color2){
            	float positions[3] = {colorRange0,colorRange1,  colorRange2}; 
            	float4 colors[3] = {color2, color1, color0 }; 

            	float4 mainTextRampedWithLightColor = float4(1.0,1.0,1.0,1.0);

            	for(int i = 0; i <2; i++){

                	if(cel_factor >= positions[i] && cel_factor <positions[i+1]){
                    	float tempFactor = (cel_factor - positions[i])/(positions[i+1] - positions[i]);
                    	mainTextRampedWithLightColor =  lerp(colors[i], colors[i+1], tempFactor);
                	}else if (cel_factor > positions [3-1]){
                    	mainTextRampedWithLightColor = colors[3 -1];
                	}
            	}
            	return mainTextRampedWithLightColor;
       		}

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.normal) ;
                Light mainLight = GetMainLight(IN.shadowCoord);
                half NdotL = dot(normal, mainLight.direction);
                half CdotL = dot(normal, _WorldSpaceCameraPos) ;
                half3 viewDir = normalize(IN.viewDir) ;

                half4 mainText = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Toon Ramp
                half cel_factor;

                if (NdotL > _ToonRampColor1Range)
					cel_factor = float4(1.0,1.0,1.0,1.0);
				else if (NdotL > _ToonRampColor2Range)
					cel_factor = float4(0.5,0.5,0.5,1.0)  ;
				else
					cel_factor = float4(0.0,0.0,0.0,1.0);

                // Dark Tones
                float4 DarkTones = ColorGradientRamp(mainText.r, 0.0f, _DarkTonesColor1Range, _DarkTonesColor2Range, float4(1.0,1.0,1.0,1.0), _DarkTonesColor1, _DarkTonesColor2) ;
                DarkTones *= mainText;

                 // Light Tones
                float4 LightTones = _LightIntesity * ColorGradientRamp(mainText.g , 0.0f, _LightTonesColor1Range, _LightTonesColor2Range, _LightTonesColor1, _LightTonesColor2, float4(0.0f,0.0f,0.0f,1.0f)) ;
                LightTones *= mainText;

                // main text modified with dark tones and light tones
                float4 mainTextChanged = (DarkTones + LightTones  ) *  mainText;

                // toon ramp map                
                float4 rampColor = ColorGradientRamp(cel_factor, 0.0f, 0.5f, 1.0f, float4(1.0,1.0,1.0,1.0), _ToonRampColor1, _ToonRampColor2) ;

                // main text with ramped light colors and light color
                float4 mainTextRamped = rampColor * mainTextChanged * float4((mainLight.color + mainLight.distanceAttenuation ), 1.0);

                // Light Text
                float4 lightText = _LightMapAttenuation *  SAMPLE_TEXTURE2D(_LightMap, sampler_LightMap, IN.uvLight);

                // I get only the blu channel of the light map
                float4 mainTextRampedWithLight = max(lightText.b, mainTextRamped);
                mainTextRampedWithLight = lerp(mainTextRamped.b, mainTextRampedWithLight, 1.0f);

                // Rim Light
                float4 rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(NdotL, 0.5f);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor *  float4(mainLight.color, 1.0); 

                mainTextRampedWithLight += rim;

                return mainTextRampedWithLight;
            }
            ENDHLSL
        }

        Pass{

            Tags {"LightMode" = "UniversalForward"  "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline"  "IgnoreProjector" = "True" }

			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            


            float _OutlineThickness;
    
            float4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
                float4 positionHCS : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v) {

	            v2f o;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
	            float3 outLine = ( v.normal.xyz * _OutlineThickness * min( vertexInput.positionCS.w , 1.5 ) );
			    v.vertex.xyz += outLine;

                
                vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.positionHCS = vertexInput.positionCS;
	    
	            o.color = _OutlineColor;
	            return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return i.color;
            }

            ENDHLSL
        }
    }
}
