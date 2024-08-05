using UnityEngine;
using UnityEngine.VFX;

public class SplashEffectPlayer : EffectPlayer
{
    private readonly int _deltaYName = Shader.PropertyToID("YDelta");
    private readonly int _colorName = Shader.PropertyToID("Color");

    public void SetCustomData(Color color, float yDelta)
    {
        foreach (VisualEffect e in _effects)
        {
            e.SetFloat(_deltaYName, yDelta);
            e.SetVector4(_colorName, color);
        }
    }
}
