// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SnowShader"
{
	Properties
	{
		_BaseAlbedo("Base Albedo", 2D) = "white" {}
		_BaseNormal("Base Normal", 2D) = "bump" {}
		_BaseMetallic("Base Metallic", 2D) = "white" {}
		_SnowAlbedo("Snow Albedo", 2D) = "white" {}
		_SnowNormal("Snow Normal", 2D) = "bump" {}
		_SnowMetallic("Snow Metallic", 2D) = "white" {}
		_SnowAmount("SnowAmount", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _BaseNormal;
		uniform float4 _BaseNormal_ST;
		uniform sampler2D _SnowNormal;
		uniform float4 _SnowNormal_ST;
		uniform float _SnowAmount;
		uniform sampler2D _BaseAlbedo;
		uniform float4 _BaseAlbedo_ST;
		uniform sampler2D _SnowAlbedo;
		uniform float4 _SnowAlbedo_ST;
		uniform sampler2D _BaseMetallic;
		uniform float4 _BaseMetallic_ST;
		uniform sampler2D _SnowMetallic;
		uniform float4 _SnowMetallic_ST;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_BaseNormal = i.uv_texcoord * _BaseNormal_ST.xy + _BaseNormal_ST.zw;
			float2 uv_SnowNormal = i.uv_texcoord * _SnowNormal_ST.xy + _SnowNormal_ST.zw;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 lerpResult15 = lerp( UnpackNormal( tex2D( _BaseNormal, uv_BaseNormal ) ) , UnpackNormal( tex2D( _SnowNormal, uv_SnowNormal ) ) , saturate( ( ase_worldNormal.y * _SnowAmount ) ));
			o.Normal = lerpResult15;
			float2 uv_BaseAlbedo = i.uv_texcoord * _BaseAlbedo_ST.xy + _BaseAlbedo_ST.zw;
			float2 uv_SnowAlbedo = i.uv_texcoord * _SnowAlbedo_ST.xy + _SnowAlbedo_ST.zw;
			float3 newWorldNormal19 = WorldNormalVector( i , lerpResult15 );
			float temp_output_18_0 = saturate( ( newWorldNormal19.y * _SnowAmount ) );
			float4 lerpResult10 = lerp( tex2D( _BaseAlbedo, uv_BaseAlbedo ) , tex2D( _SnowAlbedo, uv_SnowAlbedo ) , temp_output_18_0);
			o.Albedo = lerpResult10.rgb;
			float2 uv_BaseMetallic = i.uv_texcoord * _BaseMetallic_ST.xy + _BaseMetallic_ST.zw;
			float2 uv_SnowMetallic = i.uv_texcoord * _SnowMetallic_ST.xy + _SnowMetallic_ST.zw;
			float4 lerpResult17 = lerp( tex2D( _BaseMetallic, uv_BaseMetallic ) , tex2D( _SnowMetallic, uv_SnowMetallic ) , temp_output_18_0);
			o.Specular = lerpResult17.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14101
2567;29;1906;1004;1487.629;438.9379;1;True;True
Node;AmplifyShaderEditor.WorldNormalVector;20;-1685.906,-319.7951;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;12;-1773.599,-115.1001;Float;False;Property;_SnowAmount;SnowAmount;6;0;Create;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1444.505,-272.8951;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;-1283.806,-264.4951;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-1548.101,267.1998;Float;True;Property;_SnowNormal;Snow Normal;4;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-1548.101,72.7999;Float;True;Property;_BaseNormal;Base Normal;1;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-1067.3,73.59992;Float;False;3;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;19;-865.5047,-194.2967;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-651.7996,-116.3;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-693.0005,253.9001;Float;True;Property;_BaseMetallic;Base Metallic;2;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-811.4,-722.1001;Float;True;Property;_BaseAlbedo;Base Albedo;0;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-824.2003,-500.8003;Float;True;Property;_SnowAlbedo;Snow Albedo;3;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;18;-496.7049,-110.1974;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-698.3108,495.3673;Float;True;Property;_SnowMetallic;Snow Metallic;5;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;17;-223.4019,317.8009;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;10;-127.1,-308.6001;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;146.9001,42.89999;Float;False;True;2;Float;ASEMaterialInspector;0;0;StandardSpecular;SnowShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;3;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0.0,0,0;False;12;FLOAT3;0.0,0,0;False;14;FLOAT4;0.0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;20;2
WireConnection;21;1;12;0
WireConnection;22;0;21;0
WireConnection;15;0;4;0
WireConnection;15;1;14;0
WireConnection;15;2;22;0
WireConnection;19;0;15;0
WireConnection;11;0;19;2
WireConnection;11;1;12;0
WireConnection;18;0;11;0
WireConnection;17;0;2;0
WireConnection;17;1;16;0
WireConnection;17;2;18;0
WireConnection;10;0;1;0
WireConnection;10;1;9;0
WireConnection;10;2;18;0
WireConnection;0;0;10;0
WireConnection;0;1;15;0
WireConnection;0;3;17;0
ASEEND*/
//CHKSM=AAD552AFAB43F733C1D18C35FCECE73D56A3AFC0