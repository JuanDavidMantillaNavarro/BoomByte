using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Slow")]
public class SlowEffect : EffectData
{
    public float slowPercent = 0.3f;

    public override void Apply(GameController controller)
    {
        controller.speedMulti -= slowPercent;
    }

    public override void Remove(GameController controller)
    {
        controller.speedMulti += slowPercent;
    }
}