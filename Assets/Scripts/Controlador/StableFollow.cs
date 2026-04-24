using UnityEngine;

public class StableFollow : MonoBehaviour
{
    public Transform targetToFollow; // Asigna aquí la Main Camera
    public Vector3 offset; // Posición deseada en la cintura

    void LateUpdate()
    {
        // Fuerza la posición basada en la cámara (tu cabeza), 
        // pero reseteamos el eje Y para que siempre esté a una altura fija
        // y eliminamos el giro para que no se "vaya al frente".
        transform.position = new Vector3(targetToFollow.position.x, 1.2f, targetToFollow.position.z);
        
        // Mantenemos rotación neutral
        transform.rotation = Quaternion.identity; 
    }
}