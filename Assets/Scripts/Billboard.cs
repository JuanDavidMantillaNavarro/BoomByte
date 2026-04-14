using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float distanceFromCamera = 2f; // distancia frente al jugador
    public float heightOffset = -0.2f; // pequeño ajuste vertical

    void Update()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // POSICIÓN: siempre frente a la cámara
        Vector3 forward = cam.transform.forward;
        Vector3 targetPosition = cam.transform.position + forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        transform.position = targetPosition;

        // ROTACIÓN: siempre mirando a la cámara
        transform.LookAt(cam.transform);
        transform.Rotate(0, 180, 0);
    }
}
