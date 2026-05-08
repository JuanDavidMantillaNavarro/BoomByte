using UnityEngine;

public class TutorialExitTrigger : MonoBehaviour
{
    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaActivado) return;

        Debug.Log("Algo entr� al trigger: " + other.name);

        // SOLO jugador
        if (!other.CompareTag("Player")) return;

        yaActivado = true;
        GameController.Instance.ManejoTiempo(false);

        if (TutorialHintManager.Instance != null)
        {
            TutorialHintManager.Instance.ActivarTutorialContextual();
            Debug.Log("TUTORIAL ACTIVADO AL ENTRAR AL CUBO");
        }
    }
}