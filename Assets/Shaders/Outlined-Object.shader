Shader "Custom/Outlined_Object"
{
    Properties
    {
        _Color      ( "Main Color", Color ) = ( 0.5, 0.5, 0.5, 1)
        _MainTex    ( "Albedo (RGB)", 2D  ) = "white" {}

        [Header(Outline)]
        [Space(6)]
        // [Enum(UnityEngine.Rendering.CompareFunction)] _MaskZTest( "ZTest For Masking", Float ) = 0
        // [Enum(UnityEngine.Rendering.CompareFunction)] _FillZTest( "ZTest For Filling", Float ) = 0

        _OutlineColor( "Outline Color", Color          ) = ( 1, 1, 1, 1 )
        _OutlineWidth( "Outline Width", Range( 0, 10 ) ) = 2
    }

    SubShader
    {
        Tags { "Queue" = "Transparent+100" "RenderType" = "Transparent" "DisableBatching" = "True" }

        Pass 
        {
            Name "Mask"
            Cull Off
            ZTest /*[_MaskZTest] */ Always
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref 1
                Pass Replace
            }
        }

        Pass
        {
            Name "Fill"
            Cull Off
            ZTest /* [_FillZTest] */ Always
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            Stencil
            {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM
                #include "UnityCG.cginc"

                #pragma vertex Vert
                #pragma fragment Frag

                struct VertexInput
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float3 smoothNormal : TEXCOORD3;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct VertexToFragment
                {
                    float4 position : SV_POSITION;
                    fixed4 color : COLOR;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                uniform fixed4 _OutlineColor;
                uniform float  _OutlineWidth;

                VertexToFragment Vert( VertexInput input )
                {
                    VertexToFragment output;

                    UNITY_SETUP_INSTANCE_ID( input );
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( output );

                    float3 normal       = any( input.smoothNormal ) ? input.smoothNormal : input.normal;
                    float3 viewPosition = UnityObjectToViewPos( input.vertex );
                    float3 viewNormal   = normalize( mul( ( float3x3 )UNITY_MATRIX_IT_MV, normal ) );

                    output.position = UnityViewToClipPos( viewPosition + viewNormal * -viewPosition.z * _OutlineWidth / 1000.0 );
                    output.color = _OutlineColor;

                    return output;
                }

                fixed4 Frag( VertexToFragment input ) : SV_Target
                {
                    return input.color;
                }
            ENDCG
        }

        Pass // Normal Render.
        {
            ZWrite On

            Material
            {
                Diffuse[_Color]
                Ambient[_Color]
            }

            Lighting On    
            
            SetTexture[_MainTex]
            {
                ConstantColor[_Color]
            }

            SetTexture[_MainTex]
            {
                Combine previous * primary DOUBLE
            }
        }
    }
}