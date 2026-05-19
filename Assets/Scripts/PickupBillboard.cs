using System.Collections;
using UnityEngine;
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

    [Tooltip("Texto del mensaje Easter Egg")]
    public TMP_Text textoMensaje;

    [Tooltip("Cuánto dura visible el fondo")]
    public float duracionFondo = 8f;

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

        // FORZAR TEXTO ENCIMA DEL FONDO
        if (textoMensaje != null)
        {
            textoMensaje.transform.SetAsLastSibling();
        }
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

        // Easter Egg
        if (GameController.Instance != null)
        {
            GameController.Instance.CollectEasterEgg();

            // +30 segundos
            GameController.Instance.AddExtraTime(tiempoExtra);
        }

        // Mostrar fondo
        if (fondoMensajeUI != null)
        {
            fondoMensajeUI.SetActive(true);
        }

        // Texto siempre encima
        if (textoMensaje != null)
        {
            textoMensaje.transform.SetAsLastSibling();
        }

        // Desactivar visual del pato
        if (sprite != null)
            sprite.enabled = false;

        // Desactivar interacción
        interactionLayers = 0;

        // Esperar antes de destruir
        StartCoroutine(DestruirDespues());
    }

    IEnumerator DestruirDespues()
    {
        yield return new WaitForSeconds(duracionFondo);

        if (fondoMensajeUI != null)
            fondoMensajeUI.SetActive(false);

        Destroy(gameObject);
    }
}