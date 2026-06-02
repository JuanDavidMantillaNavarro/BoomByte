using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemporaryInputInvertEffect : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference moveAction;
    public InputActionReference turnAction;

    [Header("UI")]
    public GameObject panelEfecto;

    [Header("Efecto")]
    public float duracionEfecto = 5f;

    [Header("Mensaje")]
    public float duracionMensaje = 2f;
    public float duracionFadeIn = 0.3f;
    public float duracionFadeOut = 0.5f;

    private bool efectoActivo = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (panelEfecto != null)
        {
            canvasGroup =
                panelEfecto.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup =
                    panelEfecto.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            panelEfecto.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (efectoActivo)
            return;

        if (!other.CompareTag("Player"))
            return;

        StartCoroutine(
            InvertirTemporalmente()
        );
    }

    IEnumerator InvertirTemporalmente()
    {
        efectoActivo = true;

        AplicarInversion();

        StartCoroutine(
            MostrarMensaje()
        );

        Debug.Log(
            "[INVERTIDO] Controles invertidos"
        );

        yield return new WaitForSeconds(
            duracionEfecto
        );

        RestaurarInputs();

        Debug.Log(
            "[INVERTIDO] Restaurado"
        );

        efectoActivo = false;
    }

    IEnumerator MostrarMensaje()
    {
        if (panelEfecto == null)
            yield break;

        panelEfecto.SetActive(true);

        float t = 0f;

        while (t < duracionFadeIn)
        {
            t += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    t / duracionFadeIn
                );

            yield return null;
        }

        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(
            duracionMensaje
        );

        t = 0f;

        while (t < duracionFadeOut)
        {
            t += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    t / duracionFadeOut
                );

            yield return null;
        }

        canvasGroup.alpha = 0f;

        panelEfecto.SetActive(false);
    }

    void AplicarInversion()
    {
        if (moveAction != null)
        {
            moveAction.action.ApplyBindingOverride(
                0,
                new InputBinding
                {
                    overrideProcessors =
                        "scaleVector2(x=-1,y=-1)"
                }
            );
        }

        if (turnAction != null)
        {
            turnAction.action.ApplyBindingOverride(
                0,
                new InputBinding
                {
                    overrideProcessors =
                        "scaleVector2(x=-1,y=-1)"
                }
            );
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