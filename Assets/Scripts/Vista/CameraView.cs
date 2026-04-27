using System.Collections;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject mainCameraObject;
    public GameObject secondaryCameraObject;

    public void ShowSecondaryView(float duration)
    {
        StartCoroutine(ViewRoutine(duration));
    }

    private IEnumerator ViewRoutine(float duration)
    {
        // Activar secundaria, desactivar principal
        mainCameraObject.SetActive(false);
        secondaryCameraObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        // Volver a la normalidad
        secondaryCameraObject.SetActive(false);
        mainCameraObject.SetActive(true);
    }
}