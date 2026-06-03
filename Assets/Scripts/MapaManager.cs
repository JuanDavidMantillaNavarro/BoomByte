using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class MapaManager : MonoBehaviour
{
    [Header("Objeto completo del mapa")]
    public GameObject mapCamera;

    private bool mapaAbierto = false;
    private bool botonPresionadoAnterior = false;

    void Start()
    {
        if (mapCamera != null)
        {
            mapCamera.SetActive(false);
        }
    }

    void Update()
    {
        bool teclaK =
            Keyboard.current != null &&
            Keyboard.current.kKey.wasPressedThisFrame;

        bool botonX =
            BotonXVR();

        if (teclaK)
        {
            ToggleMapa();
        }

        if (botonX && !botonPresionadoAnterior)
        {
            ToggleMapa();
        }

        botonPresionadoAnterior = botonX;
    }

    void ToggleMapa()
    {
        mapaAbierto = !mapaAbierto;

        if (mapCamera != null)
        {
            mapCamera.SetActive(mapaAbierto);
        }

        Debug.Log(
            mapaAbierto
            ? "MAPA ABIERTO"
            : "MAPA CERRADO"
        );
    }

    bool BotonXVR()
    {
        XRInputDevice leftHand =
            InputDevices.GetDeviceAtXRNode(
                XRNode.LeftHand
            );

        if (!leftHand.isValid)
            return false;

        bool botonX = false;

        return leftHand.TryGetFeatureValue(
            XRCommonUsages.primaryButton,
            out botonX
        ) && botonX;
    }
}