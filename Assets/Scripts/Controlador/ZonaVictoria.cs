using UnityEngine;

public class ZonaVictoria : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameController.Instance.ManejoTiempo(true);

        GameController.Instance.Victoria();

        GameController.Instance.uiManager.OcultarTimer();
    }
}