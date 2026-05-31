using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIIconoPowerUp : MonoBehaviour
{
    public Image imagenPowerUp;

    public Sprite iconoPowerUp;

    public float duracionVisible = 5f;

    Coroutine rutina;

    void Awake()
    {
        if (imagenPowerUp == null)
        {
            Debug.LogError("imagenPowerUp NO ASIGNADA");
            return;
        }

        imagenPowerUp.enabled = false;
    }

    public void MostrarPowerUp()
    {
        if (rutina != null)
            StopCoroutine(rutina);

        rutina = StartCoroutine(RutinaPowerUp());
    }

    IEnumerator RutinaPowerUp()
    {
        if (imagenPowerUp == null)
            yield break;

        if (iconoPowerUp == null)
            yield break;

        imagenPowerUp.sprite = iconoPowerUp;

        imagenPowerUp.gameObject.SetActive(true);
        imagenPowerUp.enabled = true;

        yield return new WaitForSecondsRealtime(
            duracionVisible
        );

        imagenPowerUp.enabled = false;

        imagenPowerUp.gameObject.SetActive(false);
    }
}