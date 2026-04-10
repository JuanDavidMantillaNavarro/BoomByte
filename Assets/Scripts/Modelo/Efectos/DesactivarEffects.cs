using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Disable Abilities")]
public class DisableAbilitiesEffect : EffectData
{
    public override void Apply(GameController controller)
    {
        controller.abilitiesDisabled = true;
    }

    public override void Remove(GameController controller)
    {
        controller.abilitiesDisabled = false;
    }
}