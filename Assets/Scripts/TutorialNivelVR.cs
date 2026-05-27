using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class TutorialNivelVR : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasNivel;

    [Header("Movimiento XR")]
    public ContinuousMoveProvider moveProvider;
    public ContinuousTurnProvider turnProvider;

    [Header("Duración")]
    public float duracion = 6f;

    [Header("Fade")]
    public float velocidadFade = 2f;

    private bool activado = false;

    private CanvasGroup canvasGroup;

    private float velocidadOriginal;

    void Start()
    {
        if (canvasNivel != null)
        {
            canvasGroup =
                canvasNivel.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup =
                    canvasNivel.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;

            canvasNivel.SetActive(false);
        }

        // Guardar velocidad original
        if (moveProvider != null)
            velocidadOriginal = moveProvider.moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activado)
            return;

        activado = true;

        StartCoroutine(
            MostrarTutorial()
        );
    }

    IEnumerator MostrarTutorial()
    {
        // Mostrar canvas
        canvasNivel.SetActive(true);

        // BLOQUEAR MOVIMIENTO VR
        if (moveProvider != null)
            moveProvider.moveSpeed = 0f;

        if (turnProvider != null)
            turnProvider.enabled = false;

        // Fade In PRIMERO
        yield return StartCoroutine(
            FadeCanvas(0f, 1f)
        );

        // AHORA sí pausar tiempo
        Time.timeScale = 0f;

        // Espera REAL aunque el tiempo esté pausado
        yield return new WaitForSecondsRealtime(
            duracion
        );

        // Fade Out
        yield return StartCoroutine(
            FadeCanvas(1f, 0f)
        );

        // Ocultar canvas
        canvasNivel.SetActive(false);

        // REANUDAR TIEMPO
        Time.timeScale = 1f;

        // RESTAURAR MOVIMIENTO
        if (moveProvider != null)
            moveProvider.moveSpeed = velocidadOriginal;

        if (turnProvider != null)
            turnProvider.enabled = true;
    }

    IEnumerator FadeCanvas(
        float inicio,
        float final
    )
    {
        float t = 0f;

        while (t < 1f)
        {
            // usar unscaled porque el tiempo está pausado
            t += Time.unscaledDeltaTime * velocidadFade;

            canvasGroup.alpha =
                Mathf.Lerp(inicio, final, t);

            yield return null;
        }

        canvasGroup.alpha = final;
    }
}