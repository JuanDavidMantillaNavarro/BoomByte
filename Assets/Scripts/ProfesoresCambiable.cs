using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using TMPro;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class ProfesorCambiable : MonoBehaviour
{
    [Header("Canvas del diálogo")]
    public GameObject canvasProfesor;
    public CanvasGroup fadeCanvas;

    [Header("Texto diálogo")]
    public TMP_Text textoDialogo;
    [TextArea(3, 6)]
    public string mensaje =
        "Hola, soy Freddy. Bienvenido al laboratorio. Sigue las instrucciones para continuar.";

    public float velocidadEscritura = 0.03f;

    [Header("Detección")]
    public Transform jugador;
    public float distanciaActivacion = 2f;

    [Header("Movimiento XR")]
    public ContinuousMoveProvider moveProvider;
    public ContinuousTurnProvider turnProvider;

    [Header("Tiempo")]
    public float duracionMaxima = 10f;
    public float duracionFade = 0.5f;

    [Header("Power Up")]
    public ProfesorPowerUp powerUpAlCerrar;

    private bool activo = false;
    private bool yaSeActivo = false;
    private float tiempoInicio;

    private float velocidadOriginal;

    void Start()
    {
        // REFERENCIAS AUTOMÁTICAS
        GameObject canvasPadre = GameObject.Find("CanvaProfesores");

        if (canvasPadre != null)
        {
            if (canvasProfesor == null)
                canvasProfesor = canvasPadre;

            if (textoDialogo == null)
            {
                Transform txt = canvasPadre.transform.Find("TextoDialogo");
                if (txt != null)
                    textoDialogo = txt.GetComponent<TMP_Text>();
            }

            if (fadeCanvas == null)
                fadeCanvas = canvasPadre.GetComponent<CanvasGroup>();
        }

        if (canvasProfesor != null)
            canvasProfesor.SetActive(false);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;

        if (moveProvider != null)
            velocidadOriginal = moveProvider.moveSpeed;

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

        if (moveProvider != null)
            moveProvider.moveSpeed = 0f;

        if (turnProvider != null)
            turnProvider.enabled = false;

        StartCoroutine(FadeInDialogo());

        Debug.Log("DIÁLOGO ACTIVADO");
    }

    IEnumerator DesactivarDialogo()
    {
        if (!activo) yield break;

        activo = false;

        yield return StartCoroutine(FadeOutDialogo());

        if (moveProvider != null)
            moveProvider.moveSpeed = velocidadOriginal;

        if (turnProvider != null)
            turnProvider.enabled = true;

        if (powerUpAlCerrar != null)
            powerUpAlCerrar.ActivarBeneficio();

        Debug.Log("DIÁLOGO CERRADO");
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

        // TEXTO ESCRIBIÉNDOSE
        if (textoDialogo != null)
            StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        textoDialogo.text = "";

        foreach (char letra in mensaje)
        {
            textoDialogo.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }
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
