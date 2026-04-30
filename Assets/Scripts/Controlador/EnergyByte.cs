using UnityEngine;
using FMODUnity;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class EnergyByte : XRGrabInteractable
{
    [Header("Model")]
    public EnergyByteModel model;

    [Header("Config")]
    public LayerMask wallLayer;
    public GameObject explosionPrefab;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference grabSound;
    [SerializeField] private EventReference throwSound;
    [SerializeField] private EventReference explosionSound;

    private Rigidbody rb;
    private bool isFlying = false;
    private Animator animator;

    [SerializeField] private Animator explosionAnimator;
    [SerializeField] private Animator explosionAnimator2;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        GameObject globalExplosion = GameObject.Find("explo");

        if (globalExplosion != null)
        {
            Transform child1 = globalExplosion.transform.Find("explocion");
            Transform child2 = globalExplosion.transform.Find("explocion (1)");

            if (child1 != null)
                explosionAnimator = child1.GetComponent<Animator>();

            if (child2 != null)
                explosionAnimator2 = child2.GetComponent<Animator>();
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        RuntimeManager.PlayOneShot(grabSound, transform.position);

        isFlying = false;
        GameController.Instance.OnBallGrab();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        RuntimeManager.PlayOneShot(throwSound, transform.position);

        isFlying = true;

        Vector3 rawVelocity = rb.linearVelocity;
        Vector3 center = GetCentroCorredor(transform.position);
        Vector3 directionToCenter = center - transform.position;

        Vector3 correctedVelocity = rawVelocity;
        correctedVelocity.x = Mathf.Lerp(rawVelocity.x, directionToCenter.x * 5f, 0.5f);
        correctedVelocity.z = Mathf.Lerp(rawVelocity.z, directionToCenter.z * 5f, 0.5f);

        rb.linearVelocity = correctedVelocity * model.extraThrowForce;
    }

    protected virtual void FixedUpdate()
    {
        if (!isSelected && isFlying)
            Magnetismo();
    }

    private void Magnetismo()
    {
        Vector3 center = GetCentroCorredor(transform.position);
        Vector3 diff = center - transform.position;
        diff.y = 0;

        if (diff.magnitude > 0.02f)
            rb.AddForce(diff * model.magnetForce, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isFlying && collision.gameObject.CompareTag(model.groundTag))
        {
            isFlying = false;
            SnapCentro();

            Invoke(nameof(ExecuteExplosion), model.delayBeforeExplode);

            if (animator != null)
                animator.enabled = true;
        }
    }

    private void ExecuteExplosion()
    {
        Vector3 pos = transform.position;

        RuntimeManager.PlayOneShot(explosionSound, pos);

        GameController.Instance.OnBallExploded(pos, model.explosionGridRadius, explosionPrefab);
        GameController.Instance.RegisterBallDestroyed();

        if (explosionAnimator != null)
        {
            explosionAnimator.transform.position = pos;
            explosionAnimator.gameObject.SetActive(true);
            explosionAnimator.SetTrigger("Explode");
        }

        if (explosionAnimator2 != null)
        {
            explosionAnimator2.transform.position = pos;
            explosionAnimator2.gameObject.SetActive(true);
            explosionAnimator2.SetTrigger("Explode");
        }

        Destroy(gameObject);
    }

    private void SnapCentro()
    {
        Vector3 center = GetCentroCorredor(transform.position);

        transform.position = new Vector3(center.x, transform.position.y, center.z);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCentroCorredor(Vector3 currentPos)
    {
        RaycastHit hitL, hitR, hitF, hitB;

        bool wLeft = Physics.Raycast(currentPos, Vector3.left, out hitL, model.raycastDistance, wallLayer);
        bool wRight = Physics.Raycast(currentPos, Vector3.right, out hitR, model.raycastDistance, wallLayer);
        bool wForward = Physics.Raycast(currentPos, Vector3.forward, out hitF, model.raycastDistance, wallLayer);
        bool wBack = Physics.Raycast(currentPos, Vector3.back, out hitB, model.raycastDistance, wallLayer);

        float cx = currentPos.x;
        float cz = currentPos.z;

        if (wLeft && wRight)
            cx = (hitL.point.x + hitR.point.x) / 2f;

        if (wForward && wBack)
            cz = (hitF.point.z + hitB.point.z) / 2f;

        return new Vector3(cx, currentPos.y, cz);
    }
}