// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:8735,x:32523,y:32254|diff-3425-OUT;n:type:ShaderForge.SFN_Lerp,id:3425,x:32367,y:32843|A-9111-RGB,B-9162-RGB,T-8721-OUT;n:type:ShaderForge.SFN_Clamp01,id:8721,x:32269,y:33011|IN-707-OUT;n:type:ShaderForge.SFN_Power,id:707,x:32062,y:33075|VAL-8568-OUT,EXP-9100-OUT;n:type:ShaderForge.SFN_Divide,id:8568,x:31768,y:32969|A-3865-OUT,B-9103-OUT;n:type:ShaderForge.SFN_Distance,id:3865,x:31414,y:32977|A-6589-XYZ,B-9110-XYZ;n:type:ShaderForge.SFN_FragmentPosition,id:6589,x:31167,y:32829;n:type:ShaderForge.SFN_Vector1,id:2346,x:31599,y:33213,v1:20;n:type:ShaderForge.SFN_Slider,id:1231,x:32318,y:33321,ptlb:Strength,ptin:_Strength,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:9100,x:32013,y:33281|A-9101-OUT,B-1231-OUT;n:type:ShaderForge.SFN_Vector1,id:9101,x:32282,y:33264,v1:500;n:type:ShaderForge.SFN_Slider,id:9102,x:31535,y:33329,ptlb:distance,ptin:_distance,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:9103,x:31386,y:33195|A-2346-OUT,B-9102-OUT;n:type:ShaderForge.SFN_Vector4Property,id:9110,x:30908,y:33037,ptlb:root_position,ptin:_root_position,glob:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Tex2d,id:9111,x:31997,y:32446,ptlb:Inner,ptin:_Inner,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Desaturate,id:9124,x:32219,y:32238;n:type:ShaderForge.SFN_Tex2d,id:9162,x:31762,y:32602,ptlb:Outer,ptin:_Outer,ntxv:0,isnm:False;proporder:1231-9102-9110-9111-9162;pass:END;sub:END;*/

Shader "Custom/NewShader" {
    Properties {
        _Strength ("Strength", Range(0, 1)) = 1
        _distance ("distance", Range(0, 1)) = 0
        _root_position ("root_position", Vector) = (0,0,0,0)
        _Inner ("Inner", 2D) = "white" {}
        _Outer ("Outer", 2D) = "white" {}
        _Color ("color", Vector) = (0,0,0,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Strength;
            uniform float _distance;
            uniform float4 _root_position;
            uniform sampler2D _Inner; uniform float4 _Inner_ST;
            uniform sampler2D _Outer; uniform float4 _Outer_ST;
            uniform float4 _Color;
            
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz * _Color.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_9176 = i.uv0;
                float4 node_9111 = tex2D(_Inner,TRANSFORM_TEX(node_9176.rg, _Inner));
                finalColor +=  diffuseLight * lerp(node_9111.rgb,tex2D(_Outer,TRANSFORM_TEX(node_9176.rg, _Outer)).rgb,saturate(pow((distance(i.posWorld.rgb,_root_position.rgb)/(20.0*_distance)),(500.0*_Strength))));
/// Final Color:
                return fixed4(finalColor * 1.5f,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _Strength;
            uniform float _distance;
            uniform float4 _root_position;
            uniform sampler2D _Inner; uniform float4 _Inner_ST;
            uniform sampler2D _Outer; uniform float4 _Outer_ST;
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz * _Color.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_9177 = i.uv0;
                float4 node_9111 = tex2D(_Inner,TRANSFORM_TEX(node_9177.rg, _Inner));
                finalColor += diffuseLight * lerp(node_9111.rgb,tex2D(_Outer,TRANSFORM_TEX(node_9177.rg, _Outer)).rgb,saturate(pow((distance(i.posWorld.rgb,_root_position.rgb)/(20.0*_distance)),(500.0*_Strength))));
/// Final Color:
                return fixed4(finalColor * 2,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
