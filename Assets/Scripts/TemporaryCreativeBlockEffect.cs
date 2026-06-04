using System.Collections;
using UnityEngine;

public class TemporaryCreativeBlockEffect : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float distanciaDeteccion = 2f;

    [Header("UI")]
    public GameObject panelEfecto;

    [Header("Efecto")]
    public float duracionEfecto = 4f;

    [Header("Mensaje")]
    public float duracionMensaje = 2f;

    [Header("Reducción de alcance")]
    public int reduccionRadio = 1;

    private bool activado = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (panelEfecto != null)
        {
            canvasGroup = panelEfecto.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = panelEfecto.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            panelEfecto.SetActive(false);
        }
    }

    void Update()
    {
        if (activado || player == null)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distancia <= distanciaDeteccion)
        {
            activado = true;

            Debug.Log(
                "[BLOQUEO CREATIVO] Jugador detectado"
            );

            StartCoroutine(
                AplicarBloqueoCreativo()
            );
        }
    }

    IEnumerator AplicarBloqueoCreativo()
    {
        AplicarEfecto();

        StartCoroutine(
            MostrarMensaje()
        );

        Debug.Log(
            "[BLOQUEO CREATIVO] Alcance reducido"
        );

        yield return new WaitForSeconds(
            duracionEfecto
        );

        RestaurarEfecto();

        Debug.Log(
            "[BLOQUEO CREATIVO] Alcance restaurado"
        );
    }

    IEnumerator MostrarMensaje()
    {
        if (panelEfecto == null)
            yield break;

        panelEfecto.SetActive(true);

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(
            duracionMensaje
        );

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        panelEfecto.SetActive(false);
    }

    void AplicarEfecto()
    {
        GameController.Instance.explosionRadiusModifier -= reduccionRadio;
    }

    void RestaurarEfecto()
    {
        GameController.Instance.explosionRadiusModifier += reduccionRadio;
    }

    void OnDisable()
    {
        StopAllCoroutines();

        if (panelEfecto != null)
            panelEfecto.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}