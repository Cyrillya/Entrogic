sampler uImage0 : register(s0); // The contents of the screen.
sampler uImage1 : register(s1); // Up to three extra textures you can use for various purposes (for instance as an overlay).
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition; // The position of the camera.
float2 uTargetPosition; // The "target" of the shader, what this actually means tends to vary per shader.
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect; // Doesn't seem to be used, but included for parity.
float2 uZoom;
// 马赛克大小
float2 mosaicSize = float2(8, 8);
float4 dip_filter(float3x3 _filter, sampler2D _image, float2 _xy, float2 texSize)
{
    float delta_basic = 2.0;
    // 纹理坐标采样的偏移
    float2 _filter_pos_delta[3][3] =
    {
        { float2(-delta_basic, -delta_basic), float2(0, -delta_basic), float2(delta_basic, -delta_basic) },
        { float2(-delta_basic, 0.0), float2(0.0, 0.0), float2(delta_basic, 0.0) },
        { float2(-delta_basic, delta_basic), float2(0, delta_basic), float2(delta_basic, delta_basic) },
    };
    // 最终的输出颜色
    float4 final_color = float4(0.0, 0.0, 0.0, 0.0);
    // 对图像做滤波操作
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            // 计算采样点，得到当前像素附近的像素的坐标
            float2 _xy_new = float2(_xy.x + _filter_pos_delta[i][j].x, _xy.y + _filter_pos_delta[i][j].y);
            float2 _uv_new = float2(_xy_new.x / texSize.x, _xy_new.y / texSize.y);
            // 采样并乘以滤波器权重，然后累加
            final_color += tex2D(_image, _uv_new) * _filter[i][j];
        }
    }
    return final_color;
}
float k; // 控制参数，公式中k值。
float4 xposure(float4 _color, float gray, float ex)
{ // 重新调整场景的亮度
    float b = (4 * ex - 1);
    float a = 1 - b;
    float f = gray * (a * gray + b);
    return f * _color;
}
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    // 高斯模糊
    float2 intXY = float2(coords.x * uScreenResolution.x, coords.y * uScreenResolution.y);
    //用于模糊操作的滤波器
    float3x3 _smooth_fil = float3x3(
    1 / 9.0, 1 / 9.0, 1 / 9.0, 
    1 / 9.0, 1 / 9.0, 1 / 9.0,
    1 / 9.0, 1 / 9.0, 1 / 9.0);
    return dip_filter(_smooth_fil, uImage0, intXY, uScreenResolution);
    //===============
    // 浮雕
    /*float2 upLeftUV = float2(coords.x - 1.0 / uScreenResolution.x, coords.y - 1.0 / uScreenResolution.y);
    float4 bkColor = float4(0.5, 0.5, 0.5, 1.0);
    float4 curColor = tex2D(uImage0, coords);
    float4 upLeftColor = tex2D(uImage0, upLeftUV);
    //相减得到颜色的差
    float4 delColor = curColor - upLeftColor;
    //需要把这个颜色的差设置
    float h = 0.3 * delColor.x + 0.59 * delColor.y + 0.11 * delColor.z;
    float4 _outColor = float4(h, h, h, 0.0) + bkColor;
    return _outColor;*/
    //===============
    // 马赛克
    /*//得到当前纹理坐标相对图像大小整数值。
    float2 intXY = float2(coords.x * uScreenResolution.x, coords.y * uScreenResolution.y);
    //根据马赛克块大小进行取整。
    float2 XYMosaic = float2(int(intXY.x / mosaicSize.x) * mosaicSize.x, int(intXY.y / mosaicSize.y) * mosaicSize.y);
    //把整数坐标转换回纹理采样坐标
    float2 UVMosaic = float2(XYMosaic.x / uScreenResolution.x, XYMosaic.y / uScreenResolution.y);
    return tex2D(uImage0, UVMosaic);*/
    //===============
    // 描边(建议还是给单个图像的用)
    /*float2 intXY = float2(coords.x * uScreenResolution.x, coords.y * uScreenResolution.y);
    float3x3 _pencil_fil = float3x3(-0.5, -1.0, 0.0,
                                    -1.0, 0.0, 1.0,
                                   -0.0, 1.0, 0.5);
    float4 delColor = dip_filter(_pencil_fil, uImage0, intXY, uScreenResolution);
    float deltaGray = 0.3 * delColor.x + 0.59 * delColor.y + 0.11 * delColor.z;
    if (deltaGray < 0.0)
        deltaGray = -1.0 * deltaGray;
    deltaGray = 1.0 - deltaGray;
    return float4(deltaGray, deltaGray, deltaGray, 1.0);*/
}
technique Technique1
{
    pass Blur
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}