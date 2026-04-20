using UnityEngine;

/*  AgentState.cs
    Contenedor de datos del agente. Sin lógica propia.
    Todos los módulos leen y escriben aquí.

    Para Boombyte (3D): cambia Vector2 → Vector3.
*/
public class AgentState
{
    public bool    persiguiendo  = false;
    public bool    patrullando   = false;

    public Vector3 ultimaPos     = Vector3.zero;
    public int     ultimaAccion  = -1;
    public float   tiempoQuieto  = 0f;
    public float   distAnterior  = 0f;

    // Dirección actual de patrulla (la escribe MovementModule)
    public Vector3 dirPatrulla   = Vector3.right;
    public float   tiempoPatrulla = 0f;
}