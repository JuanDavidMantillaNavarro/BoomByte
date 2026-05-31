using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using TMPro;
using FMODUnity;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class ProfesorInteractivo : MonoBehaviour
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

    [Header("Timer")]
    public TimerTrigger timerTrigger;

    [Header("Power Up")]
    public ProfesorPowerUp powerUpAlCerrar;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference profesorInteractSound;

    [Header("UI PowerUp")]
    public UIIconoPowerUp uiPowerUp;

    private bool activo = false;
    private bool yaSeActivo = false;
    private bool cerrando = false;

    private float tiempoInicio;
    private float velocidadOriginal;

    void Start()
    {
        GameObject canvasPadre = GameObject.Find("CanvaProfesores");

        if (canvasPadre != null)
        {
            if (canvasProfesor == null)
                canvasProfesor = canvasPadre;

            if (fadeCanvas == null)
                fadeCanvas = canvasPadre.GetComponent<CanvasGroup>();

            if (textoDialogo == null)
            {
                Transform txt = canvasPadre.transform.Find("TextoDialogo");

                if (txt != null)
                    textoDialogo = txt.GetComponent<TMP_Text>();
            }
        }

        if (canvasProfesor != null)
            canvasProfesor.SetActive(false);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;

        if (moveProvider != null)
            velocidadOriginal = moveProvider.moveSpeed;

        yaSeActivo = false;

        Debug.Log("Profesor reutilizable listo con soporte FMOD");
    }

    void Update()
    {
        if (jugador == null)
            return;

        float distancia =
            Vector3.Distance(jugador.position, transform.position);

        if (distancia <= distanciaActivacion &&
            !activo &&
            !yaSeActivo)
        {
            ActivarDialogo();
        }

        if (!activo)
            return;

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

        RuntimeManager.PlayOneShot(
            profesorInteractSound,
            transform.position
        );

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        if (moveProvider != null)
            moveProvider.moveSpeed = 0f;

        if (turnProvider != null)
            turnProvider.enabled = false;

        if (timerTrigger != null)
            timerTrigger.PausarTemporizador();

        StartCoroutine(FadeInDialogo());

        Debug.Log("DIÁLOGO ACTIVADO");
    }

    IEnumerator DesactivarDialogo()
    {
        if (!activo || cerrando)
            yield break;

        cerrando = true;
        activo = false;

        yield return StartCoroutine(FadeOutDialogo());

        Time.timeScale = 1f;

        if (moveProvider != null)
            moveProvider.moveSpeed = velocidadOriginal;

        if (turnProvider != null)
            turnProvider.enabled = true;

        yield return null;

        if (canvasProfesor != null)
            canvasProfesor.SetActive(false);

        if (timerTrigger != null)
            timerTrigger.ReanudarTemporizador();

        if (powerUpAlCerrar != null)
            powerUpAlCerrar.ActivarBeneficio();

        if (uiPowerUp != null)
            uiPowerUp.MostrarPowerUp();

        Debug.Log("DIÁLOGO CERRADO");

        cerrando = false;
    }

    IEnumerator FadeInDialogo()
    {
        if (canvasProfesor == null)
            yield break;

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

        if (textoDialogo != null)
            StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        textoDialogo.text = "";

        foreach (char letra in mensaje)
        {
            textoDialogo.text += letra;

            yield return new WaitForSecondsRealtime(
                velocidadEscritura
            );
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

        fadeCanvas.alpha = 0f;

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