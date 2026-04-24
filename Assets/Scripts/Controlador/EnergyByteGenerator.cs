using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;

public class EnergyByteGenerator : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject energyBytePrefab;
    public GameObject Cam;

    [Header("Referencias")]
    public XRInteractionManager interactionManager;

    // Lista para ver que manos están dentro del generador
    private List<IXRSelectInteractor> interactorsInZone = new List<IXRSelectInteractor>();

    private void OnTriggerEnter(Collider other)
    {
        // Se verifica si es un interactor (es decir la mano)
        var interactor = other.GetComponentInParent<IXRSelectInteractor>();
        
        if (interactor != null && !interactorsInZone.Contains(interactor))
        {
            interactorsInZone.Add(interactor);
            // Evento de cuando el usuario presiona el botón de agarrar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactor = other.GetComponentInParent<IXRSelectInteractor>();
        if (interactor != null && interactorsInZone.Contains(interactor))
        {
            interactorsInZone.Remove(interactor);
        }
    }

    private void Update()
    {
        // Mano en generador
        foreach (var interactor in interactorsInZone)
        {
            // Verifica si la mano está intentando seleccionar (botón presionado de grab) y no tiene nada seleccionado
            if (IsPressingGrab(interactor) && !interactor.hasSelection)
            {
                CreateAndGrabBall(interactor);
            }
        }

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
        GameObject newBall = Instantiate(energyBytePrefab, Cam.transform.position, Quaternion.identity);
        GameController.Instance.RegisterBallSpawned();
        
        EnergyByte ballInteractable = newBall.GetComponent<EnergyByte>();
        Rigidbody ballRb = newBall.GetComponent<Rigidbody>();
        }
    }

    private bool IsPressingGrab(IXRSelectInteractor interactor)
    {
        // Detectar si el botón de grap está activo mientras está en la zona del generador.
        if (interactor is XRDirectInteractor direct)
        {
            return direct.isSelectActive;
        }
        return false;
    }

    private void CreateAndGrabBall(IXRSelectInteractor interactor)
    {
        // VALIDACIÓN DEL GAMECONTROLLER 
        if (!GameController.Instance.CanSpawnBall())
        return;
        
        // Crear la bola
        GameObject newBall = Instantiate(energyBytePrefab, interactor.transform.position, Quaternion.identity);
        GameController.Instance.RegisterBallSpawned();
        
        EnergyByte ballInteractable = newBall.GetComponent<EnergyByte>();
        Rigidbody ballRb = newBall.GetComponent<Rigidbody>();

        if (ballInteractable != null && ballRb != null)
        {
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            ballRb.ResetInertiaTensor();
            // Forzar el agarre a la bola
            interactionManager.SelectEnter(interactor, (IXRSelectInteractable)ballInteractable);
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            ballRb.ResetInertiaTensor();
        }
    }
}