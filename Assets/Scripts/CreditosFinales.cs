using System.Collections;
using UnityEngine;
using TMPro;

public class CreditosFinales : MonoBehaviour
{
    [Header("Canvas")]
    public CanvasGroup fadeCanvas;

    [Header("Texto")]
    public TMP_Text textoCreditos;

    [Header("Imagen equipo")]
    public GameObject imagenEquipo;

    [Header("Botón salir")]
    public GameObject botonSalir;

    [Header("Configuración")]
    public float tiempoPorPagina = 5f;
    public float duracionFade = 1f;
    public float velocidadEscritura = 0.02f;

    private string[] paginas =
    {
@"UNIVERSIDAD MILITAR NUEVA GRANADA

Ingeniería en Multimedia

Proyecto desarrollado para las asignaturas de: 

Aplicaciones 3D
Integración Multimedia",

@"Integrantes

Juan David Mantilla Navarro
Emily Mora Llanos
Nicol Stephany Valbuena Bolaños
Nikol Sofia Forero Borja
Isabella Gallego Tachack
Anderson Muñoz Monsalve",

@"Modelado 3D

Isabella Gallego Tachack

Modelos de profesores
Modelos de los laberintos
Elementos de la universidad
Modelos referenciados de la sede Calle 100",

@"Interfaces gráficas

Emily Mora Llanos
Nikol Sofia Forero Borja",

@"Material gráfico

Nicol Stephany Valbuena Bolaños
Pósters
Ilustraciones",

@"Animaciones e ilustraciones

Anderson Muñoz Monsalve",

@"Programación y desarrollo

Juan David Mantilla Navarro
Emily Mora Llanos

Lógica principal
Mecánicas del videojuego
Sistemas de interacción",

@"Referencias

Bomberman
Cuphead",

@"25 años de Ingeniería en Multimedia
Gracias por jugar"
    };

    private void Start()
    {
        gameObject.SetActive(false);

        if (botonSalir != null)
            botonSalir.SetActive(false);

        if (imagenEquipo != null)
            imagenEquipo.SetActive(false);

        if (textoCreditos != null)
            textoCreditos.text = "";

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;
    }

    public void IniciarCreditos()
    {
        gameObject.SetActive(true);

        if (imagenEquipo != null)
            imagenEquipo.SetActive(true);

        StartCoroutine(SecuenciaCreditos());
    }

    IEnumerator SecuenciaCreditos()
    {
        for (int i = 0; i < paginas.Length; i++)
        {
            bool esUltimaPagina = (i == paginas.Length - 1);

            if (esUltimaPagina)
            {
                yield return StartCoroutine(
                    MostrarPaginaFinal(paginas[i])
                );
            }
            else
            {
                yield return StartCoroutine(
                    MostrarPagina(paginas[i])
                );
            }
        }
    }

    IEnumerator MostrarPagina(string pagina)
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    tiempo / duracionFade
                );

            yield return null;
        }

        fadeCanvas.alpha = 1f;

        textoCreditos.text = "";

        foreach (char letra in pagina)
        {
            textoCreditos.text += letra;

            yield return new WaitForSecondsRealtime(
                velocidadEscritura
            );
        }

        yield return new WaitForSecondsRealtime(
            tiempoPorPagina
        );

        tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    tiempo / duracionFade
                );

            yield return null;
        }

        fadeCanvas.alpha = 0f;
    }

    IEnumerator MostrarPaginaFinal(string pagina)
    {
        float tiempo = 0f;

        while (tiempo < duracionFade)
        {
            tiempo += Time.unscaledDeltaTime;

            fadeCanvas.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    tiempo / duracionFade
                );

            yield return null;
        }

        fadeCanvas.alpha = 1f;

        textoCreditos.text = "";

        foreach (char letra in pagina)
        {
            textoCreditos.text += letra;

            yield return new WaitForSecondsRealtime(
                velocidadEscritura
            );
        }

        // MOSTRAR BOTÓN DE SALIR
        if (botonSalir != null)
            botonSalir.SetActive(true);

        // NO HACER FADE OUT
        // LA PANTALLA QUEDA FIJA
        yield break;
    }
}