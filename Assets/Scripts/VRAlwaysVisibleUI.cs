using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class VRAlwaysVisibleUI : MonoBehaviour
{
    [Header("Opcional")]
    public Camera vrCamera;

    [Header("Configuración")]
    public bool mirarALaCamara = true;

    [Tooltip("Mientras más alto, más prioridad visual")]
    public int sortingOrder = 500;

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();

        // Busca automáticamente la Main Camera
        if (vrCamera == null)
        {
            vrCamera = Camera.main;
        }

        // Fuerza el canvas a renderizar encima
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;

        // IMPORTANTE:
        // Hace que el canvas use la cámara VR correcta
        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            canvas.worldCamera = vrCamera;
        }
    }

    void LateUpdate()
    {
        if (mirarALaCamara && vrCamera != null)
        {
            // Hace que siempre mire a la cámara
            transform.forward = transform.position - vrCamera.transform.position;
        }
    }

    void OnEnable()
    {
        // Reaplica por si Unity lo pierde al activarse
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
        }
    }
}