using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAlertDetector : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float distanciaDeteccion = 4f;

    [Header("UI")]
    public GameObject panelAlerta;

    [Header("Fade")]
    public float duracionFadeIn = 0.3f;
    public float duracionVisible = 3f;
    public float duracionFadeOut = 0.8f;

    private bool activado = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (panelAlerta == null)
        {
            Debug.LogError(
                "[ALERTA] Panel no asignado"
            );
            return;
        }

        canvasGroup =
            panelAlerta.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup =
                panelAlerta.AddComponent<CanvasGroup>();

            Debug.Log(
                "[ALERTA] CanvasGroup agregado automáticamente"
            );
        }

        canvasGroup.alpha = 0f;

        panelAlerta.SetActive(false);

        Debug.Log(
            "[ALERTA] Sistema listo"
        );
    }

    void Update()
    {
        if (activado)
            return;

        if (player == null)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distancia <= distanciaDeteccion)
        {
            Debug.Log(
                "[ALERTA] Enemigo detectado a "
                + distancia
            );

            activado = true;

            StartCoroutine(
                MostrarAlerta()
            );
        }
    }

    IEnumerator MostrarAlerta()
    {
        panelAlerta.SetActive(true);

        Debug.Log(
            "[ALERTA] Mostrando panel"
        );

        // Fade In
        float t = 0f;

        while (t < duracionFadeIn)
        {
            t += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    t / duracionFadeIn
                );

            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(
            duracionVisible
        );

        // Fade Out
        t = 0f;

        while (t < duracionFadeOut)
        {
            t += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    t / duracionFadeOut
                );

            yield return null;
        }

        canvasGroup.alpha = 0f;

        panelAlerta.SetActive(false);

        Debug.Log(
            "[ALERTA] Ocultada"
        );
    }
}