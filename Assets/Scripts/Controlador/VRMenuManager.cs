using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

using XRInputDevice = UnityEngine.XR.InputDevice;
using XRNode = UnityEngine.XR.XRNode;
using XRCommonUsages = UnityEngine.XR.CommonUsages;

public class VRMenuManager : MonoBehaviour
{
    [Header("Movimiento")]
    public CharacterController characterController;
    public MonoBehaviour movimientoScript;

    [Header("Panels")]
    public GameObject panelManual;
    public GameObject radialMenu;
    public GameObject panelSonido;
    public GameObject panelSalir;
    public GameObject fondoOscuro;

    [Header("Fade")]
    public CanvasGroup fadeMenu;
    public float duracionFade = 0.4f;

    [Header("Player")]
    public Transform playerCamera;

    [Header("Ray")]
    public GameObject menuRayInteractor;

    private bool menuAbierto;
    private bool animando;

    // toggle flanco
    private bool botonPrev;

    void Update()
    {
        ActualizarFondoOscuro();

        if (!animando && DetectarToggleMenu())
            ToggleMenu();
    }

    // ================= INPUT TOGGLE =================

    bool DetectarToggleMenu()
    {
        bool teclado = Keyboard.current != null &&
                       Keyboard.current.bKey.wasPressedThisFrame;

        return teclado || BotonBVRFlanco();
    }

    bool BotonBVRFlanco()
    {
        XRInputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool pressed;
        bool current =
            right.isValid &&
            right.TryGetFeatureValue(XRCommonUsages.secondaryButton, out pressed) &&
            pressed;

        bool flanco = current && !botonPrev;
        botonPrev = current;

        return flanco;
    }

    // ================= MENU CORE =================

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

        fondoOscuro?.SetActive(true);
        radialMenu.SetActive(true);
        menuRayInteractor.SetActive(true);

        PosicionarMenu();

        yield return null; 

        Time.timeScale = 0f;
        movimientoScript.enabled = false;
        characterController.enabled = false;

        yield return Fade(0f, 1f);

        animando = false;
    }

    IEnumerator CerrarMenu()
    {
        animando = true;

        yield return Fade(1f, 0f);

        OcultarTodo();

        Time.timeScale = 1f;
        movimientoScript.enabled = true;
        characterController.enabled = true;

        menuRayInteractor.SetActive(false);
        fondoOscuro?.SetActive(false);

        animando = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        fadeMenu.alpha = from;

        while (t < duracionFade)
        {
            t += Time.unscaledDeltaTime;
            fadeMenu.alpha = Mathf.Lerp(from, to, t / duracionFade);
            yield return null;
        }

        fadeMenu.alpha = to;
    }

    // ================= INPUT CONFIRM (A BUTTON + T) =================

    public bool ConfirmacionDown()
    {
        bool teclado = Keyboard.current != null &&
                       Keyboard.current.tKey.wasPressedThisFrame;

        XRInputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool pressed;
        bool vr =
            right.isValid &&
            right.TryGetFeatureValue(XRCommonUsages.primaryButton, out pressed) &&
            pressed;

        return teclado || vr;
    }

    // ================= UI =================

    public void MostrarRadial()
    {
        OcultarTodo();
        radialMenu.SetActive(true);
        PosicionarMenu();
    }

    public void MostrarSonido()
    {
        OcultarTodo();
        panelSonido.SetActive(true);
        PosicionarPanel(panelSonido);
    }

    public void MostrarManual()
    {
        OcultarTodo();
        panelManual.SetActive(true);
        PosicionarPanel(panelManual);
    }

    public void MostrarSalirConfirmacion()
    {
        OcultarTodo();
        panelSalir.SetActive(true);
        PosicionarPanel(panelSalir);
    }

    public void CancelarSalir()
    {
        MostrarRadial();
    }

    public void ConfirmarSalir()
    {
        Application.Quit();
    }

    public void ReanudarJuego()
    {
        StartCoroutine(CerrarMenu());
    }

    public void PausarJuego()
    {
        ToggleMenu();
    }

    // ================= POSITION =================

    void PosicionarMenu()
    {
        Vector3 pos = playerCamera.position + playerCamera.forward * 1.5f;
        pos.y -= 0.2f;

        radialMenu.transform.position = pos;
        radialMenu.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    void PosicionarPanel(GameObject panel)
    {
        Vector3 pos = playerCamera.position + playerCamera.forward * 1.5f;
        pos.y -= 0.2f;

        panel.transform.position = pos;
        panel.transform.rotation =
            Quaternion.LookRotation(playerCamera.forward);
    }

    // ================= BACKGROUND =================

    void ActualizarFondoOscuro()
    {
        if (!menuAbierto || fondoOscuro == null) return;

        fondoOscuro.transform.position =
            playerCamera.position + playerCamera.forward * 0.3f;

        fondoOscuro.transform.rotation = playerCamera.rotation;
    }

    // ================= UTILS =================

    void OcultarTodo()
    {
        radialMenu.SetActive(false);
        panelSonido.SetActive(false);
        panelManual.SetActive(false);
        panelSalir.SetActive(false);
    }
}