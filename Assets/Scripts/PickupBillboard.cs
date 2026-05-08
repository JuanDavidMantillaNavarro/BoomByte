using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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

    protected override void Awake()
    {
        base.Awake();

        cam = Camera.main.transform;

        sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
        
            materialInstance = sprite.material;

            // Guardamos valores originales
            if (materialInstance.HasProperty("_EmissionColor"))
                originalEmission = materialInstance.GetColor("_EmissionColor");

            originalScale = transform.localScale;
        }
    }

    void LateUpdate()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.forward);
    }

    //  HOVER ENTER
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (materialInstance != null)
        {
            materialInstance.EnableKeyword("_EMISSION");

            Color finalGlow = glowColor * glowIntensity;
            materialInstance.SetColor("_EmissionColor", finalGlow);
        }

        transform.localScale = originalScale * hoverScale;
    }

    //  HOVER EXIT
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if (materialInstance != null)
        {
            materialInstance.SetColor("_EmissionColor", originalEmission);
        }

        transform.localScale = originalScale;
    }

    //  SELECT
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        GameController.Instance.CollectEasterEgg();

        Destroy(gameObject);
    }
}