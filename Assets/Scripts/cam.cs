using UnityEngine;

public class cam : MonoBehaviour
{
void Awake()
{
    Debug.Log("AWAKE POS: " + transform.position);
}

void OnEnable()
{
    Debug.Log("ONENABLE POS: " + transform.position);
}

void Start()
{
    Debug.Log("START POS: " + transform.position);
}
}
