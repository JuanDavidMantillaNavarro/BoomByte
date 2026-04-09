using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
