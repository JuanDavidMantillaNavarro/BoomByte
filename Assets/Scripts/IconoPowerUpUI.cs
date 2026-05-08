using System.Collections;
using UnityEngine;

public class IconoPowerUpUI : MonoBehaviour
{
    [Header("Referencia")]
    public GameObject canvasPowerUp;

    [Header("UI Icono")]
    public GameObject iconoUI;
    public CanvasGroup canvasGroup;

    [Header("Duración del efecto")]
    public float duracionPowerUp = 8f;

    [Header("Fade")]
    public float duracionFade = 0.5f;

    private bool activado = false;

    void Start()
    {
        if (iconoUI != null)
            iconoUI.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (canvasPowerUp == null) return;

        // Detecta cuando aparece el mensaje del power up
        if (canvasPowerUp.activeSelf && !activado)
        {
            StartCoroutine(MostrarIcono());
        }
    }

    IEnumerator MostrarIcono()
    {
        activado = true;

        iconoUI.SetActive(true);

        float tiempo = 0f;

        // FADE IN
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(0f, 1f, tiempo / duracionFade);

            yield return null;
        }

        canvasGroup.alpha = 1f;

        // ESPERA DURACIÓN DEL POWER UP
        yield return new WaitForSeconds(duracionPowerUp);

        tiempo = 0f;

        // FADE OUT
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(1f, 0f, tiempo / duracionFade);

            yield return null;
        }

        canvasGroup.alpha = 0f;

        iconoUI.SetActive(false);

        // Espera a que el canvas del mensaje desaparezca
        yield return new WaitUntil(() => !canvasPowerUp.activeSelf);

        activado = false;
    }

}