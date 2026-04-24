using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Increase Max Balls")]
public class IncreaseMaxBallsEffect : EffectData
{
    public int amount = 1;

    public override void Apply(GameController controller)
    {
        controller.MaxBolas += amount;
    }

    public override void Remove(GameController controller)
    {
        controller.MaxBolas -= amount;
    }
}