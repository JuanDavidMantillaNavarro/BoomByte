using UnityEngine;

public class ExpandImage : MonoBehaviour
{
    public float expandedScaleMultiplier = 3f;

    private Vector3 originalScale;
    private bool expanded = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void Toggle()
    {
        if (!expanded)
            Expand();
        else
            Restore();
    }

    void Expand()
    {
        expanded = true;
        transform.localScale = originalScale * expandedScaleMultiplier;
        Time.timeScale = 0f;
    }

    void Restore()
    {
        expanded = false;
        transform.localScale = originalScale;
        Time.timeScale = 1f;
    }
}