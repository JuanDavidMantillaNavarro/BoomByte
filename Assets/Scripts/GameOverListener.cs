using UnityEngine;

public class GameOverListener : MonoBehaviour
{
    public DefeatUIController defeatUI;

    private bool yaMostrado = false;

    void Update()
    {
        if (GameController.Instance == null) return;

        if (GameController.Instance.gameEnded && !yaMostrado)
        {
            yaMostrado = true;

            // si perdió (no ganó)
            if (!GameController.Instance.isPaused)
            {
                defeatUI.MostrarDerrota();
            }
        }
    }
}