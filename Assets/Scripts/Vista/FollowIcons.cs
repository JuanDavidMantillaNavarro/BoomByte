using UnityEngine;

public class FollowIcons : MonoBehaviour
{
    public Transform target;

    private float offsetX;
    private float offsetZ;
    private float fixedY;

    void Start()
    {
        if (target == null) return;

        offsetX = transform.position.x - target.position.x;
        offsetZ = transform.position.z - target.position.z;

        fixedY = transform.position.y; // Y se queda fijo
    }

    void Update()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.position.x + offsetX, // X sigue X
            fixedY,                      // Y fijo
            target.position.z + offsetZ  // Z sigue Z
        );
    }
}