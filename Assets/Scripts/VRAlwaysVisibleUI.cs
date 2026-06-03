using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class VRAlwaysVisibleUI : MonoBehaviour
{
    [Header("Opcional")]
    public Camera vrCamera;

    public int sortingOrder = 1000;

    [Header("Always On Top")]
    public bool ignorarParedes = true;

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (vrCamera == null)
        {
            vrCamera = Camera.main;
        }

        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;

        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            canvas.worldCamera = vrCamera;
        }

        if (ignorarParedes)
        {
            AplicarMaterialAlwaysOnTop();
        }
    }

    void OnEnable()
    {
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
        }

        if (ignorarParedes)
        {
            AplicarMaterialAlwaysOnTop();
        }
    }
    void AplicarMaterialAlwaysOnTop()
    {
        Shader shader = Shader.Find("Custom/AlwaysOnTopUI");

        if (shader == null)
        {
            Debug.LogError("NO se encontró el shader");
            return;
        }

        Image[] imagenes = GetComponentsInChildren<Image>(true);

        foreach (Image img in imagenes)
        {
            if (img == null)
                continue;

            Material mat = new Material(shader);

            if (img.mainTexture != null)
                mat.SetTexture("_MainTex", img.mainTexture);

            img.material = mat;
        }
    }
}