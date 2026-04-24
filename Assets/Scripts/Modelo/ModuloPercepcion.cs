using UnityEngine;

/* PerceptionModule.cs (Versión 3D)
    Responsabilidad: detectar al jugador y decidir transiciones de estado.
*/
public class PerceptionModule : MonoBehaviour
{
    [Header("Rangos de detección")]
    public float radioDeteccion = 20f;
    public float radioPerdida = 25f;
    [Tooltip("Diferencia de altura máxima permitida para considerar al jugador en el mismo 'piso'")]
    public float toleranciaAltura = 100f;

    [Header("Visión directa")]
    public bool usarVisionDirecta = false; // activa el raycast cuando estés listo

     [Tooltip("Capas que bloquean la visión (paredes, obstáculos)")]
    public LayerMask capasObstaculo;
 
    [Header("Referencias")]
    public Transform jugador;
 
    void Start()
    {
        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) jugador = go.transform;
        }
    }
 
    // ── Consultas ────────────────────────────────────────────────────────
 
    /// Vector 3D desde el agente hasta el jugador.
    public Vector3 VectorAlJugador()
        => jugador != null ? jugador.position - transform.position : Vector3.zero;
 
    /// Distancia total 3D.
    public float Distancia()
        => VectorAlJugador().magnitude;
 
    /// Distancia en el plano XZ (ignora altura). Usada para decisiones de rango.
    public float DistanciaPlana()
    {
        if (jugador == null) return float.MaxValue;
        Vector2 a = new Vector2(transform.position.x, transform.position.z);
        Vector2 b = new Vector2(jugador.position.x,   jugador.position.z);
        return Vector2.Distance(a, b);
    }
 
    /// ¿El jugador está dentro del radio de detección (en plano XZ)?
    public bool JugadorEnRango()
        => jugador != null && DistanciaPlana() <= radioDeteccion;
 
    /// ¿El jugador salió del radio de persecución?
    public bool JugadorSalioDeRango()
        => DistanciaPlana() > radioPerdida;
 
    /// ¿Hay línea de visión directa hasta el jugador?
    /// Mientras usarVisionDirecta = false devuelve true si está en rango.
    public bool HayVisionDirecta()
    {
        if (jugador == null) return false;
        if (!JugadorEnRango()) return false;
 
        if (!usarVisionDirecta)
            return true; // sin raycast, si está en rango lo "ve"
 
        // Raycast desde la cabeza del agente hasta la cabeza del jugador
        Vector3 origen    = transform.position + Vector3.up * 1.5f;
        Vector3 destino   = jugador.position   + Vector3.up * 1.6f;
        Vector3 direccion = (destino - origen).normalized;
        float   distancia = Vector3.Distance(origen, destino);

        Debug.DrawLine(origen, destino, Color.red);
 
        if (Physics.Raycast(origen, direccion, out RaycastHit hit, distancia, capasObstaculo))
        {
            // Si lo primero que toca es el jugador (o un hijo suyo), hay visión
            return hit.transform.IsChildOf(jugador) || hit.transform == jugador;
        }
        return true; // no hay nada bloqueando
    }
}