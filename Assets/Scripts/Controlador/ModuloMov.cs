using UnityEngine;

/* MovementModule.cs (Versión 3D)
    Responsabilidad: física de movimiento, patrulla, esquiva y animación en 3D.
*/
public class MovementModule : MonoBehaviour
{
    [Header("Velocidad")]
    public float vel = 1.5f;

    [Header("Referencias")]
    public Animator anim;

    // ── Internos ─────────────────────────────────────────────────────────
    private Rigidbody rb;

    private static readonly Vector3[] AccionADir =
    {
        Vector3.forward,  // Arriba en 2D -> Adelante en 3D
        Vector3.back,     // Abajo en 2D -> Atras en 3D
        Vector3.left,
        Vector3.right
    };

    public void Inicializar()
    {
        rb = GetComponent<Rigidbody>();
        anim = anim != null ? anim : GetComponent<Animator>();
        // Ya no usamos SpriteRenderer
    }

    public Vector3 MoverPorAccion(int accion, AgentState state)
    {
        Vector3 mov = (accion >= 0 && accion < AccionADir.Length)
            ? AccionADir[accion]
            : Vector3.zero;

        rb.MovePosition(rb.position + mov * vel * Time.fixedDeltaTime);
        ActualizarAnimacion(mov);
        Debug.Log ("Mover accion");
        return mov;
    }

    public bool Patrullar(AgentState state)
    {
        if (!state.patrullando)
        {
            Debug.Log ("Patrullando temporal 2 a 4seg");
            state.tiempoPatrulla = Random.Range(2f, 4f);
            Vector3[] dirs = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            state.dirPatrulla = dirs[Random.Range(0, dirs.Length)];
            // Rotación en el eje Y (vertical) para 3D
            state.dirPatrulla = Quaternion.Euler(0, Random.Range(-20f, 20f), 0) * state.dirPatrulla;
            state.patrullando = true;
        }

        state.tiempoPatrulla -= Time.fixedDeltaTime;

        // Durante patrulla SÍ redirigimos si hay pared al frente
        if (DetectarColisionFrontal(state.dirPatrulla))
        {
            Debug.Log ("Pared patrullando");
            state.dirPatrulla = Quaternion.Euler(0, 90f, 0) * state.dirPatrulla;
            state.tiempoPatrulla = Random.Range(1f, 2f); // nuevo segmento
        }

        rb.MovePosition(rb.position + state.dirPatrulla.normalized * vel * 0.7f * Time.fixedDeltaTime);
        ActualizarAnimacion(state.dirPatrulla);

        if (state.tiempoPatrulla <= 0f || DetectarColisionFrontal(state.dirPatrulla))
            state.patrullando = false;

        return state.patrullando;
    }

    public void IniciarEscape(AgentState state, Vector3 vectorAlJugador)
    {
        Debug.Log ("Escape");
        // Elige una dirección perpendicular al jugador para rodear obstáculos
        Vector3 dirHaciaJugador = new Vector3(vectorAlJugador.x, 0f, vectorAlJugador.z).normalized;
        Vector3 perpendicular   = Quaternion.Euler(0, 90f, 0) * dirHaciaJugador;
 
        state.patrullando    = true;
        state.tiempoPatrulla = Random.Range(1f, 2f);
        state.dirPatrulla    = perpendicular;
        state.tiempoQuieto   = 0f;
    }

    //VERIFICAR CHOQUE FRONTAL
    //  Solo se usa para DETECTAR — no modifica dirPatrulla.
    //  Retorna true si hay obstáculo en la dirección dada.

     public bool HayChoqueFrontal(Vector3 dir)
    {
        float distRaycast = 0.5f; 
        int mask = LayerMask.GetMask("Walls", "P");
        return Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            dir, distRaycast, mask);
    }

    public bool VerificarAtasco(AgentState state)
    {
        Debug.Log ("Atasco");
        return HayChoqueFrontal(state.dirPatrulla);
    }

    private bool DetectarColisionFrontal(Vector3 dir)
    {
        int mask = LayerMask.GetMask("Walls", "P");
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 0.5f, mask);
    }

    public void ActualizarAnimacion(Vector3 dir)
    {
        if (anim == null) return;

        if (dir.magnitude > 0.01f)
        {
            anim.SetBool("isMoving", true);
            
            // Orientar el objeto 3D hacia la dirección de movimiento
            transform.LookAt(transform.position + dir);
            
            // Si tu Animator sigue usando parámetros X/Y, mantenlos, 
            // pero normalmente en 3D se usa una variable "speed" o "velocity"
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.z); 
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public void Detener()
    {
        ActualizarAnimacion(Vector3.zero);
    }
}