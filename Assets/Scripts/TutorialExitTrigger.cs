using UnityEngine;

public class TutorialExitTrigger : MonoBehaviour
{
    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaActivado) return;

        Debug.Log("Algo entró al trigger: " + other.name);

        // detectar cámara, player o XR origin
        if (
            other.CompareTag("Player") ||
            other.name.Contains("Camera") ||
            other.name.Contains("XR")
        )
        {
            yaActivado = true;

            TutorialHintManager.Instance.ActivarTutorialContextual();

            Debug.Log("TUTORIAL ACTIVADO AL SALIR DEL CUBO");
        }
    }
}