using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EfectoZoomLogo : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform logoRect;
    public Image logoImage;

    [Header("Escala")]
    public Vector3 escalaInicial = new Vector3(0.2f, 0.2f, 0.2f);
    public Vector3 escalaFinal = new Vector3(4f, 4f, 4f);

    [Header("Fase 1: avance inicial")]
    public float duracionAcercamiento = 5f;
    public float avanceZInicial = 2f;

    [Header("Pausa")]
    public float tiempoEsperaAntesSalida = 5f;

    [Header("Fase 2: salida hacia adelante + fade")]
    public float duracionSalida = 2f;
    public float avanceZSalida = 2f;

    private bool enEjecucion = false;
    private bool yaSeEjecuto = false;

    private Vector3 posicionInicial;
    private Vector3 direccionZ;

    void Awake()
    {
        transform.SetParent(null, true);
    }

    void Start()
    {
        posicionInicial = transform.position;

        // Dirección Z real del objeto
        direccionZ = transform.TransformDirection(Vector3.forward);

        if (logoImage != null)
        {
            Color color = logoImage.color;
            color.a = 0f;
            logoImage.color = color;
        }

        if (logoRect != null)
        {
            logoRect.localScale = escalaInicial;
        }
    }

    public void ActivarEfectoAhora()
    {
        if (!enEjecucion && !yaSeEjecuto)
        {
            StartCoroutine(ReproducirEfecto());
        }
    }

    IEnumerator ReproducirEfecto()
    {
        if (logoRect == null || logoImage == null)
        {
            Debug.LogWarning("Faltan referencias en EfectoZoomLogo.");
            yield break;
        }

        enEjecucion = true;
        yaSeEjecuto = true;

        transform.SetParent(null, true);

        // Reset
        transform.position = posicionInicial;
        logoRect.localPosition = Vector3.zero;
        logoRect.localRotation = Quaternion.identity;
        logoRect.localScale = escalaInicial;

        Color color = logoImage.color;
        color.a = 1f;
        logoImage.color = color;

        // =========================
        // FASE 1: avanza en Z y hace zoom
        // =========================
        Vector3 posicionPausa = posicionInicial + direccionZ.normalized * avanceZInicial;

        float tiempo = 0f;
        while (tiempo < duracionAcercamiento)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracionAcercamiento);

            transform.position = Vector3.Lerp(posicionInicial, posicionPausa, t);
            logoRect.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);

            yield return null;
        }

        transform.position = posicionPausa;
        logoRect.localScale = escalaFinal;

        // =========================
        // PAUSA
        // =========================
        yield return new WaitForSeconds(tiempoEsperaAntesSalida);

        // =========================
        // FASE 2: vuelve a avanzar en Z y se desvanece
        // =========================
        Vector3 posicionFinal = posicionPausa + direccionZ.normalized * avanceZSalida;

        tiempo = 0f;
        while (tiempo < duracionSalida)
        {
            tiempo += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, tiempo / duracionSalida);

            transform.position = Vector3.Lerp(posicionPausa, posicionFinal, t);

            color.a = Mathf.Lerp(1f, 0f, t);
            logoImage.color = color;

            yield return null;
        }

        transform.position = posicionFinal;
        color.a = 0f;
        logoImage.color = color;

        enEjecucion = false;
    }
}