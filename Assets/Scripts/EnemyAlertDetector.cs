using System.Collections;
using UnityEngine;
using FMODUnity;

public class EnemyAlertDetector : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float distanciaDeteccion = 4f;

    [Header("UI")]
    public GameObject panelAlerta;

    [Header("Visual")]
    public float duracionFadeIn = 0.3f;
    public float duracionVisible = 2f;
    public float duracionFadeOut = 0.5f;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference alertaEnemigoSound;

    private bool activado = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (panelAlerta == null)
        {
            Debug.LogError("[ALERTA] Panel no asignado");
            return;
        }

        canvasGroup = panelAlerta.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = panelAlerta.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        panelAlerta.SetActive(false);
    }

    void Update()
    {
        if (activado || player == null)
            return;

        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia <= distanciaDeteccion)
        {
            activado = true;

            RuntimeManager.PlayOneShot(alertaEnemigoSound, transform.position);

            StartCoroutine(MostrarAlerta());
        }
    }

    IEnumerator MostrarAlerta()
    {
        Debug.Log("[ALERTA] Mostrar");

        panelAlerta.SetActive(true);
        canvasGroup.alpha = 0f;

        float t = 0f;

        while (t < duracionFadeIn)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duracionFadeIn);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(duracionVisible);

        t = 0f;

        while (t < duracionFadeOut)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duracionFadeOut);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        Debug.Log("[ALERTA] Desactivando");

        panelAlerta.SetActive(false);
    }

    void OnDisable()
    {
        StopAllCoroutines();

        if (panelAlerta != null)
            panelAlerta.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        activado = false;
    }
}