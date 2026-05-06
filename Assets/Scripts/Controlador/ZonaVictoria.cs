using UnityEngine;

public class ZonaVictoria : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.Victoria();
            GameController.Instance.ManejoTiempo(true);
        }
    }
}