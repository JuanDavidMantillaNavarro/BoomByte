using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class ProfesorPowerUp : MonoBehaviour
{

    [Header("UI")]
    public GameObject panel;
    public CanvasGroup fadeCanvas;
    public float retrasoMensaje = 0.5f;
    public float duracionMensaje = 2f;
    public float duracionFade = 0.5f;
    public bool activo = false;

    public void ActivarBeneficio()
    {
        if(!activo)
        StartCoroutine(BoostProfe());
    }

    IEnumerator BoostProfe()
    {
        activo = true ;
        GameController.Instance.OnNpcCollide(gameObject.name);
        Debug.Log("BOOST ACTIVADO");

        yield return new WaitForSeconds(retrasoMensaje);

        yield return StartCoroutine(MostrarMensaje());

        activo = false;
    }

    IEnumerator MostrarMensaje()
    {
        if (panel == null) yield break;

        panel.SetActive(true);
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

        panel.SetActive(false);
    }
}