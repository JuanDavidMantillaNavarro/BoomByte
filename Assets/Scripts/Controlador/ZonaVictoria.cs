using UnityEngine;

public class ZonaVictoria : MonoBehaviour
{
    public VictoryUIController victoryUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.Victoria();

            if (victoryUI != null)
                victoryUI.MostrarVictoria();
        }
    }
}