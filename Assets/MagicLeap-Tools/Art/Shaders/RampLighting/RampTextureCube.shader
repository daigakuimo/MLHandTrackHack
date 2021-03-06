// ---------------------------------------------------------------------
//
// Copyright (c) 2018-present, Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/terms/developer
//
// ---------------------------------------------------------------------

Shader "Magic Leap/Ramp/TextureCubemap" 
{
    Properties 
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Reflect ("Reflection Intensity", Range(0.0,5)) = 0.0
    }

    SubShader 
        {
        Tags {"Queue"="geometry" "RenderType"="Opaque"}
        LOD 200
        Lighting Off

        Pass 
        {  
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 pos : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                    float3 viewDir : TEXCOORD1;

                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                    half3 worldNormal : TEXCOORD4;
                    half3 worldPos : TEXCOORD2;
                    half3 viewDir : TEXCOORD1;

                    UNITY_VERTEX_OUTPUT_STEREO

                };

                sampler2D _MainTex;
                sampler2D _Ramp;
                float4 _MainTex_ST;
                float4 _Color;
                half _Reflect;
                samplerCUBE _RefCube;
                float _Intensity;
                float4 _LightVector;

                v2f vert (appdata v)
                {
                    v2f o;

                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_OUTPUT(v2f, o);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    o.pos = UnityObjectToClipPos (v.pos);
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld,v.pos).xyz;
                    o.viewDir = normalize(WorldSpaceViewDir(v.pos));

                    return o;
                }
            
                fixed4 frag (v2f i) : SV_Target
                {
                    half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                    half3 worldRefl = reflect(-worldViewDir,i.worldNormal);

                    half4 refcube = texCUBE(_RefCube, worldRefl).rgba*_Reflect;
                    float light = dot(i.worldNormal,_LightVector);
                    float rim = dot(i.worldNormal,i.viewDir);
                    float diff = (light*.5)+.5;
                    float2 brdfdiff = float2(rim, diff);
                    fixed4 BRDFLight = tex2D(_Ramp, brdfdiff.xy).rgba;

                    fixed4 col = tex2D(_MainTex, i.texcoord)*_Color;
                    col *= (BRDFLight*_Intensity);
                    col += refcube;

                    return col;
                }
            ENDCG
        }
    }
}