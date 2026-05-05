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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicaAmbienteInstance = RuntimeManager.CreateInstance(musicaAmbienteEvent);
        musicaAmbienteInstance.start();

        sfxBus = RuntimeManager.GetBus(sfxBusPath);
        sfxBus.setVolume(1f);
    }

    public void CambiarVolumenMusica(float valor)
    {
        volumenMusicaActual = valor;

        if (musicaActiva)
            musicaAmbienteInstance.setVolume(valor);
    }

    public void ActivarMusica(bool activa)
    {
        musicaActiva = activa;
        musicaAmbienteInstance.setVolume(activa ? volumenMusicaActual : 0f);
    }

    public void ActivarEfectos(bool activos)
    {
        efectosActivos = activos;
        sfxBus.setVolume(activos ? 1f : 0f);
    }

    private void OnDestroy()
    {
        musicaAmbienteInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicaAmbienteInstance.release();
    }
    public void CambiarEfectosSlider(float valor)
{
    bool activos = valor >= 1f;

    FMOD.RESULT result = sfxBus.setVolume(activos ? 1f : 0f);
    Debug.Log("SFX activo: " + activos + " / Resultado FMOD: " + result);
}
}