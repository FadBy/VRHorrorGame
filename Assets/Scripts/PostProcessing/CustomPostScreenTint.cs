using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("CustomPostScreenTint")]
public class CustomPostScreenTint : VolumeComponent, IPostProcessComponent
{
    public FloatParameter tintIntensity = new FloatParameter(1f);
    public ColorParameter tintColor = new ColorParameter(Color.white);
    
    public bool IsActive() => true;
    public bool IsTileCompatible() => true;
}
