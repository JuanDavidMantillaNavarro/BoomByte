using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Slow")]
public class SlowEffect : EffectData
{
    public float slowPercent = 0.3f;
    
    public override void Apply(GameController controller)
    {
        var moveProvider = GameController.Instance.moveProvider;
        moveProvider.moveSpeed -= slowPercent;
    }

    public override void Remove(GameController controller)
    {
        var moveProvider = GameController.Instance.moveProvider;
        moveProvider.moveSpeed += slowPercent;
    }
}