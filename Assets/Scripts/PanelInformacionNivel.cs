using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

        bool botonA =
            Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame;

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