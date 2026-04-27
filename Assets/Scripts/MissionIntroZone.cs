using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class MissionIntroZone : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelB25;
    public GameObject panelObjetivo;
    public GameObject panelContinuar;

    private bool introduccionActiva = false;
    private bool yaSeMostro = false;

    private Collider zonaCollider;

    void Start()
    {
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
            Keyboard.current.vKey.wasPressedThisFrame;

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

        // PAUSA ABSOLUTA
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

        // REANUDAR TODO
        Time.timeScale = 1f;

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        // DESACTIVAR ZONA
        if (zonaCollider != null)
            zonaCollider.enabled = false;

        Debug.Log("FASE REANUDADA");
    }

    void MostrarPanels()
    {
        if (panelB25 != null) panelB25.SetActive(true);
        if (panelObjetivo != null) panelObjetivo.SetActive(true);
        if (panelContinuar != null) panelContinuar.SetActive(true);
    }

    void OcultarPanels()
    {
        if (panelB25 != null) panelB25.SetActive(false);
        if (panelObjetivo != null) panelObjetivo.SetActive(false);
        if (panelContinuar != null) panelContinuar.SetActive(false);
    }
}