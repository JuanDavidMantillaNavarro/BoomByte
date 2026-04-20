using UnityEngine;

/* PerceptionModule.cs (Versión 3D)
    Responsabilidad: detectar al jugador y decidir transiciones de estado.
*/
public class PerceptionModule : MonoBehaviour
{
    [Header("Rangos de detección")]
    public float radioDeteccion = 3f;
    public float radioPerdida = 3.5f;
    [Tooltip("Diferencia de altura máxima permitida para considerar al jugador en el mismo 'piso'")]
    public float toleranciaAltura = 0.5f;

    [Header("Referencias")]
    public Transform jugador;

    void Start()
    {
        if (jugador == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null) jugador = playerGO.transform;
        }
    }

    // ── Consultas ────────────────────────────────────────────────────────

    // Vector dirección (posJugador - posAgente) en 3D
    public Vector3 VectorAlJugador()
        => (jugador != null) ? (jugador.position - transform.position) : Vector3.zero;

    public float Distancia()
        => VectorAlJugador().magnitude;

    // ¿El jugador entró al rango Y está en el mismo nivel de altura?
    public bool JugadorEnRango()
    {
        if (jugador == null) return false;
        
        bool distanciaCorrecta = Distancia() <= radioDeteccion;
        bool mismaAltura = Mathf.Abs(transform.position.y - jugador.position.y) <= toleranciaAltura;
        
        return distanciaCorrecta && mismaAltura;
    }

    // Obstáculos entre jugador y agente usando Raycast 3D
    public bool HayVisionDirecta()
    {
        if (jugador == null) return false;
        
        float dist = Distancia();
        if (dist > radioDeteccion) return false;

        Vector3 dir = VectorAlJugador();
        // Lanzamos rayo desde el centro del objeto hacia el jugador
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir.normalized, out RaycastHit hit, dist))
        {
            // Retorna true solo si lo que golpeamos es el jugador
            return hit.transform == jugador;
        }
        
        return false;
    }

    public bool JugadorSalioDeRango()
        => Distancia() > radioPerdida;

    public bool JugadorMuyCerca() 
        => Distancia() < 2.5f;

}