using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    public float duration;

    public abstract void Apply(GameController controller);
    public abstract void Remove(GameController controller);

}