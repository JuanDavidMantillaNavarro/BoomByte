using UnityEngine;

public class electricenemi : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerCamera;

    [Header("Movimiento")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float rotationSpeed = 5f;

    [Header("Patrulla")]
    public float patrolTimeMin = 2f;
    public float patrolTimeMax = 4f;

    [Header("Detección")]
    public float chaseDistance = 8f;

    [Header("Vision del jugador")]
    public float viewAngle = 70f;
    public float detectionDistance = 20f;
    public LayerMask visionMask;

    [Header("Paredes")]
    public float wallDistance = 1f;
    public LayerMask wallMask;

    [Header("Altura")]
    public float fixedHeight = 0.65f;

    [Header("Animación")]
    public Animator anim;

    private bool isChasing;
    private bool isBeingSeen;
    private bool isPatrolling;

    private Vector3 patrolDirection;
    private float patrolTimer;

    private void Start()
    {
        StartPatrol();
    }

    private void Update()
    {
        if (playerCamera == null)
            return;

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            playerCamera.position
        );

        isBeingSeen = PlayerCanSeeEnemy();

        if (distanceToPlayer <= chaseDistance)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            if (!isBeingSeen)
            {
                FollowPlayer();
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            Patrol();
        }

        MaintainHeight();
    }

    void Patrol()
    {
        if (!isPatrolling)
        {
            StartPatrol();
        }

        patrolTimer -= Time.deltaTime;

        if (DetectWall(patrolDirection))
        {
            Rotate90();
        }

        transform.position +=
            patrolDirection *
            patrolSpeed *
            Time.deltaTime;

        RotateTowards(patrolDirection);

        UpdateAnimation(true);

        if (patrolTimer <= 0f)
        {
            isPatrolling = false;
        }
    }

    void StartPatrol()
    {
        Vector3[] dirs =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        patrolDirection =
            dirs[Random.Range(0, dirs.Length)];

        patrolTimer =
            Random.Range(
                patrolTimeMin,
                patrolTimeMax
            );

        isPatrolling = true;
    }

    void FollowPlayer()
    {
        Vector3 dir =
            (playerCamera.position -
            transform.position).normalized;

        dir.y = 0;

        transform.position +=
            dir *
            chaseSpeed *
            Time.deltaTime;

        RotateTowards(dir);

        UpdateAnimation(true);
    }

    void StopMoving()
    {
        UpdateAnimation(false);
    }

    void RotateTowards(Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(dir);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    bool PlayerCanSeeEnemy()
    {
        Vector3 directionToEnemy =
            transform.position -
            playerCamera.position;

        float distance =
            directionToEnemy.magnitude;

        Debug.DrawRay(
            playerCamera.position,
            directionToEnemy.normalized *
            detectionDistance,
            isBeingSeen ?
            Color.green :
            Color.red
        );

        if (distance > detectionDistance)
            return false;

        float angle =
            Vector3.Angle(
                playerCamera.forward,
                directionToEnemy
            );

        if (angle > viewAngle * 0.5f)
            return false;

        Ray ray = new Ray(
            playerCamera.position,
            directionToEnemy.normalized
        );

        RaycastHit hit;

        if (Physics.Raycast(
            ray,
            out hit,
            detectionDistance,
            visionMask))
        {
            if (hit.transform.root == transform.root)
            {
                return true;
            }
        }

        return false;
    }

    bool DetectWall(Vector3 dir)
    {
        Debug.DrawRay(
            transform.position +
            Vector3.up * 0.5f,
            dir * wallDistance,
            Color.yellow
        );

        return Physics.Raycast(
            transform.position +
            Vector3.up * 0.5f,
            dir,
            wallDistance,
            wallMask
        );
    }

    void Rotate90()
    {
        int turn =
            Random.value > 0.5f ? 1 : -1;

        patrolDirection =
            Quaternion.Euler(
                0,
                90 * turn,
                0
            ) * patrolDirection;

        patrolDirection =
            SnapToAxis(
                patrolDirection.normalized
            );

        patrolTimer =
            Random.Range(1f, 2f);
    }

    Vector3 SnapToAxis(Vector3 dir)
    {
        dir.y = 0;

        if (Mathf.Abs(dir.x) >
            Mathf.Abs(dir.z))
        {
            return new Vector3(
                Mathf.Sign(dir.x),
                0,
                0
            );
        }
        else
        {
            return new Vector3(
                0,
                0,
                Mathf.Sign(dir.z)
            );
        }
    }

    void MaintainHeight()
    {
        transform.position = new Vector3(
            transform.position.x,
            fixedHeight,
            transform.position.z
        );
    }

    void UpdateAnimation(bool moving)
    {
        if (anim == null)
            return;

        anim.SetBool("isMoving", moving);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Electric Enemy Hit");

            Destroy(gameObject);
        }
    }
}