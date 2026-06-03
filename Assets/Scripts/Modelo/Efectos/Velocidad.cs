using UnityEngine;

[CreateAssetMenu(menuName = "Effects/VelocidadExtra")]
public class VelocidadEffect : EffectData
{
    public float velocidadExtra = 4f;

    public override void Apply(GameController controller)
    {
        controller.speedMulti += velocidadExtra;
    }

    public override void Remove(GameController controller)
    {
        controller.speedMulti -= velocidadExtra;
    }
}