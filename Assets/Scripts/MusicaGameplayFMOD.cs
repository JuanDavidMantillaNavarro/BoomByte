using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicaGameplayFMOD : MonoBehaviour
{
    public static MusicaGameplayFMOD Instance;

    [Header("Música después de segunda pared")]
    public EventReference musicaDespuesSegundaPared;

    private EventInstance musicaInstance;

    private int paredesDestruidas = 0;
    private bool musicaIniciada = false;

    private void Awake()
    {
        Instance = this;
    }

    public void RegistrarParedDestruida()
    {
        paredesDestruidas++;

        Debug.Log("Paredes destruidas: " + paredesDestruidas);

        // 🔥 Cuando destruya la segunda pared
        if (paredesDestruidas >= 2 && !musicaIniciada)
        {
            musicaIniciada = true;

            musicaInstance = RuntimeManager.CreateInstance(musicaDespuesSegundaPared);
            musicaInstance.start();

            Debug.Log("MÚSICA 2 INICIADA");
        }
    }

    private void OnDestroy()
    {
        if (musicaInstance.isValid())
        {
            musicaInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicaInstance.release();
        }
    }
}