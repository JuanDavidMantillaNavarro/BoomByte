using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class VRMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject radialMenu;
    public GameObject panelSonido;
    public GameObject panelSalir;
    public Transform puntoInicio;

    [Header("Fade")]
    public CanvasGroup fadeMenu;
    public float duracionFade = 0.4f;

    [Header("Player")]
    public Transform playerCamera;
    public Transform jugador;

    [Header("Ray Menu")]
    public GameObject menuRayInteractor;

    [Header("Menu Position")]
    public float distanceFromCamera = 1.5f;
    public float heightOffset = -0.2f;

    private bool menuAbierto = false;
    private bool animando = false;
    private bool avisoControlMostrado = false;
    private bool botonBPresionadoAnterior = false;

    void Start()
    {
        Debug.Log("VRMenuManager iniciado");
        OcultarTodoInstantaneo();
    }

    void Update()
    {
        DetectarInputMenu();
    }

    // ================= INPUT =================

    bool BotonBVR()
    {
        XRInputDevice rightHand =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (!rightHand.isValid)
        {
            if (!avisoControlMostrado)
            {
                Debug.LogWarning("Control derecho no detectado (normal en simulador PC)");
                avisoControlMostrado = true;
            }
            return false;
        }

        avisoControlMostrado = false;
        bool botonB = false;

        return rightHand.TryGetFeatureValue(
            XRCommonUsages.secondaryButton,
            out botonB
        ) && botonB;
    }

    bool BotonXVR()
    {
        XRInputDevice leftHand =
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (!leftHand.isValid)
            return false;

        bool botonX = false;

        return leftHand.TryGetFeatureValue(
            XRCommonUsages.primaryButton,
            out botonX
        ) && botonX;
    }

    void DetectarInputMenu()
    {
        if (animando) return;

        // Toggle menú (Teclado B o Botón B VR con detección de flanco)
        bool tecladoB =
            Keyboard.current != null &&
            Keyboard.current.bKey.wasPressedThisFrame;

        bool botonActual = BotonBVR();
        bool botonVRFrame = botonActual && !botonBPresionadoAnterior;
        botonBPresionadoAnterior = botonActual;

        if (tecladoB || botonVRFrame)
        {
            ToggleMenu();
        }

        // Habilidad de cámara (Teclado M o Botón X VR)
        bool tecladoM = Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame;

        if (tecladoM || BotonXVR())
        {
            if (GameController.Instance != null)
                GameController.Instance.ActivateCameraView();
        }
    }

    // ================= MENU =================

    public void ToggleMenu()
    {
        if (animando) return;

        menuAbierto = !menuAbierto;

        if (menuAbierto)
            StartCoroutine(AbrirMenu());
        else
            StartCoroutine(CerrarMenu());
    }

    IEnumerator AbrirMenu()
    {
        animando = true;

        MostrarRadial();
        PosicionarMenuFrenteJugador();

        if (GameController.Instance != null)
            GameController.Instance.isPaused = true;

        Time.timeScale = 0f;

        if (menuRayInteractor != null)
            menuRayInteractor.SetActive(true);

        if (radialMenu != null)
            radialMenu.SetActive(true);

        if (fadeMenu != null)
        {
            fadeMenu.alpha = 0f;
            float tiempo = 0f;

            while (tiempo < duracionFade)
            {
                tiempo += Time.unscaledDeltaTime;
                fadeMenu.alpha = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
                yield return null;
            }
            fadeMenu.alpha = 1f;
        }

        animando = false;
        Debug.Log("MENÚ ABIERTO");
    }

    IEnumerator CerrarMenu()
    {
        animando = true;

        if (fadeMenu != null)
        {
            float tiempo = 0f;
            while (tiempo < duracionFade)
            {
                tiempo += Time.unscaledDeltaTime;
                fadeMenu.alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
                yield return null;
            }
            fadeMenu.alpha = 0f;
        }

        OcultarTodoInstantaneo();

        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        Time.timeScale = 1f;

        if (menuRayInteractor != null)
            menuRayInteractor.SetActive(false);

        animando = false;
        Debug.Log("MENÚ CERRADO");
    }

    // ================= POSICIONAMIENTO =================

    void PosicionarMenuFrenteJugador()
    {
        if (playerCamera == null || radialMenu == null) return;

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        radialMenu.transform.position = targetPosition;
        radialMenu.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    void PosicionarPanel(GameObject panel)
    {
        if (playerCamera == null || panel == null) return;

        Vector3 targetPosition =
            playerCamera.position +
            playerCamera.forward * distanceFromCamera;

        targetPosition.y += heightOffset;

        panel.transform.position = targetPosition;
        panel.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    // ================= UI =================

    public void MostrarRadial()
    {
        if (radialMenu != null) radialMenu.SetActive(true);
        if (panelSonido != null) panelSonido.SetActive(false);
        if (panelSalir != null) panelSalir.SetActive(false);

        PosicionarPanel(radialMenu);
    }

    public void MostrarSonido()
    {
        if (radialMenu != null) radialMenu.SetActive(false);
        if (panelSonido != null) panelSonido.SetActive(true);
        if (panelSalir != null) panelSalir.SetActive(false);

        PosicionarPanel(panelSonido);
    }

    public void MostrarSalirConfirmacion()
    {
        if (radialMenu != null) radialMenu.SetActive(false);
        if (panelSonido != null) panelSonido.SetActive(false);
        if (panelSalir != null) panelSalir.SetActive(true);

        PosicionarPanel(panelSalir);
    }

    // ================= BOTONES =================

    public void ReanudarJuego()
    {
        Debug.Log("Reiniciando juego");

        // Intenta usar la referencia directa de la rama, si no, busca por Tag
        Transform targetJugador = jugador;
        if (targetJugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) targetJugador = playerObj.transform;
        }

        if (targetJugador != null && puntoInicio != null)
        {
            targetJugador.position = puntoInicio.position;
            targetJugador.rotation = puntoInicio.rotation;
        }

        if (GameController.Instance != null)
            GameController.Instance.ReiniciarEstado();

        menuAbierto = false;
        StartCoroutine(CerrarMenu());
    }

    public void PausarJuego()
    {
        ToggleMenu();
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    public void CancelarSalir()
    {
        MostrarRadial();
    }

    // ================= UTILS =================

    void OcultarTodoInstantaneo()
    {
        if (radialMenu != null) radialMenu.SetActive(false);
        if (panelSonido != null) panelSonido.SetActive(false);
        if (panelSalir != null) panelSalir.SetActive(false);
        
        if (fadeMenu != null)
            fadeMenu.alpha = 0f;
    }
}