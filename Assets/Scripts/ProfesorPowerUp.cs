using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class ProfesorPowerUp : MonoBehaviour
{
    [Header("Movimiento XR")]
    public ContinuousMoveProvider moveProvider;

    [Header("Boost")]
    public float velocidadExtra = 3f;
    public float duracionBoost = 8f;

    [Header("UI")]
    public GameObject panelVelocidad;
    public CanvasGroup fadeCanvas;
    public float retrasoMensaje = 0.5f;
    public float duracionMensaje = 2f;
    public float duracionFade = 0.5f;

    private bool activo = false;

    public void ActivarBeneficio()
    {
        if (!activo)
            StartCoroutine(BoostVelocidad());
    }

    IEnumerator BoostVelocidad()
    {
        activo = true;

        float velocidadOriginal = moveProvider.moveSpeed;

        moveProvider.moveSpeed += velocidadExtra;

        Debug.Log("BOOST ACTIVADO");

        yield return new WaitForSeconds(retrasoMensaje);

        yield return StartCoroutine(MostrarMensaje());

        yield return new WaitForSeconds(duracionBoost);

        moveProvider.moveSpeed = velocidadOriginal;

        Debug.Log("BOOST FINALIZADO");

        activo = false;
    }

    IEnumerator MostrarMensaje()
    {
        if (panelVelocidad == null) yield break;

        panelVelocidad.SetActive(true);
        fadeCanvas.alpha = 0f;

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(0f, 1f, tiempo / duracionFade);

            yield return null;
        }

        yield return new WaitForSeconds(duracionMensaje);

        tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(1f, 0f, tiempo / duracionFade);

            yield return null;
        }

        panelVelocidad.SetActive(false);
    }
}