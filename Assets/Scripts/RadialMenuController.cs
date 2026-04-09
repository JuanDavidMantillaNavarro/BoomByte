using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class RadialMenuController : MonoBehaviour
{
    public GameObject radialMenu; // arrastra: "radial menu"
    public CanvasGroup canvasGroup;

    private InputDevice rightController;
    private bool lastButtonState = false;

    void Start()
    {
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
        if (!rightController.isValid)
            GetRightController();

        bool buttonBPressed;

        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonBPressed))
        {
            // Detecta SOLO cuando se presiona (no mantener)
            if (buttonBPressed && !lastButtonState)
            {
                ToggleMenu();
            }

            lastButtonState = buttonBPressed;
        }
    }

    void ToggleMenu()
    {
        bool active = !radialMenu.activeSelf;
        radialMenu.SetActive(active);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = active;
        }
    }
}
