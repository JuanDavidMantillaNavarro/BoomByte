using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EnergyUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelRecargando;
    public GameObject panelLista;
    public TextMeshProUGUI textoPorcentaje;

    [Header("Modelo (para tiempo real)")]
    public EnergyByteModel modeloEnergy;

    [Header("VR Posición")]
    public Transform playerCamera;
    public float distancia = 0.5f;
    public float altura = -0.6f;

    private float tiempoActual = 0f;
    private float tiempoMax = 2f;
    private bool recargando = false;

    void Start()
    {
        if (modeloEnergy != null)
            tiempoMax = modeloEnergy.delayBeforeExplode;

        panelRecargando.SetActive(false);
        panelLista.SetActive(true);
    }

    void Update()
    {
        ActualizarRecarga();
        ActualizarPosicionVR();
        DebugSimulador();
    }

    // LÓGICA RECARGA

    public void IniciarRecarga()
    {
        tiempoActual = 0f;
        recargando = true;

        panelRecargando.SetActive(true);
        panelLista.SetActive(false);
    }

    void ActualizarRecarga()
    {
        if (!recargando) return;

        tiempoActual += Time.deltaTime;

        float progreso = tiempoActual / tiempoMax;
        int porcentaje = Mathf.RoundToInt(progreso * 100);

        textoPorcentaje.text = porcentaje + "%";

        if (tiempoActual >= tiempoMax)
        {
            recargando = false;

            panelRecargando.SetActive(false);
            panelLista.SetActive(true);
        }
    }

    //POSICIÓN VR (cintura)

    void ActualizarPosicionVR()
    {
        if (playerCamera == null) return;

        Vector3 pos =
            playerCamera.position +
            playerCamera.forward * distancia;

        pos.y += altura;

        transform.position = pos;

        transform.rotation =
            Quaternion.LookRotation(transform.position - playerCamera.position);
    }

    //DEBUG SIMULADOR con TECLA R
    void DebugSimulador()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            IniciarRecarga();
        }
    }
}
