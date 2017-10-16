// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:1,x:34494,y:32704,varn:node_1,prsc:2|diff-103-OUT,diffpow-103-OUT,spec-103-OUT,normal-103-OUT,emission-103-OUT,alpha-2804-A;n:type:ShaderForge.SFN_Fresnel,id:3,x:33320,y:32750,varn:node_3,prsc:2|NRM-80-OUT;n:type:ShaderForge.SFN_Power,id:4,x:33550,y:32815,varn:node_4,prsc:2|VAL-3-OUT,EXP-6-OUT;n:type:ShaderForge.SFN_Slider,id:6,x:33163,y:32911,ptovrint:False,ptlb:Rim Power,ptin:_RimPower,varn:node_2140,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0.1,cur:2.229323,max:6;n:type:ShaderForge.SFN_Tex2d,id:7,x:33550,y:32431,ptovrint:False,ptlb:Texture Diffuse,ptin:_TextureDiffuse,varn:node_5273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3a8c2afc0e345dc4f88b0e3489825077,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:26,x:33550,y:32627,ptovrint:False,ptlb:Main Color,ptin:_MainColor,varn:node_9244,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:27,x:33763,y:32541,varn:node_27,prsc:2|A-7-RGB,B-26-RGB;n:type:ShaderForge.SFN_Tex2d,id:62,x:33320,y:33032,ptovrint:False,ptlb:Rim Texture,ptin:_RimTexture,varn:node_3729,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-87-UVOUT;n:type:ShaderForge.SFN_Color,id:63,x:33320,y:33240,ptovrint:False,ptlb:Rim Color,ptin:_RimColor,varn:node_7598,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:64,x:33507,y:33147,varn:node_64,prsc:2|A-62-RGB,B-63-RGB;n:type:ShaderForge.SFN_ValueProperty,id:65,x:33507,y:33318,ptovrint:False,ptlb:Rim Intensity,ptin:_RimIntensity,varn:node_7209,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Multiply,id:66,x:33684,y:33129,varn:node_66,prsc:2|A-64-OUT,B-65-OUT;n:type:ShaderForge.SFN_Multiply,id:67,x:33955,y:32807,varn:node_67,prsc:2|A-4-OUT,B-66-OUT,C-2804-RGB;n:type:ShaderForge.SFN_NormalVector,id:80,x:33114,y:32701,prsc:2,pt:False;n:type:ShaderForge.SFN_Panner,id:87,x:33129,y:33009,varn:node_87,prsc:2,spu:0,spv:1|UVIN-90-UVOUT,DIST-93-OUT;n:type:ShaderForge.SFN_TexCoord,id:90,x:32768,y:32911,varn:node_90,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:93,x:32953,y:33061,varn:node_93,prsc:2|A-95-T,B-96-OUT;n:type:ShaderForge.SFN_Time,id:95,x:32768,y:33061,varn:node_95,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:96,x:32768,y:33229,ptovrint:False,ptlb:Rim Speed,ptin:_RimSpeed,varn:node_733,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Add,id:103,x:34128,y:32559,varn:node_103,prsc:2|A-27-OUT,B-67-OUT;n:type:ShaderForge.SFN_VertexColor,id:2804,x:33881,y:33050,varn:node_2804,prsc:2;proporder:26-7-63-62-6-65-96;pass:END;sub:END;*/

Shader "Langvv/Rim_Scroll_V_Emissive_AlphaSet" {
    Properties {
        _MainColor ("Main Color", Color) = (0.5,0.5,0.5,1)
        _TextureDiffuse ("Texture Diffuse", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (0.5,0.5,0.5,1)
        _RimTexture ("Rim Texture", 2D) = "white" {}
        _RimPower ("Rim Power", Range(0.1, 6)) = 2.229323
        _RimIntensity ("Rim Intensity", Float ) = 3
        _RimSpeed ("Rim Speed", Float ) = 0.5
		_AlphaSet ("Alpha Set", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            n "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float _RimPower;
            uniform sampler2D _TextureDiffuse; uniform float4 _TextureDiffuse_ST;
            uniform float4 _MainColor;
            uniform sampler2D _RimTexture; uniform float4 _RimTexture_ST;
            uniform float4 _RimColor;
            uniform float _RimIntensity;
            uniform float _RimSpeed;
			uniform float _AlphaSet;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.vertexColor.a *= _AlphaSet;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _TextureDiffuse_var = tex2D(_TextureDiffuse,TRANSFORM_TEX(i.uv0, _TextureDiffuse));
                float4 node_95 = _Time + _TimeEditor;
                float2 node_87 = (i.uv0+(node_95.g*_RimSpeed)*float2(0,1));
                float4 _RimTexture_var = tex2D(_RimTexture,TRANSFORM_TEX(node_87, _RimTexture));
                float3 node_103 = ((_TextureDiffuse_var.rgb*_MainColor.rgb)+(pow((1.0-max(0,dot(i.normalDir, viewDirection))),_RimPower)*((_RimTexture_var.rgb*_RimColor.rgb)*_RimIntensity)*i.vertexColor.rgb));
                float3 normalLocal = node_103;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = node_103;
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = pow(max( 0.0, NdotL), node_103) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 diffuseColor = node_103;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_103;
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,i.vertexColor.a);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            n "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform float _RimPower;
            uniform sampler2D _TextureDiffuse; uniform float4 _TextureDiffuse_ST;
            uniform float4 _MainColor;
            uniform sampler2D _RimTexture; uniform float4 _RimTexture_ST;
            uniform float4 _RimColor;
            uniform float _RimIntensity;
            uniform float _RimSpeed;
			uniform float _AlphaSet;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.vertexColor.a *= _AlphaSet;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _TextureDiffuse_var = tex2D(_TextureDiffuse,TRANSFORM_TEX(i.uv0, _TextureDiffuse));
                float4 node_95 = _Time + _TimeEditor;
                float2 node_87 = (i.uv0+(node_95.g*_RimSpeed)*float2(0,1));
                float4 _RimTexture_var = tex2D(_RimTexture,TRANSFORM_TEX(node_87, _RimTexture));
                float3 node_103 = ((_TextureDiffuse_var.rgb*_MainColor.rgb)+(pow((1.0-max(0,dot(i.normalDir, viewDirection))),_RimPower)*((_RimTexture_var.rgb*_RimColor.rgb)*_RimIntensity)*i.vertexColor.rgb));
                float3 normalLocal = node_103;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = node_103;
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = pow(max( 0.0, NdotL), node_103) * attenColor;
                float3 diffuseColor = node_103;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * i.vertexColor.a,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
