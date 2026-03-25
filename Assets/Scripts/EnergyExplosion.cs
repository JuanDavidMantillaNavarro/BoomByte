using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class EnergyExplosion : MonoBehaviour
{
    private float gridRange;
    private float heightRange = 4f; 
    private float slowAmount = 0.4f; // 40%
    private float duration = 3f;
    public string destructibleTag = "Destructible";

    public void Initialize(float gridRadius)
    {
        gridRange = gridRadius; 
        
        // Ejecutamos ambas lógicas
        ApplySlowEffect();
        DestroyWallsInRange();
    }

    private void ApplySlowEffect()
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) return;

        Vector3 playerPos = mainCam.transform.position;
        Vector3 explosionPos = transform.position;
        Vector3 diff = playerPos - explosionPos;

        // Verificamos si el jugador está en el cubo
        bool inHorizontalRange = Mathf.Abs(diff.x) <= gridRange && Mathf.Abs(diff.z) <= gridRange;
        bool inVerticalRange = diff.y >= -1f && diff.y <= heightRange;

        if (inHorizontalRange && inVerticalRange)
        {
            GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
            if (playerRoot != null)
            {
                StartCoroutine(SlowDownRoutine(playerRoot));
            }
        }
    }

    private void DestroyWallsInRange()
    {
        // Definimos el centro y el tamaño de la caja de detección (el mismo del Gizmo)
        Vector3 center = transform.position + new Vector3(0, heightRange / 2, 0);
        Vector3 halfExtents = new Vector3(gridRange, heightRange / 2, gridRange);

        // Detectamos todos los colisionadores dentro de esa caja
        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag(destructibleTag))
            {
                Debug.Log("<color=orange>Pared destruida: </color>" + hit.name);
                // Aquí podrías instanciar partículas de escombros antes de destruir
                Destroy(hit.gameObject);
            }
        }
    }

    IEnumerator SlowDownRoutine(GameObject player)
    {
        var moveProvider = player.GetComponentInChildren<ContinuousMoveProvider>();
        if (moveProvider == null) yield break;

        float originalSpeed = moveProvider.moveSpeed;
        float targetSpeed = originalSpeed * (1f - slowAmount);

        Debug.Log("<color=green>¡Jugador atrapado! Ralentizando...</color>");
        moveProvider.moveSpeed = targetSpeed;

        yield return new WaitForSeconds(duration - 0.5f);

        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            moveProvider.moveSpeed = Mathf.Lerp(targetSpeed, originalSpeed, elapsed / 0.5f);
            yield return null;
        }

        moveProvider.moveSpeed = originalSpeed;
        
        // No destruimos el objeto aquí para dejar que las partículas terminen si las hay
        // El objeto de la explosión se destruye al final del Initialize o con un timer
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + new Vector3(0, heightRange / 2, 0);
        Vector3 size = new Vector3(gridRange * 2, heightRange, gridRange * 2);
        Gizmos.DrawWireCube(center, size);
    }
}