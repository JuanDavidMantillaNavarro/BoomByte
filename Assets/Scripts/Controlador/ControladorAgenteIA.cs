using UnityEngine;

/*  AgentController.cs  —  Orquestador del agente
    El flujo de FixedUpdate es IDÉNTICO al ControladorAgente original,
    solo delegando cada responsabilidad al módulo correspondiente.

    Para Boombyte (3D): este archivo NO cambia.
*/

[RequireComponent(typeof(AgentBrain))]
[RequireComponent(typeof(PerceptionModule))]
[RequireComponent(typeof(MovementModule))]

public class AgentController : MonoBehaviour
{
    private AgentBrain brain;
    private PerceptionModule perception;
    private MovementModule movement;
    private Rigidbody2D rb;

    private AgentState state = new AgentState();
    

    void Start()
    {
        brain = GetComponent<AgentBrain>();
        perception = GetComponent<PerceptionModule>();
        movement = GetComponent<MovementModule>();
        rb = GetComponent<Rigidbody2D>();

        movement.Inicializar();
        state.ultimaPos = transform.position;
        state.distAnterior = perception.Distancia();
        state.ultimaAccion = 0;
    }

    void FixedUpdate()
    {
        Vector2 vectorAlJugador = perception.VectorAlJugador();
        float distancia = vectorAlJugador.magnitude;
        bool visionDirecta = perception.HayVisionDirecta();

        // 1. Detección de atasco
        float distMov = Vector2.Distance(transform.position, state.ultimaPos);
        state.ultimaPos = transform.position;

        // Si el agente está "persiguiendo" pero no se ha movido significativamente, 
        // o si detectamos una colisión frontal constante:
        bool colisionFrontal = movement.VerificarAtasco(state); 

        if (distMov < 0.02f || colisionFrontal) 
            state.tiempoQuieto += Time.fixedDeltaTime;
        else 
            state.tiempoQuieto = 0f;

        // 2. MÁQUINA DE ESTADOS
        if (!visionDirecta)
        {
            state.persiguiendo = false;
        }
        else if (distancia <= perception.radioDeteccion)
        {
            state.persiguiendo = true;
            state.patrullando = false;
        }

        // 3. PRIORIDAD DE EJECUCIÓN
        
        // A. Escape (Aseguramos que ultimaAccion sea válida antes de aprender)
        if (state.persiguiendo && state.tiempoQuieto > 0.5f)
        {
            if (state.ultimaAccion >= 0)
            {
                brain.Aprende(vectorAlJugador, state.ultimaAccion, -1f);
            }
            movement.IniciarEscape(state,vectorAlJugador);
        }

        // B. Ejecución
        if (state.patrullando)
        {
            movement.Patrullar(state);
        }
        else if (state.persiguiendo)
        {
            int accion = brain.ElegirAccion(vectorAlJugador);
            state.ultimaAccion = accion;
            Vector2 dirUsada = movement.MoverPorAccion(accion, state);

            bool huboChoque = movement.VerificarAtasco(state);

            float distActual = perception.Distancia();
            float recompensa = state.distAnterior - distActual;

            if (huboChoque) recompensa -= 0.5f; 
            else recompensa += Vector2.Dot(dirUsada.normalized, vectorAlJugador.normalized) * 0.1f;
            
            state.distAnterior = distActual;
            brain.Aprende(vectorAlJugador, accion, recompensa);
        }
        else
        {
            movement.Patrullar(state);
        }
    }
}