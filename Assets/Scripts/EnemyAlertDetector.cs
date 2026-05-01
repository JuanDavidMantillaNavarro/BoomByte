using System.Collections;
using UnityEngine;

public class EnemyAlertDetector : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Distancia")]
    public float distanciaDeteccion = 4f;

    [Header("UI")]
    public GameObject panelAlerta;
    public CanvasGroup fadeCanvas;

    [Header("Tiempo")]
    public float duracionVisible = 2f;
    public float duracionFade = 1f;

    private bool alertaYaMostrada = false;
    private bool mostrandoAlerta = false;

    void Start()
    {
        if (panelAlerta != null)
            panelAlerta.SetActive(false);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;

        Debug.Log("Detector de alerta iniciado");
    }

    void Update()
    {
        if (player == null) return;

        if (alertaYaMostrada) return;

        float distancia = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distancia <= distanciaDeteccion && !mostrandoAlerta)
        {
            StartCoroutine(MostrarAlertaTemporal());
        }
    }

    IEnumerator MostrarAlertaTemporal()
    {
        mostrandoAlerta = true;
        alertaYaMostrada = true;

        Debug.Log("ALERTA: enemigo detectado");

        if (panelAlerta != null)
            panelAlerta.SetActive(true);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 1f;

        yield return new WaitForSeconds(duracionVisible);

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            if (fadeCanvas != null)
            {
                fadeCanvas.alpha =
                    Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            }

            yield return null;
        }

        if (panelAlerta != null)
            panelAlerta.SetActive(false);

        mostrandoAlerta = false;

        Debug.Log("ALERTA ocultada definitivamente");
    }
}