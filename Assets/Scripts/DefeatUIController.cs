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
    public float tiempoEspera = 4f;

    public void MostrarDerrota()
    {
        Debug.Log("MostrarDerrota() llamado");

        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);
            Debug.Log("Panel derrota activado");
        }
        else
        {
            Debug.LogError("panelDerrota NO asignado");
        }

        StartCoroutine(Reiniciar());
    }

    IEnumerator Reiniciar()
    {
        Debug.Log("Esperando reinicio: " + tiempoEspera);

        yield return new WaitForSeconds(tiempoEspera);

        Debug.Log("Tiempo cumplido, reiniciando");

        if (panelDerrota != null)
        {
            panelDerrota.SetActive(false);
            Debug.Log("Panel derrota ocultado");
        }

        if (player != null && puntoInicio != null)
        {
            Debug.Log("Moviendo player al punto inicio");

            Debug.Log("Pos actual: " + player.position);
            Debug.Log("Pos destino: " + puntoInicio.position);

            player.position = puntoInicio.position;
            player.rotation = puntoInicio.rotation;

            Debug.Log("Nueva pos: " + player.position);
        }
        else
        {
            Debug.LogError("player o puntoInicio NO asignado");
        }

        if (GameController.Instance != null)
        {
            Debug.Log("Reiniciando estado juego");
            GameController.Instance.ReiniciarEstado();
        }
        else
        {
            Debug.LogError("GameController.Instance es NULL");
        }
    }
}