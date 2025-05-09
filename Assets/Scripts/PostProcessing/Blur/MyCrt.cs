using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class MyCrt : VolumeComponent
{
    public ColorParameter colorParameter = new ColorParameter(Color.white) { hdr = true };
    public ClampedFloatParameter offset =
        new ClampedFloatParameter(0.002f, -0.003f, 0.1f);
    public FloatParameter frequency =
        new FloatParameter(10);
    public FloatParameter stripeSpeed =
        new FloatParameter(10);
}