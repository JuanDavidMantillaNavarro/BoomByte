using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class MissionIntroZone : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mensajePatroclo;
    public GameObject panelObjetivo;
    public GameObject panelContinuar;
    public GameObject imagenPatroclo;

    private bool introduccionActiva = false;
    private bool yaSeMostro = false;

    private Collider zonaCollider;

    void Start()
    {
        // BUSCAR DESDE EL CANVAS PADRE
        GameObject canvasPadre = GameObject.Find("CanvaPatrocloInfo");

        if (canvasPadre != null)
        {
            mensajePatroclo = canvasPadre.transform.Find("mensajePatroclo")?.gameObject;
            panelObjetivo = canvasPadre.transform.Find("panelObjetivo")?.gameObject;
            panelContinuar = canvasPadre.transform.Find("panelContinuar")?.gameObject;
            imagenPatroclo = canvasPadre.transform.Find("imagenPatroclo")?.gameObject;
        }
        else
        {
            Debug.LogError("No se encontró CanvaPatrocloInfo");
        }

        Debug.Log("mensajePatroclo: " + mensajePatroclo);
        Debug.Log("panelObjetivo: " + panelObjetivo);
        Debug.Log("panelContinuar: " + panelContinuar);
        Debug.Log("imagenPatroclo: " + imagenPatroclo);

        OcultarPanels();

        zonaCollider = GetComponent<Collider>();

        Debug.Log("Sistema intro listo");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (yaSeMostro) return;

        if (other.CompareTag("Player"))
        {
            MostrarInicioFase();
        }
    }

    void Update()
    {
        if (!introduccionActiva) return;

        bool teclaCerrar =
            Keyboard.current != null &&
            Keyboard.current.pKey.wasPressedThisFrame;

        bool botonA = BotonAVR();

        if (teclaCerrar || botonA)
        {
            IniciarFase();
        }
    }

    bool BotonAVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonA = false;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.primaryButton,
            out botonA
        ) && botonA;
    }

    void MostrarInicioFase()
    {
        yaSeMostro = true;
        introduccionActiva = true;

        Cursor.lockState = CursorLockMode.None;

        MostrarPanels();

        Time.timeScale = 0f;

        if (GameController.Instance != null)
            GameController.Instance.isPaused = true;

        Debug.Log("INTRO PAUSADA");
    }

    void IniciarFase()
    {
        introduccionActiva = false;

        Cursor.lockState = CursorLockMode.Locked;

        OcultarPanels();

        // SOLO REANUDAR, NO MOVER JUGADOR
        Time.timeScale = 1f;

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        if (zonaCollider != null)
            zonaCollider.enabled = false;

        Debug.Log("FASE REANUDADA - jugador permanece en su posición");
    }

    void MostrarPanels()
    {
        if (mensajePatroclo != null) mensajePatroclo.SetActive(true);
        if (panelObjetivo != null) panelObjetivo.SetActive(true);
        if (panelContinuar != null) panelContinuar.SetActive(true);
        if (imagenPatroclo != null) imagenPatroclo.SetActive(true);
    }

    void OcultarPanels()
    {
        if (mensajePatroclo != null) mensajePatroclo.SetActive(false);
        if (panelObjetivo != null) panelObjetivo.SetActive(false);
        if (panelContinuar != null) panelContinuar.SetActive(false);
        if (imagenPatroclo != null) imagenPatroclo.SetActive(false);
    }
}