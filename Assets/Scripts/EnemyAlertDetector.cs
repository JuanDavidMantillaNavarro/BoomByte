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

    [Header("Tiempo")]
    public float duracionVisible = 2f;

    private bool alertaYaMostrada = false;

    void Start()
    {
        Debug.Log("[ALERTA] Start");

        if (panelAlerta != null)
            panelAlerta.SetActive(false);
    }

    void Update()
    {
        if (player == null)
            return;

        if (alertaYaMostrada)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distancia <= distanciaDeteccion)
        {
            alertaYaMostrada = true;

            Debug.Log(
                "[ALERTA] Jugador detectado"
            );

            StartCoroutine(
                MostrarAlerta()
            );
        }
    }

    IEnumerator MostrarAlerta()
    {
        Debug.Log(
            "[ALERTA] Mostrando imagen"
        );

        if (panelAlerta != null)
            panelAlerta.SetActive(true);

        yield return new WaitForSeconds(
            duracionVisible
        );

        if (panelAlerta != null)
            panelAlerta.SetActive(false);

        Debug.Log(
            "[ALERTA] Ocultada"
        );
    }
}