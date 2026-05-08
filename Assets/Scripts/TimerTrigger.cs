using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    public UIManagerVR uiManager;
    public float tiempoInicial = 120f; // 2 minutos

    private float tiempoRestante;
    private bool timerActivo = false;

    private void Start()
    {
        tiempoRestante = tiempoInicial;
    }

    private void Update()
    {
        if (!timerActivo) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante < 0)
        {
            tiempoRestante = 0;
            timerActivo = false;

            // Aquí decides qué pasa al perder
            uiManager.MostrarDerrota();
        }

        uiManager.UpdateTimer(tiempoRestante);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ajusta esto según tu XR Rig
        if (other.CompareTag("MainCamera"))
        {
            timerActivo = true;
        }
    }
}