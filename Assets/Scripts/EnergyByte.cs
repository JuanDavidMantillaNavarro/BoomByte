using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class EnergyByte : XRGrabInteractable
{
    [Header("Maze Magnet Settings")]
    public float magnetForce = 10f; // Fuerza suave en el aire
    public LayerMask wallLayer;
    public string groundTag = "Ground";

    [Header("Explosion Settings")]
    public GameObject explosionPrefab; // El objeto con la animación
    public float delayBeforeExplode = 2f;
    
    [Header("Power Up Settings")]
    public float extraThrowForce = 1.5f; // Prueba con 1.5 o 2 para que se note
    public float explosionGridRadius = 2f; // Power-up variable

    private Rigidbody rb;
    private bool isFlying = false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // 1. Ejecutar base para que XRIT calcule la velocidad de la mano
        base.OnSelectExited(args);
        
        isFlying = true;

        // 2. Corregir la trayectoria inicial sin perder potencia
        Vector3 rawVelocity = rb.linearVelocity;
        Vector3 center = GetCorridorCenter(transform.position);
        Vector3 directionToCenter = (center - transform.position);

        // Mantener la fuerza vertical (Y) y la fuerza de avance, 
        // pero redirigir el exceso lateral hacia el centro.
        Vector3 correctedVelocity = rawVelocity;

        // Si el pasillo es longitudinal (Z), alineamos la X al centro
        // Si el pasillo es transversal (X), alineamos la Z al centro
        // Pero usamos un simple Lerp para no ser bruscos
        correctedVelocity.x = Mathf.Lerp(rawVelocity.x, directionToCenter.x * 5f, 0.5f);
        correctedVelocity.z = Mathf.Lerp(rawVelocity.z, directionToCenter.z * 5f, 0.5f);

        // 3. Aplicar el multiplicador de poder (afecta a todos los ejes por igual)
        rb.linearVelocity = correctedVelocity * extraThrowForce;
    }

    protected virtual void FixedUpdate()
    {
        if (!isSelected && isFlying)
        {
            // Imán suave en el aire para ayudar a la trayectoria
            ApplyAirMagnetism();
        }
    }

    private void ApplyAirMagnetism()
    {
        Vector3 center = GetCorridorCenter(transform.position);
        Vector3 diff = center - transform.position;
        diff.y = 0;

        // Solo aplicamos fuerza si no estamos ya muy cerca del centro
        if (diff.magnitude > 0.02f)
        {
            rb.AddForce(diff * magnetForce, ForceMode.Acceleration);
        }
    }

    // --- DETECCIÓN DE SUELO PARA SNAP INMEDIATO ---
    private void OnCollisionEnter(Collision collision)
    {
        if (isFlying && collision.gameObject.CompareTag(groundTag))
        {
            isFlying = false;
            SnapToCenterAndStop();

            Invoke(nameof(ExecuteExplosion), delayBeforeExplode);
        }
    }

    private void ExecuteExplosion()
    {
        // Instanciamos el efecto visual
        GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        // Pasamos los datos del power-up al script de la explosión
        var expScript = fx.GetComponent<EnergyExplosion>();
        if(expScript != null) expScript.Initialize(explosionGridRadius);

        Destroy(gameObject); // Destruimos la bola
    }

    private void SnapToCenterAndStop()
    {
        Vector3 center = GetCorridorCenter(transform.position);
        
        // Snap de posición inmediato
        transform.position = new Vector3(center.x, transform.position.y, center.z);
        
        // Matar inercias para que no rebote ni se deslice
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCorridorCenter(Vector3 currentPos)
    {
        RaycastHit hitL, hitR, hitF, hitB;
        float rayDist = 10f;

        bool wLeft = Physics.Raycast(currentPos, Vector3.left, out hitL, rayDist, wallLayer);
        bool wRight = Physics.Raycast(currentPos, Vector3.right, out hitR, rayDist, wallLayer);
        bool wFwd = Physics.Raycast(currentPos, Vector3.forward, out hitF, rayDist, wallLayer);
        bool wBack = Physics.Raycast(currentPos, Vector3.back, out hitB, rayDist, wallLayer);

        float cx = currentPos.x;
        float cz = currentPos.z;

        // Lógica de pasillos: centra en el eje donde hay paredes
        if (wLeft && wRight) cx = (hitL.point.x + hitR.point.x) / 2f;
        if (wFwd && wBack) cz = (hitF.point.z + hitB.point.z) / 2f;

        return new Vector3(cx, currentPos.y, cz);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isFlying = false;
    }
}