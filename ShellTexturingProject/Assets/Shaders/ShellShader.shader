Shader "Custom/ShellShader"
{
    //no properties
    SubShader
    {
        //https://docs.unity3d.com/Manual/shader-predefined-pass-tags-built-in.html
        //use the normal main directional lighting
        Tags { "LightMode" = "ForwardBase" }

        Pass{

            //Make it so you can see the shader on the backside
            Cull Off

            CGPROGRAM
            
            //declare my passes and assign a name for them
            #pragma vertex vert
            #pragma fragment frag

            //using the base lighting includes from the acerlola video
            //#include "Packages/com.unity.render-piplines.universal/ShaderLibrary/UnityInput.hlsl"
            #include "UnityPBSLighting.cginc"
            #include "AutoLight.cginc"

            //Create the structs 
            
            //This one gets passed into the vertex shader
            struct VertexData {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            //vertex to fragment struct
            struct vtf {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCORD1;
            };


            //Shader Variables

            int _ShellIndex;   
            int _ShellCount;
            float _ShellLength;
            float _Density;
            float _Angle1;
            float _Angle2;
            float2 _Dimensions;
            float3 _ShellColor1;
            float3 _ShellColor2;





            //Hashing function

            float hash(uint n) {
                // integer hash copied from Hugo Elias, output 0 -> 1
                n = (n << 13U) ^ n;
                n = n * (n * n * 15731U + 0x789221U) + 0x1376312589U;
                return float(n & uint(0x7fffffffU)) / float(0x7fffffff);
            }



            //Shader Passes

            vtf vert(VertexData v) {
                //create the return data structure
                vtf i;

                //normalize where the shell is in height from 0 -> 1
                float shellHeight = (float)_ShellIndex / (float)_ShellCount;

                shellHeight = pow(shellHeight, 1);

                //take the normal and extrude the shell along it
                v.vertex.xyz += v.normal.xyz * _ShellLength * shellHeight;

                //normalize by converting to world space
                i.normal = normalize(UnityObjectToWorldNormal(v.normal));

                //unused but in acerola code
                i.pos = UnityObjectToClipPos(v.vertex);

                //pass the uvs along
                i.uv = v.uv;

                //return the vtf
                return i;
            }

            float4 frag(vtf i) : SV_TARGET{

                float2 newUV = i.uv * _Density;

                float2 localUV = frac(newUV) * 2 - 1;

                float height = (float)_ShellIndex / (float)_ShellCount;

                if (_ShellIndex % 2 == 0) {

                    float s = sin(_Angle1);
                    float c = cos(_Angle1);

                    float2 q = mul(float2x2(c, s, -s, c), localUV);

                    float2 d = abs(q) - (_Dimensions - height) * 0.5;

                    if (d.x > 0 || d.y > 0) {
                        discard;
                    }

                    return float4(_ShellColor1, 1.0);
                }
                else {

                    float s = sin(_Angle2);
                    float c = cos(_Angle2);

                    float2 q = mul(float2x2(c, s, -s, c), localUV);

                    float2 d = abs(q) - (_Dimensions - height) * 0.5;

                    if (d.x > 0 || d.y > 0) {
                        discard;
                    }

                    return float4(_ShellColor2, 1.0);
                }
            }

            ENDCG
        }
    }
}
