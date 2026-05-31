using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TimerTrigger : MonoBehaviour
{
    [Header("UI")]
    public UIManagerVR uiManager;

    [Header("Tiempo")]
    public float tiempoInicial = 120f;

    [Header("FMOD - Música 2")]
    [SerializeField] private EventReference musicaTiempoEvent;

    private EventInstance musicaTiempoInstance;

    private float tiempoRestante;
    private bool timerActivo = false;
    private bool musicaIniciada = false;
    private bool pausado = false;

    private void Start()
    {
        tiempoRestante = tiempoInicial;
        musicaTiempoInstance = RuntimeManager.CreateInstance(musicaTiempoEvent);
    }

    private void Update()
    {
        if (!timerActivo || pausado) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            timerActivo = false;

            DetenerMusicaTiempo();

            uiManager.MostrarDerrota();
        }

        uiManager.UpdateTimer(tiempoRestante);
    }

    public void IniciarTemporizador()
    {
        if (timerActivo) return;

        timerActivo = true;

        Debug.Log("TEMPORIZADOR INICIADO DESDE FUNCIÓN");

        IniciarMusicaTiempo();
    }

    private void IniciarMusicaTiempo()
    {
        if (musicaIniciada) return;

        musicaIniciada = true;

        musicaTiempoInstance.start();

        Debug.Log("MÚSICA 2 INICIADA");
    }

    private void DetenerMusicaTiempo()
    {
        if (!musicaIniciada) return;

        musicaIniciada = false;

        musicaTiempoInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        Debug.Log("MÚSICA 2 DETENIDA");
    }

    public void PausarTemporizador()
    {
        pausado = true;
    }

    public void ReanudarTemporizador()
    {
        pausado = false;
    }

    private void OnDestroy()
    {
        musicaTiempoInstance.release();
    }
}