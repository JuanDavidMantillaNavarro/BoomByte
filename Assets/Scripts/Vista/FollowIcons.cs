using UnityEngine;

public class FollowIcons : MonoBehaviour
{
    [Header("Objeto a seguir")]
    public Transform target;

    [Header("Separación respecto al objetivo")]
    public Vector3 offset = Vector3.zero;

    [Header("Mantener altura fija")]
    public bool mantenerYFija = true;

    private float yInicial;

    void Start()
    {
        yInicial = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 nuevaPosicion = target.position + offset;

        if (mantenerYFija)
            nuevaPosicion.y = yInicial;

        transform.position = nuevaPosicion;
    }
}