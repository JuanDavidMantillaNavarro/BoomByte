using UnityEngine;

public class ErrorEnemy : MonoBehaviour
{
    public EnemysModel model;
    int combinedMask = 0;

    private void Awake()
    {
        
    }

    private void Update()
    {
        Move();
        CheckWallAndRotate();
    }

    void Move()
    {
        transform.position += model.direction * model.speedError * Time.deltaTime;
        // 🔥 ROTACIÓN SUAVE HACIA LA DIRECCIÓN
        if (model.direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(model.direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                model.rotationSpeed * Time.deltaTime
            );
        }
    }

    void CheckWallAndRotate()
    {
        Ray ray = new Ray(transform.position, model.direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, model.direction * model.detectionDistance, Color.red);
        combinedMask = 0;
       foreach (LayerMask layer in model.wallLayer)
        {
            combinedMask |= layer;
        }

        if (Physics.Raycast(ray, out hit, model.detectionDistance, combinedMask))
        {
            Rotate90();
        }
    }

    void Rotate90()
    {
        // Giro aleatorio a izquierda o derecha (90°)
        int turn = Random.value > 0.5f ? 1 : -1;

        model.direction = Quaternion.Euler(0, 90 * turn, 0) * model.direction;

        model.direction.y = 0;
        model.direction = model.direction.normalized;

        // Forzar que quede alineado a ejes (importante para pasillos)
        model.direction = SnapToAxis(model.direction);
    }

    Vector3 SnapToAxis(Vector3 dir)
    {
        dir.y = 0;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            return new Vector3(Mathf.Sign(dir.x), 0, 0);
        else
            return new Vector3(0, 0, Mathf.Sign(dir.z));
    }

    void OnTriggerEnter(Collider other)
    {
    // Verificamos si el enemigo tocó el "PlayerTarget"
    if (other.CompareTag("Player")) 
    {
            Debug.Log("Error 404");
            EnemySpawnPoint spawnData = gameObject.GetComponent<EnemySpawnPoint>();
            GameController.Instance.OnEnemyCollide(transform.position, "Error 404", spawnData);
            Destroy(gameObject);
    }
    }
}
