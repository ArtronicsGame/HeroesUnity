Shader "Custom/SelectionShader"
{
    Properties
    {
        _Texture("Texture", 2D) = "red" {}
        _Color("Color", Color) = (0, 0, 0, 0)
        _MinX("MinX", Float) = 0
        _MaxX("MaxX", Float) = 10
        _Error("Error", Float) = 3
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
    
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float4 position : SV_POSITION;
                float2 worldPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };
            
            fixed _MinX;
            fixed _MaxX;
            fixed _Error;
            
            fixed4 _Color;
            sampler2D _Texture;
            
            v2f vert(appdata IN){
                v2f OUT;                
                
                OUT.worldPos = mul (unity_ObjectToWorld, IN.vertex);
                OUT.position = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target {
                fixed4 pixel = tex2D(_Texture, IN.uv);
                if(IN.worldPos.x < _MinX + _Error)
                    if(IN.worldPos.x < _MinX)
                        pixel.a = 0.0;
                    else
                        pixel.a = ((IN.worldPos.x - _MinX) / _Error) ;
                if(IN.worldPos.x > _MaxX - _Error)
                    if(IN.worldPos.x > _MaxX)
                        pixel.a = 0.0;
                    else
                        pixel.a = ((_MaxX - IN.worldPos.x) / _Error) ;
                return pixel;
            }
            
            ENDCG
        }
    }
}
