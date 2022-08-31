// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

// Upgrade NOTE: commented mainTextRampedWithLight 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented mainTextRampedWithLight 'sampler2D unity_Lightmap', a built-in variable

Shader "StylizedToon"
{
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

		_ToonRampColor2("Toon Ramp Dark Tones", Color) = (1.0,1.0,1.0)

        _ToonRampColor1Range("Toon Ramp Light Range", Range(0,1)) = 1

		_ToonRampColor2Range("Toon Ramp Dark Range", Range(0,1)) = 1  


        [Space(10)]
        [Header(Dark Tones)]
        [Space(10)]

        _DarkTonesColor1("Dark Tones Color 1", Color) = (1.0,1.0,1.0)

		_DarkTonesColor2("Dark Tones Color 2", Color) = (1.0,1.0,1.0)

		_DarkTonesColor1Range("Sharpness", Range(0,1)) = 1

        _DarkTonesColor2Range("Range", Range(0,1)) = 1


        [Space(10)]
        [Header(Light Tones)]
        [Space(10)]

        _LightIntesity("Light Intensity", Range(0,10)) = 1

        _LightTonesColor1("Lights Tones Color 1", Color) = (1.0,1.0,1.0)

        _LightTonesColor2("Lights Tones Color 2", Color) = (1.0,1.0,1.0)

		_LightTonesColor1Range("Sharpness", Range(0,1)) = 1

        _LightTonesColor2Range("Range", Range(0,1)) = 1

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
        Tags { "RenderType"="Opaque"  }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
             #include "UnityLightingCommon.cginc" 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
                float2 uvLight : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 viewDir  : TEXCOORD3;
                float2 uvLight:TEXCOORD2;
                float3 normal:NORMAL;
            };

            // Main Map
            sampler2D _MainTex;
            float4 _MainTex_ST;

            // Light Map
            sampler2D _LightMap;
            float4 _LightMap_ST;
            float _LightIntesity;
            float _LightMapAttenuation;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvLight =  v.uvLight.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // Toon Ramp
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

            fixed4 ColorGradientRamp(half cel_factor, float colorRange0,float colorRange1, float colorRange2,float4 color0, float4 color1,float4 color2){
            	float positions[3] = {colorRange0,colorRange1,  colorRange2}; 
            	fixed4 colors[3] = {color2, color1, color0 }; 

            	fixed4 mainTextRampedWithLightColor = float4(1.0,1.0,1.0,1.0);

            	for(int i = 0; i <2; i++){

                	if(cel_factor >= positions[i] && cel_factor <positions[i+1]){
                    	fixed tempFactor = (cel_factor - positions[i])/(positions[i+1] - positions[i]);
                    	mainTextRampedWithLightColor =  lerp(colors[i], colors[i+1], tempFactor);
                	}else if (cel_factor > positions [3-1]){
                    	mainTextRampedWithLightColor = colors[3 -1];
                	}
            	}
            	return mainTextRampedWithLightColor;
       		}

            fixed4 frag (v2f i) : SV_Target
            {
                // Dot products for toon ramp, rimLight, fade light effect with camera
                float3 normal = normalize(i.normal) ;
                half NdotL = dot(normal, _WorldSpaceLightPos0);
                half CdotL = dot(normal, _WorldSpaceCameraPos) ;
                half3 viewDir = normalize(i.viewDir) ;

                // Light Attenuation
                float3 toLight = _WorldSpaceLightPos0 - i.worldPos;
                float d = length( toLight );
                float attenuation = clamp( _LightAttenuation / d, 0.0, 1.0);

                // Main Text
                half4 mainText = tex2D(_MainTex, i.uv) ;

                // Toon Ramp
                half cel_factor;

                if (NdotL > _ToonRampColor1Range)
					cel_factor = fixed4(1.0,1.0,1.0,1.0);
				else if (NdotL > _ToonRampColor2Range)
					cel_factor = fixed4(0.5,0.5,0.5,1.0)  ;
				else
					cel_factor = fixed4(0.0,0.0,0.0,1.0);

                // Dark Tones
                fixed4 DarkTones = ColorGradientRamp(mainText.r, 0.0f, _DarkTonesColor1Range, _DarkTonesColor2Range, float4(1.0,1.0,1.0,1.0), _DarkTonesColor1, _DarkTonesColor2) ;
                DarkTones *= mainText;

                // Light Tones
                fixed4 LightTones = _LightIntesity * ColorGradientRamp(mainText.g , 0.0f, _LightTonesColor1Range, _LightTonesColor2Range, _LightTonesColor1, _LightTonesColor2, float4(0.0f,0.0f,0.0f,1.0f)) ;
                LightTones *= mainText;
                
                // main text modified with dark tones and light tones
                fixed4 mainTextChanged = (DarkTones + LightTones  ) *  mainText;

                // toon ramp map                
                fixed4 rampColor = ColorGradientRamp(cel_factor, 0.0f, 0.5f, 1.0f, fixed4(1.0,1.0,1.0,1.0), _ToonRampColor1, _ToonRampColor2) ;

                // main text with ramped light colors and light color
                fixed4 mainTextRamped = rampColor * mainTextChanged * (_LightColor0 + attenuation);

                // Lightmap attenuation is 0 if camera is not normal to surface (Fade/ appear effect lighter parts)
                if(NdotL < -0.3f)
                    _LightMapAttenuation = 0;

                // Light Text
                fixed4 lightText = _LightMapAttenuation *  tex2D(_LightMap, i.uvLight);

                // I get only the blu channel of the light map
                fixed4 mainTextRampedWithLight = max(lightText.b, mainTextRamped);
                mainTextRampedWithLight = lerp(mainTextRamped.b, mainTextRampedWithLight, 1.0f);
                
                // Rim Light
                float4 rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(NdotL, 0.5f);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor *  _LightColor0; 
                
                mainTextRampedWithLight += rim;
           
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, mainTextRampedWithLight);

                return mainTextRampedWithLight;
            }
            ENDCG
        }

        Pass{

            
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v) {

	            v2f o;
	            float3 outLine = ( v.normal.xyz * _OutlineThickness * min( UnityObjectToClipPos(v.vertex).w , 1.5 ) );
			    v.vertex.xyz += outLine;
	            o.vertex = UnityObjectToClipPos(v.vertex);
	            o.color = _OutlineColor;
	            return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}
