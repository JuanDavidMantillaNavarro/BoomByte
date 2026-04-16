using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameController gameController;

    public void ApplyEffect(EffectData effect)
    {
        StartCoroutine(ApplyEffectRoutine(effect));
    }

    private IEnumerator ApplyEffectRoutine(EffectData effect)
    {
        effect.Apply(gameController);

        if (effect.duration > 0)
        {
            yield return new WaitForSeconds(effect.duration);
            effect.Remove(gameController);
        }
    }
}