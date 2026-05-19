using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class VRAlwaysVisibleUI : MonoBehaviour
{
    [Header("Opcional")]
    public Camera vrCamera;

    [Header("Configuración")]
    public bool mirarALaCamara = true;

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

    void LateUpdate()
    {
        if (mirarALaCamara && vrCamera != null)
        {
            transform.forward = transform.position - vrCamera.transform.position;
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
            Debug.LogError("NO se encontró el shader Custom/AlwaysOnTopUI");
            return;
        }

        Graphic[] graficos = GetComponentsInChildren<Graphic>(true);

        foreach (Graphic g in graficos)
        {
            if (g == null) continue;

            Material mat = new Material(shader);

            if (g.mainTexture != null)
            {
                mat.SetTexture("_MainTex", g.mainTexture);
            }

            g.material = mat;
        }
    }
}