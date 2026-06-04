using UnityEngine;

[CreateAssetMenu(menuName = "Effects/InmuneLentitud")]
public class InmuneLentitudEffect : EffectData
{
    public override void Apply(GameController controller)
    {
        controller.inmuneLentitud = true;
    }

    public override void Remove(GameController controller)
    {
        controller.inmuneLentitud = false;
    }
}