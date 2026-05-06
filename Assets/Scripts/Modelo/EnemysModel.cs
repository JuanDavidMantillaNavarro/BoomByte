using UnityEngine;

[System.Serializable]

public class EnemysModel
{
    //Error de Código
    public float speedError = 3f;

    // Dirección actual (solo ejes)
    public Vector3 direction = Vector3.right;

    // Distancia para detectar pared
    public float detectionDistance = 0.5f;
    public float rotationSpeed = 5f;

    // Layer de paredes
    public LayerMask [] wallLayer;
}
