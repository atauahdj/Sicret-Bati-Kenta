using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviour
{
    public Transform[] points;
    public int currentPoint;
    public float MaxTime;
    private float currentTime;
    public Animator anim;
    public Transform player;
    public float range;
    private FPC FPC;
    private GameObject Head;
    private float speedAt;
    private float speedDef;
    public Vector3 EnemyPosition;
    public Vector3 PlayerPosition;
    NavMeshAgent agent;
    private bool isAttacking = false;
    public GameObject lastPosPref;
    private GameObject lastPos;
    private float twc;
    private bool wasSpawned;
    void Start()
    {
        Head = GameObject.Find("Head");
        FPC = FindObjectOfType<FPC>();
        anim = GetComponent<Animator>();
        currentPoint = 0;
        agent = GetComponent<NavMeshAgent>();
        speedDef = agent.speed;
        speedAt = agent.speed * 1.4f;
    }

    private void Update()
    {
        if (isAttacking)
        {
            AttackLogic();
            return;
        }

        if (CanSeePlayer())
        {
            twc = 0f;
            wasSpawned = false;
            Chase();
            Destroy(lastPos);
        }
        else
        {
            twc += Time.deltaTime;

            if (!wasSpawned && twc >= 0.1f)
            {
                lastPos = Instantiate(lastPosPref, player.position, Quaternion.identity);
                wasSpawned = true;
            }
            Patrol();
        }
        UpdateAnimation();
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > range)
            return false;

        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red, 0.1f);
            if (hit.collider.gameObject == this.gameObject)
            {
                return false;
            }

            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    void Patrol()
    {
        agent.speed = speedDef;
        agent.isStopped = false;
        if(lastPos != null)
        {
            agent.destination = lastPos.transform.position;
            if (Vector3.Distance(transform.position, lastPos.transform.position) < 1f)
            {
                Destroy(lastPos);
            }
        }
        else
        {
            agent.destination = points[currentPoint].position;
        }
        if (Vector3.Distance(transform.position, points[currentPoint].position) <= 1f)
        { 
            agent.isStopped = true;
            currentTime += Time.deltaTime;

            if (currentTime >= MaxTime)
            {
                currentTime = 0f;
                int newPoint = currentPoint;
                while (newPoint == currentPoint && points.Length > 1)
                {
                    newPoint = Random.Range(0, points.Length);
                }
                currentPoint = newPoint;
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }

    void Chase()
    {
        twc = 1f;
        agent.speed = speedAt;
        agent.destination = player.position;
        agent.isStopped = false;

        if (Vector3.Distance(transform.position, player.position) <= 2f)
        {
            isAttacking = true;
            currentTime = 0f;
        }
    }

    void AttackLogic()
    {
        agent.isStopped = true;
        currentTime += Time.deltaTime;
        FPC.speed = 0f;
        transform.LookAt(FPC.transform);
        FPC.transform.LookAt(transform.position);
        if (currentTime >= 1f)
        {
            transform.position = EnemyPosition;
            FPC.transform.position = PlayerPosition;
            FPC.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            FPC.speed = 2f;
            agent.speed = speedDef;
            isAttacking = false;
        }
    }

    void UpdateAnimation()
    {
        bool Chase = false;
        bool isMoving = !agent.isStopped && agent.velocity.magnitude > 0.1f;
        if (twc == 1f || lastPos != null)
        {
            Chase = true;
        }
        else
        {
            Chase = false;
        }
        if (twc < 1f) anim.SetBool("Walk", isMoving);
        else anim.SetBool("Running", Chase);
    }
}