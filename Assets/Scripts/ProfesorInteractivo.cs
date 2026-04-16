using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class ProfesorInteractivo : MonoBehaviour
{
    public Transform playerCamera;
    public GameObject canvasProfesor;

    public float distanciaActivacion = 2f;
    public float duracionMaxima = 10f;

    private bool activo = false;
    private float tiempoInicio;

    void Start()
    {
        canvasProfesor.SetActive(false);
        Debug.Log("Script iniciado");
    }

    void Update()
    {
        if (playerCamera == null || canvasProfesor == null)
        {
            Debug.Log("Faltan referencias");
            return;
        }

        float distancia = Vector3.Distance(playerCamera.position, transform.position);

        if (distancia <= distanciaActivacion && !activo)
        {
            ActivarDialogo();
        }

        if (activo)
        {
            // cerrar con tecla T
            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {
                Debug.Log("Cierre manual con T");
                DesactivarDialogo();
            }

            // cerrar con botón A VR
            if (BotonAVR())
            {
                Debug.Log("Cierre con botón A (VR)");
                DesactivarDialogo();
            }

            // cierre automático real
            if (Time.time - tiempoInicio >= duracionMaxima)
            {
                Debug.Log("Se desactivó el diálogo por tiempo");
                DesactivarDialogo();
            }
        }
    }

    void ActivarDialogo()
    {
        activo = true;
        tiempoInicio = Time.time;

        Debug.Log("Se activó el diálogo");

        canvasProfesor.SetActive(true);

        // siempre delante del profesor
        Vector3 frente = transform.position + transform.forward * 1.0f;

        canvasProfesor.transform.position = frente;

        canvasProfesor.transform.LookAt(playerCamera);
        canvasProfesor.transform.Rotate(0, 180, 0);

        // tamaño visible
        canvasProfesor.transform.localScale = Vector3.one * 1f;
    }

    void DesactivarDialogo()
    {
        Debug.Log("Se desactivó el diálogo");

        activo = false;
        canvasProfesor.SetActive(false);
    }

    bool BotonAVR()
    {
        UnityEngine.XR.InputDevice rightHand =
            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonA = false;

        if (rightHand.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out botonA))
        {
            return botonA;
        }

        return false;
    }
}
