using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class EnergyExplosion : MonoBehaviour
{
    private float gridRange;
    private float heightRange = 4f;
    private float slowAmount = 0.4f;
    private float duration = 3f;

    public string destructibleTag = "Destructible";

    [Header("Efectos")]
    public EffectData RadioExplosionEffect;

    public void Initialize(float gridRadius)
    {
        gridRange = gridRadius + GameController.Instance.explosionRadiusModifier;
        //GameController.Instance.effectManager.ApplyEffect(RadioExplosionEffect);
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
        Vector3 center = transform.position + new Vector3(0, heightRange / 2f, 0);
        Vector3 halfExtents = new Vector3(gridRange, heightRange / 2f, gridRange);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity);

        Debug.Log("Cantidad de colliders detectados: " + hitColliders.Length);

        foreach (Collider hit in hitColliders)
        {
            Debug.Log("Detectó collider: " + hit.name + " | tag: " + hit.tag);

            if (hit.CompareTag(destructibleTag))
            {
                Debug.Log("<color=orange>Pared destruida: </color>" + hit.name);
                //animacion destruir?
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
}