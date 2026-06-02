using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemporaryInputInvertEffect : MonoBehaviour
{
    [Header("Jugador")]
    public Transform player;

    [Header("Detección")]
    public float distanciaDeteccion = 2f;

    [Header("Input")]
    public InputActionReference moveAction;
    public InputActionReference turnAction;

    [Header("UI")]
    public GameObject panelEfecto;

    [Header("Efecto")]
    public float duracionEfecto = 5f;

    [Header("Mensaje")]
    public float duracionMensaje = 2f;

    private bool activado = false;
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

    void Update()
    {
        if (activado)
            return;

        if (player == null)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distancia <= distanciaDeteccion)
        {
            activado = true;

            Debug.Log(
                "[INVERTIDO] Jugador detectado"
            );

            StartCoroutine(
                InvertirTemporalmente()
            );
        }
    }

    IEnumerator InvertirTemporalmente()
    {
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
            "[INVERTIDO] Controles restaurados"
        );
    }

    IEnumerator MostrarMensaje()
    {
        if (panelEfecto == null)
            yield break;

        panelEfecto.SetActive(true);

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        Debug.Log(
            "[INVERTIDO] Mensaje mostrado"
        );

        yield return new WaitForSeconds(
            duracionMensaje
        );

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        panelEfecto.SetActive(false);

        Debug.Log(
            "[INVERTIDO] Mensaje ocultado"
        );
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