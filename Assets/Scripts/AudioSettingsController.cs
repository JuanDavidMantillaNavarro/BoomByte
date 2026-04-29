using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Sliders")]
    public Slider volumenGeneralSlider;
    public Slider musicaSlider;
    public Slider efectosSlider;

    [Header("Audio Sources")]
    public AudioSource musicaSource;
    public AudioSource[] efectosSources;

    void Start()
    {
        volumenGeneralSlider.value = AudioListener.volume;

        if (musicaSource != null)
            musicaSlider.value = musicaSource.volume;

        if (efectosSources.Length > 0)
            efectosSlider.value = efectosSources[0].volume;

        volumenGeneralSlider.onValueChanged.AddListener(CambiarVolumenGeneral);
        musicaSlider.onValueChanged.AddListener(CambiarMusica);
        efectosSlider.onValueChanged.AddListener(CambiarEfectos);
    }

    public void CambiarVolumenGeneral(float valor)
    {
        AudioListener.volume = valor;
        Debug.Log("Volumen general: " + valor);
    }

    public void CambiarMusica(float valor)
    {
        if (musicaSource != null)
        {
            musicaSource.volume = valor;
        }

        Debug.Log("Volumen m�sica: " + valor);
    }

    public void CambiarEfectos(float valor)
    {
        foreach (AudioSource efecto in efectosSources)
        {
            if (efecto != null)
            {
                efecto.volume = valor;
            }
        }

        Debug.Log("Volumen efectos: " + valor);
    }
}
