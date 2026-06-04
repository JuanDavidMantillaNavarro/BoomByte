using UnityEngine;

[CreateAssetMenu(menuName = "Effects/VelocidadExtra")]
public class VelocidadEffect : EffectData
{
    public float velocidadExtra = 1.3f;
    public override void Apply(GameController controller)
    {
        var moveProvider = GameController.Instance.moveProvider;
        moveProvider.moveSpeed *= velocidadExtra;
    }

    public override void Remove(GameController controller)
    {
        var moveProvider = GameController.Instance.moveProvider;
        moveProvider.moveSpeed /= velocidadExtra;
    }
}