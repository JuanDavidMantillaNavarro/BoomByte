using UnityEngine;

[CreateAssetMenu(menuName = "Effects/TiempoEffect")]
public class TiempoEffect : EffectData
{
    [Range(0.1f, 0.9f)]
    public float factorLentitud = 0.5f; // 50% más lento
    
    public override void Apply(GameController controller)
    {
        // Solo ralentiza el timer del juego, no la física
        controller.timerSpeed = factorLentitud;
    }

    public override void Remove(GameController controller)
    {
        controller.timerSpeed = 1f;
    }
}