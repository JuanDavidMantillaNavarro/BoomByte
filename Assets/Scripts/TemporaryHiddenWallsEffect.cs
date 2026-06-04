using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHiddenWallsEffect : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float distanciaDeteccion = 2f;

    [Header("UI")]
    public GameObject panelEfecto;

    [Header("Efecto")]
    public float duracionEfecto = 7f;

    [Header("Mensaje")]
    public float duracionMensaje = 2f;

    [Header("Tags")]
    public string destructibleTag = "Destructible";
    public string destructibleTag2 = "Destructible2";
    public string destructibleTag3 = "Destructible3";

    private bool activado = false;
    private CanvasGroup canvasGroup;

    private List<Renderer> paredesOcultadas =
        new List<Renderer>();

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

            StartCoroutine(
                AplicarOcultamiento()
            );
        }
    }

    IEnumerator AplicarOcultamiento()
    {
        OcultarParedes();

        StartCoroutine(
            MostrarMensaje()
        );

        Debug.Log(
            "[OCULTAMIENTO] Paredes ocultadas"
        );

        yield return new WaitForSeconds(
            duracionEfecto
        );

        MostrarParedes();

        Debug.Log(
            "[OCULTAMIENTO] Paredes restauradas"
        );
    }

    void OcultarParedes()
    {
        paredesOcultadas.Clear();

        GameObject[] objetos =
            FindObjectsByType<GameObject>(
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objetos)
        {
            if (
                obj.CompareTag(destructibleTag) ||
                obj.CompareTag(destructibleTag2) ||
                obj.CompareTag(destructibleTag3)
            )
            {
                Renderer[] renderers =
                    obj.GetComponentsInChildren<Renderer>();

                foreach (Renderer rend in renderers)
                {
                    rend.enabled = false;
                    paredesOcultadas.Add(rend);
                }
            }
        }
    }

    void MostrarParedes()
    {
        foreach (Renderer rend in paredesOcultadas)
        {
            if (rend != null)
                rend.enabled = true;
        }

        paredesOcultadas.Clear();
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
        MostrarParedes();

        if (panelEfecto != null)
            panelEfecto.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}