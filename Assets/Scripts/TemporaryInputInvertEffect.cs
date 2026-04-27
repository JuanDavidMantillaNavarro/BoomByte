using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemporaryInputInvertEffect : MonoBehaviour
{
    [Header("Input Actions (nuevo sistema)")]
    public InputActionReference moveAction;
    public InputActionReference turnAction;

    [Header("UI")]
    public GameObject panelEfecto;
    public CanvasGroup fadeCanvas;

    [Header("Tiempo")]
    public float duracionEfecto = 5f;
    public float duracionFade = 1f;

    private bool efectoActivo = false;

    private void Start()
    {
        if (panelEfecto != null)
            panelEfecto.SetActive(false);

        Debug.Log("Sistema de inversión listo");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (efectoActivo) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("JUGADOR TOCÓ ENEMIGO");

            StartCoroutine(InvertirTemporalmente());
        }
    }

    IEnumerator InvertirTemporalmente()
    {
        efectoActivo = true;

        if (panelEfecto != null)
            panelEfecto.SetActive(true);

        if (fadeCanvas != null)
            fadeCanvas.alpha = 1f;

        AplicarInversion();

        Debug.Log("CONTROLES INVERTIDOS");

        // esperar TODO el efecto primero
        yield return new WaitForSeconds(duracionEfecto);

        // luego hacer fade
        yield return StartCoroutine(FadeOutUI());

        RestaurarInputs();

        if (panelEfecto != null)
            panelEfecto.SetActive(false);

        Debug.Log("CONTROLES RESTAURADOS");

        efectoActivo = false;
    }

    IEnumerator FadeOutUI()
    {
        if (fadeCanvas == null)
            yield break;

        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(1f, 0f, tiempo / duracionFade);

            yield return null;
        }

        fadeCanvas.alpha = 0f;
    }

    void AplicarInversion()
    {
        if (moveAction != null)
        {
            moveAction.action.ApplyBindingOverride(
                0,
                new InputBinding
                {
                    overrideProcessors = "scaleVector2(x=-1,y=-1)"
                }
            );

            Debug.Log("Movimiento invertido");
        }

        if (turnAction != null)
        {
            turnAction.action.ApplyBindingOverride(
                0,
                new InputBinding
                {
                    overrideProcessors = "scaleVector2(x=-1,y=-1)"
                }
            );

            Debug.Log("Rotación invertida");
        }
    }

    void RestaurarInputs()
    {
        if (moveAction != null)
            moveAction.action.RemoveAllBindingOverrides();

        if (turnAction != null)
            turnAction.action.RemoveAllBindingOverrides();
    }
}