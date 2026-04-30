using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Sliders")]
    public Slider volumenGeneralSlider;
    public Slider musicaSlider;
    public Slider efectosSlider;

    void Start()
    {
        volumenGeneralSlider.value = AudioManagerFMOD.Instance.volumenMusicaActual;

        musicaSlider.minValue = 0f;
        musicaSlider.maxValue = 1f;
        musicaSlider.wholeNumbers = true;
        musicaSlider.value = AudioManagerFMOD.Instance.musicaActiva ? 1f : 0f;

        efectosSlider.minValue = 0f;
        efectosSlider.maxValue = 1f;
        efectosSlider.wholeNumbers = true;
        efectosSlider.value = AudioManagerFMOD.Instance.efectosActivos ? 1f : 0f;

        volumenGeneralSlider.onValueChanged.AddListener(AudioManagerFMOD.Instance.CambiarVolumenMusica);
        musicaSlider.onValueChanged.AddListener(valor => AudioManagerFMOD.Instance.ActivarMusica(valor >= 1f));
        efectosSlider.onValueChanged.AddListener(valor => AudioManagerFMOD.Instance.ActivarEfectos(valor >= 1f));
    }
}