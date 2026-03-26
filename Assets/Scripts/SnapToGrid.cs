using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapToGrid : MonoBehaviour
{
    public float gridSize = 0.5f;
    public Vector3 gridOffset = new Vector3(0.25f, 0, 0);
    public float magnetForce = 8f;
    public LayerMask wallLayer;

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

    Vector3 GetCorridorCenter()
    {
        RaycastHit hitLeft, hitRight, hitForward, hitBack;
        bool wLeft   = Physics.Raycast(transform.position, Vector3.left,    out hitLeft,   10f, wallLayer);
        bool wRight  = Physics.Raycast(transform.position, Vector3.right,   out hitRight,  10f, wallLayer);
        bool wFwd    = Physics.Raycast(transform.position, Vector3.forward,  out hitForward, 10f, wallLayer);
        bool wBack   = Physics.Raycast(transform.position, Vector3.back,    out hitBack,   10f, wallLayer);

        float centerX = transform.position.x;
        float centerZ = transform.position.z;

        // Paredes en ambos lados en X → pasillo va en Z → centra en X
        if (wLeft && wRight)
            centerX = (hitLeft.point.x + hitRight.point.x) / 2f;

        // Paredes en ambos lados en Z → pasillo va en X → centra en Z
        if (wFwd && wBack)
            centerZ = (hitForward.point.z + hitBack.point.z) / 2f;

        return new Vector3(centerX, transform.position.y, centerZ);
    }

    Vector3 GetNearestSnapPoint()
    {
        Vector3 corridorCenter = GetCorridorCenter();

        RaycastHit hitLeft, hitRight, hitForward, hitBack;
        bool wLeft  = Physics.Raycast(transform.position, Vector3.left,    out hitLeft,   10f, wallLayer);
        bool wRight = Physics.Raycast(transform.position, Vector3.right,   out hitRight,  10f, wallLayer);
        bool wFwd   = Physics.Raycast(transform.position, Vector3.forward, out hitForward, 10f, wallLayer);
        bool wBack  = Physics.Raycast(transform.position, Vector3.back,    out hitBack,   10f, wallLayer);

        bool corridorAlongZ = wLeft && wRight;   // paredes en X → pasillo en Z
        bool corridorAlongX = wFwd && wBack;     // paredes en Z → pasillo en X

        if (corridorAlongZ)
        {
            // Imán en X al centro del pasillo, Z snapea libre a la grilla
            Vector3 pos = transform.position - gridOffset;
            float z = Mathf.Round(pos.z / gridSize) * gridSize + gridOffset.z;
            return new Vector3(corridorCenter.x, transform.position.y, z);
        }
        else if (corridorAlongX)
        {
            // Imán en Z al centro del pasillo, X snapea libre a la grilla
            Vector3 pos = transform.position - gridOffset;
            float x = Mathf.Round(pos.x / gridSize) * gridSize + gridOffset.x;
            return new Vector3(x, transform.position.y, corridorCenter.z);
        }
        else
        {
            // Esquina o sin detección clara — snapea a grilla normal
            Vector3 pos = transform.position - gridOffset;
            float x = Mathf.Round(pos.x / gridSize) * gridSize + gridOffset.x;
            float z = Mathf.Round(pos.z / gridSize) * gridSize + gridOffset.z;
            return new Vector3(x, transform.position.y, z);
        }
    }

    void FixedUpdate()
    {
        if (!isReleased || isSnapped || grab.isSelected) return;

        Vector3 corridorCenter = GetCorridorCenter();

        RaycastHit hitLeft, hitRight, hitForward, hitBack;
        bool wLeft  = Physics.Raycast(transform.position, Vector3.left,    out hitLeft,   10f, wallLayer);
        bool wRight = Physics.Raycast(transform.position, Vector3.right,   out hitRight,  10f, wallLayer);
        bool wFwd   = Physics.Raycast(transform.position, Vector3.forward, out hitForward, 10f, wallLayer);
        bool wBack  = Physics.Raycast(transform.position, Vector3.back,    out hitBack,   10f, wallLayer);

        bool corridorAlongZ = wLeft && wRight;
        bool corridorAlongX = wFwd && wBack;

        Vector3 force = Vector3.zero;

        if (corridorAlongZ)
        {
            // Solo atrae en X hacia el centro del pasillo
            float diffX = corridorCenter.x - transform.position.x;
            force = new Vector3(diffX, 0, 0);
        }
        else if (corridorAlongX)
        {
            // Solo atrae en Z hacia el centro del pasillo
            float diffZ = corridorCenter.z - transform.position.z;
            force = new Vector3(0, 0, diffZ);
        }

        rb.AddForce(force * magnetForce, ForceMode.Acceleration);
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

        yield return new WaitForSeconds(0.1f);

        while (rb.linearVelocity.magnitude > 0.05f)
        {
            if (grab.isSelected) yield break;
            yield return null;
        }

        if (!grab.isSelected && hasTouchedGround)
        {
            isReleased = false;
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