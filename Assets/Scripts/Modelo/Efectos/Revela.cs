using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Revelar")]
public class RevelarEffect : EffectData
{
    public override void Apply(GameController controller)
    {
        GameController.Instance.RevelarEfecto(true);
    }

    public override void Remove(GameController controller)
    {
        GameController.Instance.RevelarEfecto(false);
    }
}