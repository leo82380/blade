using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SplashEffectPlayer : EffectPlayer
{
    private readonly int _deltaYName = Shader.PropertyToID("YDelta");
    private readonly int _colorName = Shader.PropertyToID("Color");
    public void SetCustomData(Color color, float yDelta)
    {
        foreach (VisualEffect effect in _effects)
        {
            effect.SetFloat(_deltaYName, yDelta);
            effect.SetVector4(_colorName, color);
        }
    }
}
