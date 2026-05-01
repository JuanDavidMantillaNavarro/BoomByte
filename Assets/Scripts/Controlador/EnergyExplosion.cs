using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using FMODUnity;

public class EnergyExplosion : MonoBehaviour
{
    private float gridRange;
    private float heightRange = 4f; 
    private float slowAmount = 0.4f;
    private float duration = 3f;
    public string destructibleTag = "Destructible";

    [Header("Efectos")]
    public EffectData RadioExplosionEffect;

    [Header("FMOD - Audio")]
    [SerializeField] private EventReference zonaDesbloqueadaSound;

    public void Initialize(float gridRadius)
    {
        gridRange = gridRadius + GameController.Instance.explosionRadiusModifier;

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

        bool inHorizontalRange = Mathf.Abs(diff.x) <= gridRange && Mathf.Abs(diff.z) <= gridRange;
        bool inVerticalRange = diff.y >= -1f && diff.y <= heightRange;

        if (inHorizontalRange && inVerticalRange)
        {
            GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
            if (playerRoot != null)
            {
                StartCoroutine(SlowDownRoutine(playerRoot));
                GameController.Instance.OnPlayerExplosion();
            }
        }
    }

    private void DestroyWallsInRange()
    {
        Vector3 center = transform.position + new Vector3(0, heightRange / 2, 0);
        Vector3 halfExtents = new Vector3(gridRange, heightRange / 2, gridRange);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity);

        bool destruyoPared = false;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag(destructibleTag))
            {
                Debug.Log("<color=orange>Pared destruida: </color>" + hit.name);
                Destroy(hit.gameObject);
                destruyoPared = true;
            }
        }

        if (destruyoPared)
        {
            RuntimeManager.PlayOneShot(zonaDesbloqueadaSound, transform.position);
            Debug.Log("Sonido zona desbloqueada");
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