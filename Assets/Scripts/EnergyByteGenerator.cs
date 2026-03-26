using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class EnergyByteGenerator : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject energyBytePrefab;

    [Header("Referencias")]
    public XRInteractionManager interactionManager;

    // Lista para rastrear qué manos están dentro de la zona
    private List<IXRSelectInteractor> interactorsInZone = new List<IXRSelectInteractor>();

    private void OnTriggerEnter(Collider other)
    {
        // Al entrar, verificamos si es un interactor (mano)
        var interactor = other.GetComponentInParent<IXRSelectInteractor>();
        
        if (interactor != null && !interactorsInZone.Contains(interactor))
        {
            interactorsInZone.Add(interactor);
            // Nos suscribimos al evento de cuando el usuario presiona el botón de agarrar
            // Nota: En versiones nuevas de XRIT se usa selectEntered en el interactor
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
        // Revisamos si alguna mano en la zona está intentando agarrar y no tiene nada
        foreach (var interactor in interactorsInZone)
        {
            // Verificamos si el interactor está intentando seleccionar (botón presionado)
            // y si NO tiene ya algo seleccionado
            if (IsPressingGrab(interactor) && !interactor.hasSelection)
            {
                CreateAndGrabBall(interactor);
            }
        }
    }

    private bool IsPressingGrab(IXRSelectInteractor interactor)
    {
        // Esta es la forma compatible de detectar si el botón de agarre está activo
        // mientras está en la zona del trigger.
        if (interactor is XRDirectInteractor direct)
        {
            return direct.isSelectActive;
        }
        return false;
    }

    private void CreateAndGrabBall(IXRSelectInteractor interactor)
    {
        // Instanciar
        GameObject newBall = Instantiate(energyBytePrefab, interactor.transform.position, Quaternion.identity);
        
        // Obtener el componente que creamos antes
        EnergyByte ballInteractable = newBall.GetComponent<EnergyByte>();
        Rigidbody ballRb = newBall.GetComponent<Rigidbody>();

        if (ballInteractable != null && ballRb != null)
        {
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            ballRb.ResetInertiaTensor();
            // Forzar el agarre
            interactionManager.SelectEnter(interactor, (IXRSelectInteractable)ballInteractable);
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            ballRb.ResetInertiaTensor();
            
            // Opcional: Remover de la lista temporalmente para evitar doble spawn 
            // aunque hasSelection ya debería prevenirlo
        }
    }
}