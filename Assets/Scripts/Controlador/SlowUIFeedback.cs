using System.Collections;
using UnityEngine;

public class SlowUIFeedback : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelSlow;

    [Header("VR")]
    public Transform playerCamera;
    public float distancia = 1.5f;
    public float altura = -0.2f;

    [Header("Tiempo visible")]
    public float duracion = 2.5f;

    public void MostrarSlow()
    {
        if (panelSlow == null || playerCamera == null) return;

        StopAllCoroutines();
        StartCoroutine(MostrarRutina());
    }

    IEnumerator MostrarRutina()
    {
        // posicionar frente al jugador
        Vector3 pos =
            playerCamera.position +
            playerCamera.forward * distancia;

        pos.y += altura;

        panelSlow.transform.position = pos;

        panelSlow.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);

        panelSlow.SetActive(true);

        yield return new WaitForSeconds(duracion);

        panelSlow.SetActive(false);
    }
}