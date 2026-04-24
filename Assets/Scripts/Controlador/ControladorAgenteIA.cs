using UnityEngine;

[RequireComponent(typeof(AgentBrain))]
[RequireComponent(typeof(PerceptionModule))]
[RequireComponent(typeof(MovementModule))]
public class AgentController : MonoBehaviour
{
    private AgentBrain brain;
    private PerceptionModule perception;
    private MovementModule movement;
    private Rigidbody rb; 

    private AgentState state = new AgentState();

    [Header("Anti-atasco")]
    [Tooltip("Segundos quieto antes de activar el escape. Sube si el agente escapa demasiado.")]
    public float umbralAtasco = 0.5f;
    void Start()
    {
        brain = GetComponent<AgentBrain>();
        perception = GetComponent<PerceptionModule>();
        movement = GetComponent<MovementModule>();
        rb = GetComponent<Rigidbody>(); // CAMBIO: Rigidbody (3D)

        movement.Inicializar();
        state.ultimaPos = transform.position;
        state.distAnterior = perception.Distancia();
        state.ultimaAccion = 0;
    }

    void FixedUpdate()
    {
        // CAMBIO: Usamos Vector3 en todo el flujo
        Vector3 vectorAlJugador = perception.VectorAlJugador();
        float distancia = vectorAlJugador.magnitude;
        bool visionDirecta = perception.HayVisionDirecta();

        // 1. Detección de atasco
        float distMov = Vector3.Distance(transform.position, state.ultimaPos);
        state.ultimaPos = transform.position;

        //bool colisionFrontal = movement.VerificarAtasco(state); 

        if (distMov < 0.02f) 
            state.tiempoQuieto += Time.fixedDeltaTime;
        else 
            state.tiempoQuieto = 0f;

        // 2. MÁQUINA DE ESTADOS
        if (!visionDirecta && distancia <= perception.radioDeteccion)
        {
            // Ve al jugador → perseguir
            state.persiguiendo = true;
            state.patrullando  = false;
            Debug.Log("persigue");
        }
        else if (state.persiguiendo && !visionDirecta && distancia > perception.radioPerdida)
        {
            // Perdió al jugador → volver a patrullar
            state.persiguiendo = false;
        }

        // 3. PRIORIDAD DE EJECUCIÓN
        if (state.persiguiendo && state.tiempoQuieto > umbralAtasco)
        {
            // Penaliza la última acción que lo dejó quieto
            if (state.ultimaAccion >= 0)
                brain.Aprende(vectorAlJugador, state.ultimaAccion, -1f);
            
            movement.IniciarEscape(state, vectorAlJugador);
        }

        // B. Ejecución
        if (state.patrullando)
        {
            movement.Patrullar(state);
            return;
        }
        else if (state.persiguiendo)
        {
           /// Detectar si hay pared en la dirección que va a elegir la IA
            // (solo para calcular recompensa, NO para cambiar dirección)
            int accion = brain.ElegirAccion(vectorAlJugador);
            state.ultimaAccion = accion;
 
            Vector3 dirElegida = movement.MoverPorAccion(accion, state);
 
            // Recompensa: acercarse al jugador + alineación
            float distActual = perception.Distancia();
            float recompensa  = state.distAnterior - distActual;
            recompensa += Vector3.Dot(dirElegida.normalized,
                          new Vector3(vectorAlJugador.x, 0f, vectorAlJugador.z).normalized) * 0.1f;
 
            // Penalización si hay pared al frente de donde se movió
            if (movement.HayChoqueFrontal(dirElegida))
                recompensa -= 1f;
 
            state.distAnterior = distActual;
            brain.Aprende(vectorAlJugador, accion, recompensa);
        }
        else
        {
            movement.Patrullar(state);
        }
    }
}