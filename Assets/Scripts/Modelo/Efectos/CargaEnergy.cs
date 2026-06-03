using UnityEngine;

[CreateAssetMenu(menuName = "Effects/SlowCarga")]
public class SlowCargaEffect : EffectData
{
    [Header("Model")]
    public EnergyByteModel model; //Guarda datos del modelo (La bola)

    public override void Apply(GameController controller)
    {
        model.delayBeforeExplode = 1f;
    }

    public override void Remove(GameController controller)
    {
        model.delayBeforeExplode = 2f;
    }
}