using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using FMODUnity; 

public class EnergyExplosion : MonoBehaviour
{
    private float gridRange;
    private float heightRange = 4f; 
    private float slowAmount = 0.4f; // 40%
    private float duration = 3f;
    public string destructibleTag = "Destructible";

    [Header("Efectos")]
    public EffectData RadioExplosionEffect;

    [Header("FMOD - Audio")] // Mantenido de Main
    [SerializeField] private EventReference zonaDesbloqueadaSound;

    public void Initialize(float gridRadius)
    {
        gridRange = gridRadius + GameController.Instance.explosionRadiusModifier;
        
        // Ejecutamos todas las lógicas combinadas
        AjustarSprite(); 
        ApplySlowEffect();
        DestroyWallsInRange();
    }

    void AjustarSprite() 
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;
        
        float spriteSize = sr.bounds.size.x;
        float targetSize = gridRange * 2f; // diámetro (no radio)
        float scaleFactor = targetSize / spriteSize;

        sr.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
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
            // ── Enemigos ──────────────────────────────────────────────────
            if (hit.CompareTag("Enemy"))
            {
                EnemySpawnPoint spawnData = hit.gameObject.GetComponent<EnemySpawnPoint>();
                // Notifica al GameController
                GameController.Instance.OnEnemyExplosion(hit.gameObject.transform.position, hit.gameObject.name,spawnData);
                Destroy(hit.gameObject);
            }
        }

        // Si se destruyó algo, suena el FMOD
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
        if (gridRange <= 0) return; // Seguridad para que no ensucie el editor si no se ha inicializado
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position + new Vector3(0, heightRange / 2, 0);
        Vector3 size = new Vector3(gridRange * 2, heightRange, gridRange * 2);
        Gizmos.DrawWireCube(center, size);
    }
}