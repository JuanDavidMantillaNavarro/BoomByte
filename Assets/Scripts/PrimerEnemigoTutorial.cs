using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PrimerEnemigoTutorial : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Distancia")]
    public float distanciaActivacion = 2f;

    [Header("Movimiento Real")]
    public CharacterController characterController;
    public MonoBehaviour movimientoScript;

    [Header("UI")]
    public GameObject fondoNegro;
    public GameObject imagenPrimerEnemigo;
    public GameObject imagenEfecto;

    [Header("Tiempo")]
    public float duracionMensajeEnemigo = 6f;
    public float duracionMensajeEfecto = 4f;

    [Header("Movimiento VR")]
    public MonoBehaviour locomotionProvider;
    public MonoBehaviour turnProvider;

    private bool activado = false;

    void Start()
    {
        Debug.Log("[Tutorial] Start");

        // SOLO ocultar la interfaz al iniciar
        if (fondoNegro != null)
            fondoNegro.SetActive(false);

        if (imagenPrimerEnemigo != null)
            imagenPrimerEnemigo.SetActive(false);

        if (imagenEfecto != null)
            imagenEfecto.SetActive(false);
    }

    void Update()
    {
        if (activado)
            return;

        if (jugador == null)
            return;

        float distancia =
            Vector3.Distance(
                transform.position,
                jugador.position
            );

        if (distancia <= distanciaActivacion)
        {
            Debug.Log(
                "[Tutorial] Jugador cerca. Distancia = "
                + distancia
            );

            activado = true;

            StartCoroutine(
                SecuenciaTutorial()
            );
        }
    }

    IEnumerator SecuenciaTutorial()
    {
        Debug.Log("PASO 1");

        // Pausar timer del juego
        if (GameController.Instance != null)
        {
            GameController.Instance.isPaused = true;
            Debug.Log("GameController pausado");
        }

        // Congelar tiempo global
        Time.timeScale = 0f;

        Debug.Log("TimeScale = 0");

        // Bloquear locomoción XR
        if (locomotionProvider != null)
        {
            locomotionProvider.enabled = false;
            Debug.Log("Locomotion desactivada");
        }

        // Bloquear giro por joystick
        if (turnProvider != null)
        {
            turnProvider.enabled = false;
            Debug.Log("Turn Provider desactivado");
        }

        // Bloquear script principal de movimiento
        if (movimientoScript != null)
        {
            movimientoScript.enabled = false;
            Debug.Log("Movimiento Script desactivado");
        }

        // Bloquear Character Controller
        if (characterController != null)
        {
            characterController.enabled = false;
            Debug.Log("Character Controller desactivado");
        }

        // Mostrar fondo negro
        fondoNegro.SetActive(true);

        // Mostrar primer mensaje
        imagenPrimerEnemigo.SetActive(true);

        yield return new WaitForSecondsRealtime(
            duracionMensajeEnemigo
        );

        // Ocultar primer mensaje
        imagenPrimerEnemigo.SetActive(false);

        // Mostrar segundo mensaje
        imagenEfecto.SetActive(true);

        yield return new WaitForSecondsRealtime(
            duracionMensajeEfecto
        );

        // Ocultar segundo mensaje
        imagenEfecto.SetActive(false);

        // Ocultar fondo
        fondoNegro.SetActive(false);

        Debug.Log("Reactivando controles");

        // Reactivar locomoción
        if (locomotionProvider != null)
            locomotionProvider.enabled = true;

        // Reactivar giro
        if (turnProvider != null)
            turnProvider.enabled = true;

        // Reactivar movimiento
        if (movimientoScript != null)
            movimientoScript.enabled = true;

        // Reactivar CharacterController
        if (characterController != null)
            characterController.enabled = true;

        // Reanudar timer
        if (GameController.Instance != null)
            GameController.Instance.isPaused = false;

        // Reanudar tiempo global
        Time.timeScale = 1f;

        Debug.Log("PASO 10 - FIN");
    }
}