using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using FMODUnity;
using TMPro;

public class PickupBillboard : XRGrabInteractable
{
    private Transform cam;

    private SpriteRenderer sprite;
    private Material materialInstance;

    private Color originalEmission;
    private Vector3 originalScale;

    [Header("Hover Settings")]
    public float hoverScale = 1.2f;
    public Color glowColor = Color.yellow;
    public float glowIntensity = 2f;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference duckGrabSound;

    [Header("UI Extra")]
    public GameObject fondoMensajeUI;

    [Tooltip("Imagen del fondo")]
    public Image imagenFondo;

    [Tooltip("Texto del mensaje Easter Egg")]
    public TMP_Text textoMensaje;

    [Header("Duraciones")]
    public float duracionFondo = 10f;
    public float duracionFade = 2f;

    [Header("Timer")]
    public float tiempoExtra = 30f;

    private bool recogido = false;

    protected override void Awake()
    {
        base.Awake();

        if (Camera.main != null)
            cam = Camera.main.transform;

        sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
            materialInstance = sprite.material;

            if (materialInstance.HasProperty("_EmissionColor"))
                originalEmission =
                    materialInstance.GetColor("_EmissionColor");

            originalScale = transform.localScale;
        }

        // Ocultar fondo inicialmente
        if (fondoMensajeUI != null)
            fondoMensajeUI.SetActive(false);

        // Texto encima del fondo
        if (textoMensaje != null)
            textoMensaje.transform.SetAsLastSibling();
    }

    void LateUpdate()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.forward);
    }

    // ================= HOVER =================

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (materialInstance != null)
        {
            materialInstance.EnableKeyword("_EMISSION");

            Color finalGlow = glowColor * glowIntensity;

            materialInstance.SetColor(
                "_EmissionColor",
                finalGlow
            );
        }

        transform.localScale =
            originalScale * hoverScale;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if (materialInstance != null)
        {
            materialInstance.SetColor(
                "_EmissionColor",
                originalEmission
            );
        }

        transform.localScale = originalScale;
    }

    // ================= SELECT =================

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (recogido) return;

        recogido = true;

        // Sonido
        RuntimeManager.PlayOneShot(
            duckGrabSound,
            transform.position
        );

        // Easter Egg + tiempo
        if (GameController.Instance != null)
        {
            GameController.Instance.CollectEasterEgg();
            GameController.Instance.AddExtraTime(tiempoExtra);
        }

        // Mostrar fondo
        if (fondoMensajeUI != null)
            fondoMensajeUI.SetActive(true);

        // Texto encima
        if (textoMensaje != null)
            textoMensaje.transform.SetAsLastSibling();

        // Reset alpha
        if (textoMensaje != null)
        {
            Color c = textoMensaje.color;
            c.a = 1f;
            textoMensaje.color = c;
        }

        if (imagenFondo != null)
        {
            Color c = imagenFondo.color;
            c.a = 1f;
            imagenFondo.color = c;
        }

        // Ocultar pato
        if (sprite != null)
            sprite.enabled = false;

        // Desactivar interacción
        interactionLayers = 0;

        // Fade coroutine
        StartCoroutine(FadeOutUI());
    }

    IEnumerator FadeOutUI()
    {
        // Espera visible normal
        yield return new WaitForSeconds(
            duracionFondo
        );

        float tiempo = 0f;

        Color textoColor =
            textoMensaje != null
            ? textoMensaje.color
            : Color.white;

        Color fondoColor =
            imagenFondo != null
            ? imagenFondo.color
            : Color.white;

        // Fade
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;

            float alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    tiempo / duracionFade
                );

            // Texto
            if (textoMensaje != null)
            {
                textoColor.a = alpha;
                textoMensaje.color = textoColor;
            }

            // Fondo
            if (imagenFondo != null)
            {
                fondoColor.a = alpha;
                imagenFondo.color = fondoColor;
            }

            yield return null;
        }

        // Asegurar invisibilidad
        if (textoMensaje != null)
        {
            textoColor.a = 0f;
            textoMensaje.color = textoColor;
        }

        if (imagenFondo != null)
        {
            fondoColor.a = 0f;
            imagenFondo.color = fondoColor;
        }

        // Apagar fondo
        if (fondoMensajeUI != null)
            fondoMensajeUI.SetActive(false);

        Destroy(gameObject);
    }
}