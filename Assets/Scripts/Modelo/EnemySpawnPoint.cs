using UnityEngine;

// Agrega este componente al prefab de cada enemigo.
// Guarda automáticamente la posición/rotación de spawn para el respawn.
public class EnemySpawnPoint : MonoBehaviour
{
    [HideInInspector] public Vector3    spawnPosition;
    [HideInInspector] public Quaternion spawnRotation;

    void Awake()
    {
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }
}