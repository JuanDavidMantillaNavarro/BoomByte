using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;
using UnityEngine.XR;

public class ProfesorInteractivo : MonoBehaviour
{
    [Header("Canvas del di�logo")]
    public GameObject canvasProfesor;
    public CanvasGroup fadeCanvas;

    [Header("Detecci�n")]
    public Transform jugador;
    public float distanciaActivacion = 2f;

    [Header("Tiempo")]
    public float duracionMaxima = 10f;
    public float duracionFade = 0.5f;

    [Header("Power Up")]
    public ProfesorPowerUp powerUpAlCerrar;

    private bool activo = false;
    private bool yaSeActivo = false;
    private float tiempoInicio;

    void Start()
    {
        if (canvasProfesor != null)
            canvasProfesor.SetActive(false);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;

        Debug.Log("Profesor reutilizable listo");
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(
            jugador.position,
            transform.position
        );

        if (distancia <= distanciaActivacion && !activo && !yaSeActivo)
        {
            ActivarDialogo();
        }

        if (!activo) return;

        bool teclaCerrar =
            Keyboard.current != null &&
            Keyboard.current.tKey.wasPressedThisFrame;

        bool botonA = BotonAVR();

        if (teclaCerrar || botonA)
        {
            StartCoroutine(DesactivarDialogo());
        }

        if (Time.unscaledTime - tiempoInicio >= duracionMaxima)
        {
            StartCoroutine(DesactivarDialogo());
        }
    }

    void ActivarDialogo()
    {
        activo = true;
        yaSeActivo = true;
        tiempoInicio = Time.unscaledTime;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        StartCoroutine(FadeInDialogo());

        Debug.Log("DI�LOGO ACTIVADO");
    }

    IEnumerator DesactivarDialogo()
    {
        if (!activo) yield break;

        activo = false;

        yield return StartCoroutine(FadeOutDialogo());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        if (powerUpAlCerrar != null)
            powerUpAlCerrar.ActivarBeneficio();

        Debug.Log("DI�LOGO CERRADO");
    }

    IEnumerator FadeInDialogo()
    {
        canvasProfesor.SetActive(true);

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(0f, 1f, tiempo / duracionFade);

            yield return null;
        }

        fadeCanvas.alpha = 1f;
    }

    IEnumerator FadeOutDialogo()
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(1f, 0f, tiempo / duracionFade);

            yield return null;
        }

        canvasProfesor.SetActive(false);
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
}