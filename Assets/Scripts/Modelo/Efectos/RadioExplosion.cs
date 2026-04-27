using UnityEngine;

[CreateAssetMenu(menuName = "Effects/RadioExplosion")]
public class RadioExplosion : EffectData
{
    public float amount = 1f;

    public override void Apply(GameController controller)
    {
        controller.explosionRadiusModifier += amount;
        Debug.Log("Radio incr");
    }

    public override void Remove(GameController controller)
    {
        controller.explosionRadiusModifier -= amount;
        Debug.Log("Radio decr");
    }
}