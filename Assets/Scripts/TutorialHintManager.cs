using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialHintManager : MonoBehaviour
{
    public static TutorialHintManager Instance;

    [Header("Canvases guía")]
    public GameObject moverUI;
    public GameObject rotarUI;
    public GameObject agarrarUI;
    public GameObject menuUI;
    public GameObject vistaSuperiorUI;
    public GameObject confirmarUI;

    [Header("Tiempo")]
    public float duracionUI = 3f;

    private bool tutorialActivo = false;

    private bool movimientoMostrado = false;
    private bool rotacionMostrada = false;
    private bool agarreMostrado = false;
    private bool menuMostrado = false;
    private bool vistaMostrada = false;
    private bool confirmarMostrado = false;

    private void Awake()
    {
        Instance = this;
        tutorialActivo = false;

        Debug.Log("TutorialHintManager iniciado - BLOQUEADO");
    }

    public void ActivarTutorialContextual()
    {
        tutorialActivo = true;
        Debug.Log("Tutorial contextual ACTIVADO");
    }

    void Update()
    {
        if (!tutorialActivo) return;

        DetectarMovimiento();
        DetectarRotacion();
        DetectarMenu();
        DetectarVistaSuperior();
        DetectarConfirmacion();
    }

    bool BotonAVR()
    {
        var rightHand =
            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonA;

        return rightHand.TryGetFeatureValue(
            UnityEngine.XR.CommonUsages.primaryButton,
            out botonA
        ) && botonA;
    }

    bool BotonBVR()
    {
        var rightHand =
            UnityEngine.XR.InputDevices.GetDeviceAtXRNode(UnityEngine.XR.XRNode.RightHand);

        if (!rightHand.isValid)
            return false;

        bool botonB;

        return rightHand.TryGetFeatureValue(
            UnityEngine.XR.CommonUsages.secondaryButton,
            out botonB
        ) && botonB;
    }

    // DETECCIONES
    void DetectarMovimiento()
    {
        if (movimientoMostrado) return;

        bool teclaW =
            Keyboard.current != null &&
            Keyboard.current.wKey.wasPressedThisFrame;

        if (teclaW)
        {
            Debug.Log("MOVIMIENTO detectado");
            MostrarUI(moverUI);
            movimientoMostrado = true;
        }
    }

    void DetectarRotacion()
    {
        if (rotacionMostrada) return;

        bool teclaD =
            Keyboard.current != null &&
            Keyboard.current.dKey.wasPressedThisFrame;

        if (teclaD)
        {
            Debug.Log("ROTACIÓN detectada");
            MostrarUI(rotarUI);
            rotacionMostrada = true;
        }
    }

    void DetectarMenu()
    {
        if (menuMostrado) return;

        bool teclaB =
            Keyboard.current != null &&
            Keyboard.current.bKey.wasPressedThisFrame;

        if (teclaB || BotonBVR())
        {
            Debug.Log("MENÚ detectado");
            MostrarUI(menuUI);
            menuMostrado = true;
        }
    }

    void DetectarVistaSuperior()
    {
        if (vistaMostrada) return;

        bool teclaX =
            Keyboard.current != null &&
            Keyboard.current.xKey.wasPressedThisFrame;

        if (teclaX)
        {
            Debug.Log("VISTA SUPERIOR detectada");
            MostrarUI(vistaSuperiorUI);
            vistaMostrada = true;
        }
    }

    void DetectarConfirmacion()
    {
        if (confirmarMostrado) return;

        bool teclaA =
            Keyboard.current != null &&
            Keyboard.current.aKey.wasPressedThisFrame;

        if (teclaA || BotonAVR())
        {
            Debug.Log("CONFIRMAR detectado");
            MostrarUI(confirmarUI);
            confirmarMostrado = true;
        }
    }

    public void DetectarAgarre()
    {
        if (agarreMostrado) return;

        Debug.Log("AGARRE detectado");
        MostrarUI(agarrarUI);
        agarreMostrado = true;
    }

    // UI
    void MostrarUI(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Panel no asignado");
            return;
        }

        StartCoroutine(MostrarTemporal(panel));
    }

    IEnumerator MostrarTemporal(GameObject panel)
    {
        panel.SetActive(true);

        Debug.Log("Mostrando: " + panel.name);

        yield return new WaitForSeconds(duracionUI);

        panel.SetActive(false);

        Debug.Log("Ocultando: " + panel.name);
    }
}