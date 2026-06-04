using UnityEngine;

public class FlechaGuia : MonoBehaviour
{
    private Transform paredObjetivo;
    private float velocidadOscilacion = 8f;
    private float timerBusqueda = 0f;
    private float intervaloBusqueda = 0.5f;
    private Vector3 posicionBase;

    [SerializeField] private float offsetZ = 85f;       // Z cuando apunta al frente
    [SerializeField] private float distanciaOscilacion = 20f; // ajusta en Inspector

    void Start()
    {
        posicionBase = transform.localPosition;
        BuscarParedMasCercana();
    }

    void Update()
    {
        timerBusqueda += Time.deltaTime;
        if (timerBusqueda >= intervaloBusqueda)
        {
            BuscarParedMasCercana();
            timerBusqueda = 0f;
        }

        RotarHaciaPared();
        OscilarHaciaFrente();
    }

 private void BuscarParedMasCercana()
{
    Vector3 origenBusqueda = Camera.main.transform.position;

    GameObject[] paredes1 = GameObject.FindGameObjectsWithTag("Destructible");
    GameObject[] paredes2 = GameObject.FindGameObjectsWithTag("Destructible2");
    GameObject[] paredes3 = GameObject.FindGameObjectsWithTag("Destructible3");

    GameObject[] todas = new GameObject[paredes1.Length + paredes2.Length + paredes3.Length];
    paredes1.CopyTo(todas, 0);
    paredes2.CopyTo(todas, paredes1.Length);
    paredes3.CopyTo(todas, paredes1.Length + paredes2.Length);

    float distanciaMinima = Mathf.Infinity;
    paredObjetivo = null;

    foreach (GameObject pared in todas)
    {
        if (pared == null) continue;

        // ✅ Usar el centro real del collider o renderer
        Vector3 centroPared = ObtenerCentro(pared);

        float distancia = Vector3.Distance(origenBusqueda, centroPared);
        if (distancia < distanciaMinima)
        {
            distanciaMinima = distancia;
            paredObjetivo = pared.transform;
        }
    }

    if (paredObjetivo != null)
        Debug.Log($"<color=yellow>Pared: {paredObjetivo.name} a {distanciaMinima}m</color>");
    else
        Debug.Log("<color=red>No encontró paredes</color>");
}

// ✅ Obtiene el centro real del objeto
private Vector3 ObtenerCentro(GameObject obj)
{
    Collider col = obj.GetComponent<Collider>();
    if (col != null) return col.bounds.center;

    Renderer rend = obj.GetComponent<Renderer>();
    if (rend != null) return rend.bounds.center;

    return obj.transform.position; // fallback al pivot
}

    private void RotarHaciaPared()
{
    if (paredObjetivo == null) return;

    // ✅ Centro real de la pared
    Vector3 centroPared = ObtenerCentro(paredObjetivo.gameObject);

    Vector3 dirMundo = centroPared - Camera.main.transform.position;
    dirMundo.y = 0;
    dirMundo.Normalize();


        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // Proyectar dirección en ejes de la cámara
        float lateral = Vector3.Dot(dirMundo, camRight);   // -1 izq, +1 der
        float frontal = Vector3.Dot(dirMundo, camForward); // -1 atrás, +1 frente

        // Ángulo 2D relativo a la cámara
        float anguloRelativo = Mathf.Atan2(lateral, frontal) * Mathf.Rad2Deg;

        // Aplicar offset: Z=85 cuando está al frente (anguloRelativo=0)
        float anguloFinal = offsetZ - anguloRelativo;

        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x,
            transform.localRotation.eulerAngles.y,
            anguloFinal
        );
    }

    private void OscilarHaciaFrente()
{
    if (paredObjetivo == null) return;

    float t = (Mathf.Sin(Time.time * velocidadOscilacion) + 1f) / 2f;
    float desplazamiento = Mathf.Lerp(0f, distanciaOscilacion, t);

    Vector3 centroPared = ObtenerCentro(paredObjetivo.gameObject);
    Vector3 dir = centroPared - transform.position;
    dir.y = 0;
    dir.Normalize();

    // ✅ Convertir dirección world space a local space del padre
    Vector3 dirLocal = transform.parent.InverseTransformDirection(dir);

    transform.localPosition = posicionBase + dirLocal * desplazamiento;
}

   private void OnDrawGizmos()
{
    if (paredObjetivo == null || Camera.main == null) return;

    Vector3 centro = ObtenerCentro(paredObjetivo.gameObject);

    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(Camera.main.transform.position, centro);

    Gizmos.color = Color.red;
    Gizmos.DrawSphere(centro, 0.3f);
}
}