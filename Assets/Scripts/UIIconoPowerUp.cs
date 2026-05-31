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
        Debug.Log("MOSTRAR POWERUP LLAMADO");

        if (rutina != null)
            StopCoroutine(rutina);

        rutina = StartCoroutine(RutinaPowerUp());
    }

    IEnumerator RutinaPowerUp()
    {
        if (imagenPowerUp == null)
        {
            Debug.LogError("imagenPowerUp ES NULL");
            yield break;
        }

        if (iconoPowerUp == null)
        {
            Debug.LogError("iconoPowerUp ES NULL");
            yield break;
        }

        imagenPowerUp.sprite = iconoPowerUp;

        imagenPowerUp.enabled = true;

        Debug.Log("ICONO ACTIVADO");

        yield return new WaitForSecondsRealtime(duracionVisible);

        imagenPowerUp.enabled = false;

        Debug.Log("ICONO DESACTIVADO");
    }
}