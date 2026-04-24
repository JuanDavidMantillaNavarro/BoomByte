using UnityEngine;

/*  CollisionHandler.cs  —  Colisiones del agente
    Lógica IDÉNTICA al CollisionAgente.cs original.
    Añade la llamada a DarRecompensa al brain (que antes estaba en ControladorAgente).

    Para Boombyte (3D): OnCollisionEnter2D → OnCollisionEnter
*/

[RequireComponent(typeof(AgentBrain))]
public class CollisionHandler : MonoBehaviour
{
    private AgentBrain brain;
    private GameObject jugador;

    void Start()
    {
        brain   = GetComponent<AgentBrain>();
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        // Usamos el jugador encontrado en el start
        if (collision.gameObject.CompareTag("Player"))
        {
            brain.DarRecompensa(+10f); // Se asegura que el cerebro reciba el +10f
            Debug.Log("¡La IA le gana al jugador!");
            GameController.Instance.OnEnemyCollide();
            Destroy(gameObject);
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
    // Verificamos si el enemigo tocó el "PlayerTarget"
    if (other.CompareTag("Player")) 
    {
        brain.DarRecompensa(+10f); // Se asegura que el cerebro reciba el +10f
            Debug.Log("¡La IA le gana al jugador!");
            GameController.Instance.OnEnemyCollide();
            Destroy(gameObject);
    }
    }
}