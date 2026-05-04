using UnityEngine;

public class ParedObjetivo : MonoBehaviour
{
    [Tooltip("Tiempo en segundos entre cada comprobación para no saturar la CPU")]
    public float checkInterval = 1.0f;

    void Start()
    {
        // Inicia una repetición que revisa cada 'checkInterval' segundos
        InvokeRepeating(nameof(CheckForDestructibles), 0f, checkInterval);
    }

    void CheckForDestructibles()
    {
        // Busca si existe AL MENOS un objeto activo con el tag "Destructible"
        GameObject destructible = GameObject.FindGameObjectWithTag("Destructible");

        // Si no se encuentra ninguno (es null), destruye este objeto
        if (destructible == null)
        {
            Debug.Log("No quedan objetos destructibles. Autodestruyendo: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
