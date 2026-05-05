using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogoEntradaVR : MonoBehaviour
{
    [Header("Referencias")]
    public Transform mainCamera;
    public CanvasGroup canvasGroup;
    public RectTransform logo;

    [Header("Movimiento")]
    public float distanciaInicial = 4f;
    public float velocidad = 2f;
    public float puntoDesaparicion = -0.5f;

    [Header("Fade")]
    public float duracionFade = 0.5f;

    private bool activado = false;

    void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SecuenciaLogo());
        }
    }

    IEnumerator SecuenciaLogo()
    {
        activado = true;

        // Activar canvas
        canvasGroup.gameObject.SetActive(true);

        // Posición inicial del logo (frente a la cámara)
        logo.localPosition = new Vector3(0, 0, distanciaInicial);

        // Fade In
        yield return StartCoroutine(Fade(0f, 1f));

        // Movimiento hacia el jugador
        while (logo.localPosition.z > puntoDesaparicion)
        {
            logo.localPosition += Vector3.back * velocidad * Time.deltaTime;
            yield return null;
        }

        // Fade Out cuando atraviesa
        yield return StartCoroutine(Fade(1f, 0f));

        canvasGroup.gameObject.SetActive(false);
    }

    IEnumerator Fade(float inicio, float fin)
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(inicio, fin, tiempo / duracionFade);
            yield return null;
        }

        canvasGroup.alpha = fin;
    }
}