using System.Collections;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject mainCameraObject;
    public GameObject secondaryCameraObject;
    public bool activo = false;

    public void ShowSecondaryView()
    {
        AlternarMapa();
    }

    private void AlternarMapa()
    {
        // Activar secundaria, desactivar principal
        if(!activo)
        {
            mainCameraObject.SetActive(false);
            secondaryCameraObject.SetActive(true);
        }
    
        //yield return new WaitForSeconds(duration);

        // Volver a la normalidad
        if(activo)
        {
            secondaryCameraObject.SetActive(false);
            mainCameraObject.SetActive(true);
        }
        activo = !activo;
    }
}