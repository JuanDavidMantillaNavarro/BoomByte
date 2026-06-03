using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class VRButtonTriggerUI : MonoBehaviour
{
    public enum TipoBoton
    {
        Reanudar,
        Salir,
        Sonido,
        Manual,
        VolverMenu,
        Pausa,
        ConfirmarSalir,
        CancelarSalir
    }

    public TipoBoton tipo;
    public VRMenuManager manager;

    public void Ejecutar()
    {

        Debug.Log("EJECUTAR LLAMADO");
        bool teclaT =
            Keyboard.current != null &&
            Keyboard.current.tKey.isPressed;

        bool botonA = BotonAVR();

        bool gatillo = GatilloVR();

        if (!teclaT && !botonA && !gatillo)
        {
            Debug.Log("Botón bloqueado: falta T, A o Trigger");
            return;
        }

        switch (tipo)
        {
            case TipoBoton.Reanudar:
                manager.ReanudarJuego();
                break;

            case TipoBoton.Pausa:
                manager.PausarJuego();
                break;

            case TipoBoton.Sonido:
                manager.MostrarSonido();
                break;

            case TipoBoton.Manual:
                manager.MostrarManual();
                break;

            case TipoBoton.VolverMenu:
                manager.MostrarRadial();
                break;

            case TipoBoton.Salir:
                manager.MostrarSalirConfirmacion();
                break;

            case TipoBoton.ConfirmarSalir:
                manager.SalirJuego();
                break;

            case TipoBoton.CancelarSalir:
                manager.CancelarSalir();
                break;
        }
    }

    bool BotonAVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonA = false;

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

        float trigger = 0f;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.trigger,
            out trigger
        ) && trigger > 0.7f;
    }
}