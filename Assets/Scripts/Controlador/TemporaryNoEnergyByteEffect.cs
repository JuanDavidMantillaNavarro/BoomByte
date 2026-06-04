using System.Collections;
using UnityEngine;

public class TemporaryDisableEnergyBytes : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float distanciaDeteccion = 2f;

    [Header("UI")]
    public GameObject panelEfecto;

    [Header("Efecto")]
    public float duracionEfecto = 3f;

    [Header("Mensaje")]
    public float duracionMensaje = 2f;

    private bool activado = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (panelEfecto != null)
        {
            canvasGroup =
                panelEfecto.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup =
                    panelEfecto.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            panelEfecto.SetActive(false);
        }
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
            activado = true;

            StartCoroutine(
                BloquearEnergyBytes()
            );
        }
    }

    IEnumerator BloquearEnergyBytes()
    {
        GameController.Instance.energyBytesBloqueadas = true;

        StartCoroutine(
            MostrarMensaje()
        );

        Debug.Log(
            "[ERROR] Energy Bytes deshabilitadas"
        );

        yield return new WaitForSeconds(
            duracionEfecto
        );

        GameController.Instance.energyBytesBloqueadas = false;

        Debug.Log(
            "[ERROR] Energy Bytes habilitadas"
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

    void OnDisable()
    {
        StopAllCoroutines();

        GameController.Instance.energyBytesBloqueadas = false;

        if (panelEfecto != null)
            panelEfecto.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}