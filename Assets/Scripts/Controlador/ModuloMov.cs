using UnityEngine;

/* MovementModule.cs (Versión 3D)
    Responsabilidad: física de movimiento, patrulla, esquiva y animación en 3D.
*/
public class MovementModule : MonoBehaviour
{
    [Header("Velocidad")]
    public float vel = 3f;

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

        ActualizarAnimacion(mov);
        state.dirPatrulla = mov; 

        VerificarAtasco(state);
        rb.MovePosition(rb.position + mov * vel * Time.fixedDeltaTime);

        return mov;
    }

    public bool Patrullar(AgentState state)
    {
        if (!state.patrullando)
        {
            state.tiempoPatrulla = Random.Range(2f, 4f);
            Vector3[] dirs = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
            state.dirPatrulla = dirs[Random.Range(0, dirs.Length)];
            // Rotación en el eje Y (vertical) para 3D
            state.dirPatrulla = Quaternion.Euler(0, Random.Range(-20f, 20f), 0) * state.dirPatrulla;
            state.patrullando = true;
        }

        state.tiempoPatrulla -= Time.fixedDeltaTime;
        rb.MovePosition(rb.position + state.dirPatrulla.normalized * vel * 0.7f * Time.fixedDeltaTime);
        ActualizarAnimacion(state.dirPatrulla);

        if (state.tiempoPatrulla <= 0f || DetectarColisionFrontal(state.dirPatrulla))
            state.patrullando = false;

        return state.patrullando;
    }

    public void IniciarEscape(AgentState state, Vector3 vectorAlJugador)
    {
        state.patrullando = true;
        state.persiguiendo = false;
        state.tiempoPatrulla = Random.Range(1.0f, 2.0f);

        Vector3 dirHaciaJugador = vectorAlJugador.normalized;
        // Rotación de 90 grados en el eje Y para buscar tangente
        state.dirPatrulla = Quaternion.Euler(0, 90f, 0) * dirHaciaJugador;
        state.tiempoQuieto = 0f;
    }

    public bool VerificarAtasco(AgentState state)
    {
        float distRaycast = 0.5f;
        int mask = LayerMask.GetMask("Walls", "P");
        
        // Raycast 3D
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, state.dirPatrulla, distRaycast, mask))
        {
            state.dirPatrulla = Quaternion.Euler(0, 90f, 0) * state.dirPatrulla;
            state.tiempoPatrulla = 0.3f;
            return true;
        }
        return false;
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