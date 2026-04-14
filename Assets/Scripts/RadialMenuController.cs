using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class RadialMenuController : MonoBehaviour
{
    public GameObject radialMenu; // arrastra el menú radial
    public CanvasGroup canvasGroup;

    private InputDevice rightController;
    private bool lastButtonState = false;

    [Header("Simulador (Editor)")]
    public KeyCode teclaSimulador = KeyCode.B;

    void Start()
    {
        if (radialMenu != null)
            radialMenu.SetActive(false);

        GetRightController();
    }

    void GetRightController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
            rightController = devices[0];
    }

    void Update()
    {
        //Reintenta obtener el control si se pierde
        if (!rightController.isValid)
            GetRightController();

        bool buttonBPressed = false;

        // INPUT VR (Meta Quest)
        if (rightController.isValid)
        {
            rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonBPressed);
        }

        //INPUT SIMULADOR (teclado)
        bool simuladorPressed = Input.GetKeyDown(teclaSimulador) || Input.GetKeyDown(KeyCode.M);

        //Detectar pulsación (no mantener)
        if ((buttonBPressed && !lastButtonState) || simuladorPressed)
        {
            ToggleMenu();
        }

        lastButtonState = buttonBPressed;
    }

    void ToggleMenu()
    {
        bool active = !radialMenu.activeSelf;
        radialMenu.SetActive(active);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
            canvasGroup.interactable = active;
        }

        Debug.Log("Radial Menu: " + (active ? "ABIERTO" : "CERRADO"));
    }
}
