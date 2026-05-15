using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManagerFMOD : MonoBehaviour
{
    public static AudioManagerFMOD Instance;

    [Header("FMOD - Música ambiente")]
    [SerializeField] private EventReference musicaAmbienteEvent;

    [Header("FMOD - Bus")]
    [SerializeField] private string sfxBusPath = "bus:/SFX";

    private EventInstance musicaAmbienteInstance;
    private Bus sfxBus;

    public float volumenMusicaActual = 1f;
    public bool musicaActiva = true;
    public bool efectosActivos = true;

    private bool musicaDetenida = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        musicaAmbienteInstance =
            RuntimeManager.CreateInstance(musicaAmbienteEvent);

        musicaAmbienteInstance.start();
        musicaAmbienteInstance.setVolume(volumenMusicaActual);

        sfxBus = RuntimeManager.GetBus(sfxBusPath);
        sfxBus.setVolume(1f);

        Debug.Log("AudioManager iniciado");
    }

    public void CambiarVolumenMusica(float valor)
    {
        volumenMusicaActual = valor;

        if (!musicaDetenida && musicaActiva)
            musicaAmbienteInstance.setVolume(valor);
    }

    public void ActivarMusicaa(bool activa)
    {
        musicaActiva = activa;

        if (musicaDetenida)
            return;

        musicaAmbienteInstance.setVolume(activa ? volumenMusicaActual : 0f);

        Debug.Log("Música activa: " + activa);
    }

    public void DetenerMusicaAmbiente()
    {
        if (musicaDetenida) return;

        musicaDetenida = true;
        musicaActiva = false;

        musicaAmbienteInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        Debug.Log("Música ambiente detenida definitivamente");
    }

    public void ActivarEfectos(bool activos)
    {
        efectosActivos = activos;

        sfxBus.setVolume(activos ? 1f : 0f);

        Debug.Log("Efectos: " + activos);
    }

    private void OnDestroy()
    {
        musicaAmbienteInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicaAmbienteInstance.release();
    }
}