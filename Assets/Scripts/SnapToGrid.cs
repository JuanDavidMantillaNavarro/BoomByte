using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapToGrid : MonoBehaviour
{
    public float gridSize = 0.5f;
    public Vector3 gridOffset = new Vector3(0.25f, 0, 0);
    public float magnetForce = 5f; // fuerza del imán, prueba entre 3 y 15

    bool isSnapped = false;
    bool hasTouchedGround = false;
    bool isReleased = false;
    Coroutine snapRoutine;
    XRGrabInteractable grab;
    Rigidbody rb;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isSnapped = false;
        isReleased = false;
        hasTouchedGround = false;

        rb.linearVelocity = Vector3.zero;  
        rb.angularVelocity = Vector3.zero;

        if (snapRoutine != null)
        {
            StopCoroutine(snapRoutine);
            snapRoutine = null;
        }
    }

    Vector3 GetNearestSnapPoint()
    {
        float snapSize = gridSize * 2f; // snap cada 2 casillas
        Vector3 pos = transform.position - gridOffset;
        float x = Mathf.Round(pos.x / snapSize) * snapSize;
        float z = Mathf.Round(pos.z / snapSize) * snapSize;
        return new Vector3(x, transform.position.y, z) + gridOffset;
    }

    void FixedUpdate()
    {
        if (!isReleased || isSnapped || grab.isSelected) return;

        Vector3 snapPoint = GetNearestSnapPoint();
        Vector3 direction = snapPoint - transform.position;
        direction.y = 0; // solo en X y Z, Y lo maneja la gravedad

        rb.AddForce(direction * magnetForce, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !grab.isSelected && !isSnapped)
            hasTouchedGround = true;
    }

    IEnumerator SnapWhenStill()
    {
        while (!hasTouchedGround)
        {
            if (grab.isSelected) yield break;
            yield return null;
        }

        // Espera a que se quede quieta
        yield return new WaitForSeconds(0.1f);

        while (rb.linearVelocity.magnitude > 0.05f)
        {
            if (grab.isSelected) yield break;
            yield return null;
        }

        if (!grab.isSelected && hasTouchedGround)
        {
            isReleased = false; // apaga el imán
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 snapPoint = GetNearestSnapPoint();
            transform.position = snapPoint;

            rb.isKinematic = true;
            isSnapped = true;
            rb.isKinematic = false;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isReleased = true;

        if (snapRoutine != null)
            StopCoroutine(snapRoutine);

        snapRoutine = StartCoroutine(SnapWhenStill());
    }
}