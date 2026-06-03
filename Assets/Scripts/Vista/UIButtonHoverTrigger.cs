using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class UIButtonHoverTrigger : MonoBehaviour, IPointerEnterHandler
{
    public VRMenuManager menuManager;
    public string accion;

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool teclaT =
            Keyboard.current != null &&
            Keyboard.current.tKey.isPressed;

        bool botonA = BotonAVR();
        bool gatillo = GatilloVR();

        if (!teclaT && !botonA && !gatillo)
        {
            Debug.Log("Hover bloqueado");
            return;
        }

        Debug.Log("Hover aceptado: " + accion);

        switch (accion)
        {
            case "Sonido":
                menuManager.MostrarSonido();
                break;

            case "Salir":
                menuManager.MostrarSalirConfirmacion();
                break;

            case "Reanudar":
                menuManager.ReanudarJuego();
                break;

            case "Manual":
                menuManager.MostrarManual();
                break;
        }
    }

    bool BotonAVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonA;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.primaryButton,
            out botonA
        ) && botonA;
    }

    bool GatilloVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        float trigger;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.trigger,
            out trigger
        ) && trigger > 0.7f;
    }
}