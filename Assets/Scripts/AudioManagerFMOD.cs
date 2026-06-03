using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManagerFMOD : MonoBehaviour
{
    public static AudioManagerFMOD Instance;

    [Header("FMOD - Música inicial")]
    [SerializeField] private EventReference musicaAmbienteEvent;

    [Header("FMOD - Bus")]
    [SerializeField] private string sfxBusPath = "bus:/SFX";

    private EventInstance musicaActualInstance;
    private Bus sfxBus;

    public float volumenMusicaActual = 1f;
    public bool musicaActiva = true;
    public bool efectosActivos = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        musicaActualInstance = RuntimeManager.CreateInstance(musicaAmbienteEvent);
        musicaActualInstance.start();
        musicaActualInstance.setVolume(volumenMusicaActual);

        sfxBus = RuntimeManager.GetBus(sfxBusPath);
        sfxBus.setVolume(1f);
    }

    public void CambiarMusicaZona(EventReference nuevaMusica)
    {
        musicaActualInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicaActualInstance.release();

        musicaActualInstance = RuntimeManager.CreateInstance(nuevaMusica);
        musicaActualInstance.start();
        musicaActualInstance.setVolume(musicaActiva ? volumenMusicaActual : 0f);

        Debug.Log("Música cambiada por zona");
    }

    public void CambiarVolumenMusica(float valor)
    {
        volumenMusicaActual = valor;

        if (musicaActiva)
            musicaActualInstance.setVolume(valor);
    }

    public void ActivarMusicaa(bool activa)
    {
        musicaActiva = activa;
        musicaActualInstance.setVolume(activa ? volumenMusicaActual : 0f);
    }

    public void ActivarEfectos(bool activos)
    {
        efectosActivos = activos;
        sfxBus.setVolume(activos ? 1f : 0f);
    }

    private void OnDestroy()
    {
        musicaActualInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicaActualInstance.release();
    }
}