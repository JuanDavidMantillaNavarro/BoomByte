using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
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
        "Hola, soy Freddy. Bienvenido al laboratorio.";

    public float velocidadEscritura = 0.03f;

    [Header("Detección")]
    public Transform jugador;
    public float distanciaActivacion = 2f;

    [Header("Tiempo")]
    public float duracionMaxima = 10f;
    public float duracionFade = 0.5f;

    [Header("Power Up")]
    public ProfesorPowerUp powerUpAlCerrar;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference profesorInteractSound;

    [Header("UI PowerUp")]
    public UIIconoPowerUp uiPowerUp;

    private bool activo = false;
    private bool yaSeActivo = false;
    private float tiempoDialogo = 0f;

    void Start()
    {
        Debug.Log("=== START PROFESOR ===");

        if (canvasProfesor != null)
        {
            Debug.Log("Canvas asignado: " + canvasProfesor.name);
            canvasProfesor.SetActive(false);
        }
        else
        {
            Debug.LogError("canvasProfesor es NULL");
        }

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
        }
        else
        {
            Debug.LogError("fadeCanvas es NULL");
        }

        if (textoDialogo == null)
        {
            Debug.LogError("textoDialogo es NULL");
        }
    }

    void Update()
    {
        if (jugador == null)
            return;

        float distancia =
            Vector3.Distance(
                jugador.position,
                transform.position
            );

        if (
            distancia <= distanciaActivacion &&
            !activo &&
            !yaSeActivo
        )
        {
            ActivarDialogo();
        }

        if (!activo)
            return;

        tiempoDialogo += Time.deltaTime;

        bool teclaCerrar =
            Keyboard.current != null &&
            Keyboard.current.tKey.wasPressedThisFrame;

        bool botonA = BotonAVR();

        if (teclaCerrar || botonA)
        {
            StartCoroutine(
                DesactivarDialogo()
            );
        }

        if (tiempoDialogo >= duracionMaxima)
        {
            StartCoroutine(
                DesactivarDialogo()
            );
        }
    }

    void ActivarDialogo()
    {
        Debug.Log("=== ACTIVAR DIALOGO ===");

        activo = true;
        yaSeActivo = true;
        tiempoDialogo = 0f;

        RuntimeManager.PlayOneShot(
            profesorInteractSound,
            transform.position
        );

        StartCoroutine(
            FadeInDialogo()
        );

        Debug.Log("DIÁLOGO ACTIVADO");
    }

    IEnumerator FadeInDialogo()
    {
        Debug.Log("=== INICIO FADE ===");

        if (canvasProfesor == null)
        {
            Debug.LogError(
                "canvasProfesor es NULL"
            );

            yield break;
        }

        canvasProfesor.SetActive(true);

        Debug.Log(
            "SELF: " +
            canvasProfesor.activeSelf
        );

        Debug.Log(
            "HIERARCHY: " +
            canvasProfesor.activeInHierarchy
        );

        Debug.Log(
            "POSICION: " +
            canvasProfesor.transform.position
        );

        Debug.Log(
            "ESCALA LOCAL: " +
            canvasProfesor.transform.localScale
        );

        Debug.Log(
            "ESCALA GLOBAL: " +
            canvasProfesor.transform.lossyScale
        );

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 1f;

            Debug.Log(
                "ALPHA: " +
                fadeCanvas.alpha
            );
        }

        if (textoDialogo != null)
        {
            textoDialogo.enabled = true;
            textoDialogo.gameObject.SetActive(true);

            textoDialogo.text = mensaje;

            Debug.Log(
                "Texto asignado: " +
                mensaje
            );

            Debug.Log(
                "TMP activo: " +
                textoDialogo.gameObject.activeInHierarchy
            );
        }

        yield return null;
    }

    IEnumerator DesactivarDialogo()
    {
        if (!activo)
            yield break;

        activo = false;

        yield return StartCoroutine(
            FadeOutDialogo()
        );

        if (canvasProfesor != null)
            canvasProfesor.SetActive(false);

        yield return null;

        if (powerUpAlCerrar != null)
        {
            Debug.Log(
                "ACTIVANDO POWERUP"
            );

            powerUpAlCerrar
                .ActivarBeneficio();
        }

        if (uiPowerUp != null)
        {
            Debug.Log(
                "INTENTANDO MOSTRAR ICONO"
            );

            uiPowerUp.gameObject
                .SetActive(true);

            Debug.Log(
                "GameObject icono activo: " +
                uiPowerUp.gameObject
                    .activeInHierarchy
            );

            uiPowerUp.MostrarPowerUp();
        }

        Debug.Log(
            "DIÁLOGO CERRADO"
        );
    }

    IEnumerator FadeOutDialogo()
    {
        if (fadeCanvas == null)
            yield break;

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    tiempo /
                    duracionFade
                );

            yield return null;
        }

        fadeCanvas.alpha = 0f;
    }

    bool BotonAVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(
                XRNode.RightHand
            );

        if (!rightHand.isValid)
            return false;

        bool botonA = false;

        return
            rightHand.TryGetFeatureValue(
                XRCommonUsages.primaryButton,
                out botonA
            ) &&
            botonA;
    }
}