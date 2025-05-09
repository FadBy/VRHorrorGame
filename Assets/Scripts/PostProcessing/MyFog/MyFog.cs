using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class MyFog : VolumeComponent
{
    public ColorParameter fogColor = new ColorParameter(Color.white);
}