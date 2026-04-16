using System.Collections;
using UnityEngine;

public class DefeatUIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelDerrota;

    [Header("Reinicio")]
    public Transform puntoInicio;
    public Transform player;

    [Header("Tiempo reinicio")]
    public float tiempoEspera = 10f;

    public void MostrarDerrota()
    {
        if (panelDerrota != null)
            panelDerrota.SetActive(true);

        StartCoroutine(Reiniciar());
    }

    IEnumerator Reiniciar()
    {
        yield return new WaitForSeconds(tiempoEspera);

        if (panelDerrota != null)
            panelDerrota.SetActive(false);

        if (player != null && puntoInicio != null)
        {
            player.position = puntoInicio.position;
            player.rotation = puntoInicio.rotation;
        }

        Time.timeScale = 1f;
    }
}