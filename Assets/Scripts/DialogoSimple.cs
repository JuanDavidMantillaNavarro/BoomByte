using System.Collections;
using UnityEngine;
using TMPro;
using FMODUnity;

public class DialogoSimple : MonoBehaviour
{
    [Header("Canvas del diálogo")]
    public GameObject canvasDialogo;
    public CanvasGroup fadeCanvas;

    [Header("Texto")]
    public TMP_Text textoDialogo;

    [TextArea(3, 6)]
    public string mensaje = "Mensaje del diálogo.";

    [Header("Jugador")]
    public Transform jugador;

    [Header("Distancia de activación")]
    public float distanciaActivacion = 6f;

    [Header("Velocidad escritura")]
    public float velocidadEscritura = 0.02f;

    [Header("Duraciones")]
    public float duracionVisible = 8f;
    public float duracionFade = 0.5f;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference sonidoDialogo;

    [Header("Final del juego")]
    public VideoFinalController videoFinal;

    private bool yaSeActivo = false;

    void Start()
    {
        if (canvasDialogo != null)
            canvasDialogo.SetActive(false);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;
    }

    void Update()
    {
        if (jugador == null || yaSeActivo)
            return;

        float distancia = Vector3.Distance(jugador.position, transform.position);

        if (distancia <= distanciaActivacion)
        {
            StartCoroutine(SecuenciaDialogo());
        }
    }

    IEnumerator SecuenciaDialogo()
    {
        yaSeActivo = true;

        // Audio FMOD
        RuntimeManager.PlayOneShot(sonidoDialogo, transform.position);

        // Mostrar canvas
        canvasDialogo.SetActive(true);

        // Fade In
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);

            yield return null;
        }

        fadeCanvas.alpha = 1f;

        // Texto escribiéndose
        yield return StartCoroutine(EscribirTexto());

        // Esperar visible
        yield return new WaitForSecondsRealtime(duracionVisible);

        // Fade Out
        tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);

            yield return null;
        }

        fadeCanvas.alpha = 0f;

        // Ocultar canvas
        canvasDialogo.SetActive(false);

        if (videoFinal != null)
        {
            videoFinal.IniciarVideo();
        }
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
}