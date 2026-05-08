using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "Effects/InvControls")]

public class InvControls : EffectData
{
   
    public InputActionReference moveAction;
    public InputActionReference turnAction;
    public override void Apply(GameController controller)
    {
        if (moveAction != null)
        {
            moveAction.action.ApplyBindingOverride(
                0,
                new InputBinding
                {
                    overrideProcessors = "scaleVector2(x=-1,y=-1)"
                }
            );

            Debug.Log("Movimiento invertido");
        }

        if (turnAction != null)
        {
            turnAction.action.ApplyBindingOverride(
                0,
                new InputBinding
                {
                    overrideProcessors = "scaleVector2(x=-1,y=-1)"
                }
            );

            Debug.Log("Rotaci�n invertida");
        }
    }

    public override void Remove(GameController controller)
    {
        if (moveAction != null)
            moveAction.action.RemoveAllBindingOverrides();

        if (turnAction != null)
            turnAction.action.RemoveAllBindingOverrides();
    }
}