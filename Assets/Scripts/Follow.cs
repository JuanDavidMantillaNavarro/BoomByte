using UnityEngine;

public class Follow : MonoBehaviour
{
    [Header("Referencia del jugador")]
    public Transform playerCamera;

    [Header("Activación")]
    public float activationDistance = 4f;
    public bool onlyWhileClose = true;

    [Header("Suavidad")]
    public float rotationSpeed = 8f;

    [Header("Corrección de orientación")]
    public Vector3 rotationOffset = new Vector3(0f, 180f, 0f);

    [Header("Límites de giro")]
    public bool limitRotation = false;
    public float maxYaw = 45f;
    public float maxPitch = 30f;

    private Quaternion initialRotation;
    private Vector3 fixedPosition;

    void Start()
    {
        fixedPosition = transform.position;
        initialRotation = transform.rotation;

        if (playerCamera == null && Camera.main != null)
            playerCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (playerCamera == null) return;

        transform.position = fixedPosition;

        float distance = Vector3.Distance(playerCamera.position, fixedPosition);

        if (onlyWhileClose && distance > activationDistance)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                initialRotation,
                Time.deltaTime * rotationSpeed
            );
            return;
        }

        Vector3 dir = playerCamera.position - fixedPosition;

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(rotationOffset);

        if (limitRotation)
        {
            Quaternion delta = Quaternion.Inverse(initialRotation) * targetRotation;
            Vector3 euler = NormalizeEuler(delta.eulerAngles);

            euler.y = Mathf.Clamp(euler.y, -maxYaw, maxYaw);
            euler.x = Mathf.Clamp(euler.x, -maxPitch, maxPitch);
            euler.z = 0f;

            targetRotation = initialRotation * Quaternion.Euler(euler);
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    Vector3 NormalizeEuler(Vector3 euler)
    {
        euler.x = NormalizeAngle(euler.x);
        euler.y = NormalizeAngle(euler.y);
        euler.z = NormalizeAngle(euler.z);
        return euler;
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}