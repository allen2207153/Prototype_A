// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effect/Alpha_Trail"
{
	Properties
	{
		_Float0("RG是纹理图，B是噪波，A是mask", Float) = 0
		[Enum(Less,0,Greater,1,Less or Equal,2,Greater or Equal,3,Equal,4,Not Equal,5,Always,6)]_ZtestMode("Ztest Mode", Int) = 2
		[Enum(Back,2,Front,1,Off,0)]_CullMode("Cull Mode", Int) = 0
		_R_Color("R_Color ", Color) = (1,1,1,1)
		_G_Color("G_Color ", Color) = (1,1,1,1)
		_main_tex("main_tex", 2D) = "white" {}
		_GB_Tiling("GB_Tiling", Vector) = (1,1,1,1)
		_R_Panner("R_Panner", Vector) = (0,0,0,0)
		_G_Panner("G_Panner", Vector) = (0,0,0,0)
		_NoisePanner("NoisePanner", Vector) = (0,0,0,0)
		_RGBA_Scale("RGBA_Scale", Vector) = (10,3,0.1,1)
		_A_TilingOffset("A_TilingOffset", Vector) = (0,0,0,0)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
	LOD 0

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull [_CullMode]
		ColorMask RGBA
		ZWrite Off
		ZTest [_ZtestMode]
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform half _Float0;
			uniform int _CullMode;
			uniform int _ZtestMode;
			uniform half4 _R_Color;
			uniform half4 _RGBA_Scale;
			uniform sampler2D _main_tex;
			uniform float4 _R_Panner;
			uniform half4 _main_tex_ST;
			uniform float4 _NoisePanner;
			uniform half4 _GB_Tiling;
			uniform float4 _G_Panner;
			uniform half4 _G_Color;
			uniform half4 _A_TilingOffset;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_color = v.color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
#endif
				half mulTime172 = _Time.y * _R_Panner.z;
				half2 appendResult171 = (half2(_R_Panner.x , _R_Panner.y));
				half2 uv0_main_tex = i.ase_texcoord1.xy * _main_tex_ST.xy + _main_tex_ST.zw;
				half2 panner169 = ( ( mulTime172 + _R_Panner.w ) * appendResult171 + uv0_main_tex);
				half mulTime188 = _Time.y * _NoisePanner.z;
				half2 appendResult186 = (half2(_NoisePanner.x , _NoisePanner.y));
				half2 appendResult222 = (half2(_GB_Tiling.z , _GB_Tiling.w));
				half2 uv0184 = i.ase_texcoord1.xy * appendResult222 + float2( 0,0 );
				half2 panner185 = ( ( mulTime188 + _NoisePanner.w ) * appendResult186 + uv0184);
				half mulTime226 = _Time.y * _G_Panner.z;
				half2 appendResult227 = (half2(_G_Panner.x , _G_Panner.y));
				half2 appendResult221 = (half2(_GB_Tiling.x , _GB_Tiling.y));
				half2 uv0223 = i.ase_texcoord1.xy * appendResult221 + float2( 0,0 );
				half2 panner225 = ( ( mulTime226 + _G_Panner.w ) * appendResult227 + uv0223);
				half2 appendResult232 = (half2(_A_TilingOffset.x , _A_TilingOffset.y));
				half2 appendResult233 = (half2(_A_TilingOffset.z , _A_TilingOffset.w));
				half2 uv0230 = i.ase_texcoord1.xy * appendResult232 + appendResult233;
				half4 appendResult157 = (half4(( ( i.ase_color * _R_Color * _RGBA_Scale.x * tex2D( _main_tex, ( panner169 + ( tex2D( _main_tex, panner185 ).b * _RGBA_Scale.z ) ) ).r ) + ( tex2D( _main_tex, panner225 ).g * _G_Color * _RGBA_Scale.y ) ).rgb , ( tex2D( _main_tex, uv0230 ).a * _RGBA_Scale.w )));
				
				
				finalColor = appendResult157;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18000
56;7;1829;1004;1632.798;-324.3321;1;True;True
Node;AmplifyShaderEditor.Vector4Node;220;-1668.898,-15.11166;Inherit;False;Property;_GB_Tiling;GB_Tiling;6;0;Create;True;0;0;False;0;1,1,1,1;1.45,0.4,1,0.45;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;187;-1743.927,547.6433;Float;False;Property;_NoisePanner;NoisePanner;9;0;Create;True;0;0;False;0;0,0,0,0;0.1,0.05,1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;222;-1335.49,90.8006;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;188;-1471.665,620.1636;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;197;-1189.915,642.0126;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;186;-1474.146,527.1047;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;170;-812.0162,-72.94581;Float;False;Property;_R_Panner;R_Panner;7;0;Create;True;0;0;False;0;0,0,0,0;-0.5,0.2,1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;184;-1065.109,460.4629;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;172;-574.1667,-43.19382;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;229;-1148.292,197.2351;Float;False;Property;_G_Panner;G_Panner;8;0;Create;True;0;0;False;0;0,0,0,0;-0.35,0.1,1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;185;-750.8024,487.0169;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;171;-570.2666,-132.8938;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;-824.8376,-363.1327;Inherit;True;0;46;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;221;-1336.184,-22.14728;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;183;-537.4728,482.6778;Inherit;True;Property;_Noise;Noise;5;0;Create;True;0;0;False;0;-1;None;2786e1440dc855d44881aff3ee1450be;True;0;False;white;Auto;False;Instance;46;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;219;-406.7198,717.7778;Inherit;False;Property;_RGBA_Scale;RGBA_Scale;10;0;Create;True;0;0;False;0;10,3,0.1,1;10,3,0.085,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-381.7667,-22.39384;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;226;-882.5811,265.1718;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;231;-776.9316,999.4427;Inherit;False;Property;_A_TilingOffset;A_TilingOffset;11;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;228;-599.5311,263.6208;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;169;-208.8165,-48.24579;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;189;-171.599,458.5584;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;223;-1123.095,16.63211;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;227;-885.062,172.1129;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;232;-534.7979,996.3321;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;233;-534.7979,1084.332;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;190;-7.692168,-44.05503;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;225;-385.3927,112.5343;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;49;212.1858,-234.6275;Half;False;Property;_R_Color;R_Color ;3;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;217;122.3247,125.4846;Inherit;True;Property;_main_tex2;main_tex;5;0;Create;True;0;0;False;0;-1;None;6aa278d5af275804a889f64ed7f6a351;True;0;False;white;Auto;False;Instance;46;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;215;206.2187,317.8849;Half;False;Property;_G_Color;G_Color ;4;0;Create;True;0;0;False;0;1,1,1,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;46;127.9286,-64.06422;Inherit;True;Property;_main_tex;main_tex;5;0;Create;True;0;0;False;0;-1;None;05e7758da7a075a43b0589d3aea91846;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;230;-262.9918,968.1354;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;142;239.68,-386.5024;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;796.3731,-74.0352;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;214;553.572,410.8775;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;210;140.7678,787.6674;Inherit;True;Property;_main_tex1;main_tex;5;0;Create;True;0;0;False;0;-1;None;6aa278d5af275804a889f64ed7f6a351;True;0;False;white;Auto;False;Instance;46;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;213;984.3835,-66.15543;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;211;569.6856,531.6014;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;161;1398.187,-126.6301;Float;False;Property;_ZtestMode;Ztest Mode;1;1;[Enum];Create;True;7;Less;0;Greater;1;Less or Equal;2;Greater or Equal;3;Equal;4;Not Equal;5;Always;6;0;True;0;2;2;0;1;INT;0
Node;AmplifyShaderEditor.IntNode;164;1397.787,-195.5297;Float;False;Property;_CullMode;Cull Mode;2;1;[Enum];Create;True;3;Back;2;Front;1;Off;0;0;True;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.DynamicAppendNode;157;1141.173,-71.88304;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;224;1398.014,-268.5287;Inherit;False;Property;_Float0;RG是纹理图，B是噪波，A是mask;0;0;Create;False;0;0;True;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;152;1394.758,-58.79367;Half;False;True;-1;2;ASEMaterialInspector;0;1;Effect/Alpha_Trail;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;2;True;164;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;161;True;True;0;False;-1;0;False;-1;True;2;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;222;0;220;3
WireConnection;222;1;220;4
WireConnection;188;0;187;3
WireConnection;197;0;188;0
WireConnection;197;1;187;4
WireConnection;186;0;187;1
WireConnection;186;1;187;2
WireConnection;184;0;222;0
WireConnection;172;0;170;3
WireConnection;185;0;184;0
WireConnection;185;2;186;0
WireConnection;185;1;197;0
WireConnection;171;0;170;1
WireConnection;171;1;170;2
WireConnection;221;0;220;1
WireConnection;221;1;220;2
WireConnection;183;1;185;0
WireConnection;173;0;172;0
WireConnection;173;1;170;4
WireConnection;226;0;229;3
WireConnection;228;0;226;0
WireConnection;228;1;229;4
WireConnection;169;0;15;0
WireConnection;169;2;171;0
WireConnection;169;1;173;0
WireConnection;189;0;183;3
WireConnection;189;1;219;3
WireConnection;223;0;221;0
WireConnection;227;0;229;1
WireConnection;227;1;229;2
WireConnection;232;0;231;1
WireConnection;232;1;231;2
WireConnection;233;0;231;3
WireConnection;233;1;231;4
WireConnection;190;0;169;0
WireConnection;190;1;189;0
WireConnection;225;0;223;0
WireConnection;225;2;227;0
WireConnection;225;1;228;0
WireConnection;217;1;225;0
WireConnection;46;1;190;0
WireConnection;230;0;232;0
WireConnection;230;1;233;0
WireConnection;212;0;142;0
WireConnection;212;1;49;0
WireConnection;212;2;219;1
WireConnection;212;3;46;1
WireConnection;214;0;217;2
WireConnection;214;1;215;0
WireConnection;214;2;219;2
WireConnection;210;1;230;0
WireConnection;213;0;212;0
WireConnection;213;1;214;0
WireConnection;211;0;210;4
WireConnection;211;1;219;4
WireConnection;157;0;213;0
WireConnection;157;3;211;0
WireConnection;152;0;157;0
ASEEND*/
//CHKSM=8ADA63C712C67636C9C28134F410DCDA9F11ABF3