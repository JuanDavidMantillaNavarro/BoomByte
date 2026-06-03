using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioManagerFMODVR : MonoBehaviour
{
    [Header("Sliders")]
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderEfectos;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        sliderGeneral.onValueChanged.AddListener(SetGeneral);
        sliderMusica.onValueChanged.AddListener(SetMusica);
        sliderEfectos.onValueChanged.AddListener(SetEfectos);

        sliderGeneral.value =
            PlayerPrefs.GetFloat("MasterVol", 1f);

        sliderMusica.value =
            PlayerPrefs.GetFloat("MusicVol", 1f);

        sliderEfectos.value =
            PlayerPrefs.GetFloat("SFXVol", 1f);

        SetGeneral(sliderGeneral.value);
        SetMusica(sliderMusica.value);
        SetEfectos(sliderEfectos.value);
    }

    public void SetGeneral(float value)
    {
        float volumen = Mathf.Lerp(0.05f, 1f, value);

        masterBus.setVolume(volumen);

        PlayerPrefs.SetFloat("MasterVol", value);
    }

    public void SetMusica(float value)
    {
        musicBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void SetEfectos(float value)
    {
        sfxBus.setVolume(value);
        PlayerPrefs.SetFloat("SFXVol", value);
    }
}