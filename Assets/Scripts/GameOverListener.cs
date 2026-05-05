using UnityEngine;

public class GameOverListener : MonoBehaviour
{
    public DefeatUIController defeatUI;

    private bool yaMostrado = false;

    void Update()
    {
        if (GameController.Instance == null) return;

        // detectar derrota
        if (GameController.Instance.gameEnded && !yaMostrado)
        {
            yaMostrado = true;

            Debug.Log("Game Over detectado en listener");

            if (!GameController.Instance.isPaused)
            {
                defeatUI.MostrarDerrota();
            }
        }

        // resetear bandera cuando el juego reinicia
        if (!GameController.Instance.gameEnded)
        {
            yaMostrado = false;
        }
    }
}