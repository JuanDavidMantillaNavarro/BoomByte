using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class PanelInformacionNivel : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Fade")]
    public CanvasGroup canvasGroup;
    public float velocidadFade = 2f;

    bool abierto = false;

    private void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0;
    }

    private void Update()
    {
        if (!abierto)
            return;

        bool teclaT =
            Keyboard.current != null &&
            Keyboard.current.tKey.wasPressedThisFrame;

        bool botonA = BotonAVR();

        if (teclaT || botonA)
        {
            StartCoroutine(CerrarPanel());
        }
    }

    public void MostrarPanel()
    {
        if (abierto)
            return;

        abierto = true;

        panel.SetActive(true);

        StartCoroutine(FadeIn());

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

    IEnumerator FadeIn()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * velocidadFade;

            canvasGroup.alpha =
                Mathf.Lerp(0, 1, t);

            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    IEnumerator CerrarPanel()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * velocidadFade;

            canvasGroup.alpha =
                Mathf.Lerp(1, 0, t);

            yield return null;
        }

        canvasGroup.alpha = 0;

        panel.SetActive(false);

        abierto = false;

    }
}