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

    [Header("Tiempo")]
    public float duracionEfecto = 5f;

    private bool efectoActivo = false;

    void Start()
    {
        Debug.Log("[INVERTIDO] Start");

        if (panelEfecto != null)
            panelEfecto.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (efectoActivo)
            return;

        if (!other.CompareTag("Player"))
            return;

        Debug.Log(
            "[INVERTIDO] Player detectado"
        );

        StartCoroutine(
            InvertirTemporalmente()
        );
    }

    IEnumerator InvertirTemporalmente()
    {
        efectoActivo = true;

        if (panelEfecto != null)
            panelEfecto.SetActive(true);
        Debug.Log(
    "Activo = " +
    panelEfecto.activeInHierarchy
);

        Debug.Log(
            "Posicion = " +
            panelEfecto.transform.position
        );

        AplicarInversion();

        Debug.Log(
            "[INVERTIDO] Controles invertidos"
        );

        yield return new WaitForSeconds(
            duracionEfecto
        );

        RestaurarInputs();

        if (panelEfecto != null)
            panelEfecto.SetActive(false);

        Debug.Log(
            "[INVERTIDO] Restaurado"
        );

        efectoActivo = false;
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