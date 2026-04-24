using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [Header("Referencia")]
    public Transform playerCamera;

    [Header("Posición")]
    public float distancia = 1.5f;
    public float alturaOffset = -0.2f;

    void LateUpdate()
    {
        if (playerCamera == null) return;

        Vector3 posicion =
            playerCamera.position +
            playerCamera.forward * distancia;

        posicion.y += alturaOffset;

        transform.position = posicion;

        // mirar al jugador correctamente
        transform.rotation =
            Quaternion.LookRotation(transform.position - playerCamera.position);
    }
}