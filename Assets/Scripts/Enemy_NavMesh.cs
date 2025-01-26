using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_NavMesh : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 targetLastPos;

    NavMeshAgent agent;
    bool enemyInRange;

    [SerializeField] private float offSightDetectionRadius = 5.0f;
    [SerializeField] private float currDetectionRadius = 5.0f;
    [SerializeField] private float onSightDetectionRadius = 10f;

    [SerializeField] private float SearchLastPosDuration = 5f;
    Vector2 spawn_pos;
    private int frames = 0; //exclusive for patrolling every x frames
    Collider thisCollider;

    // Raycasting and detecting player
    private Vector3 pos1, pos2, pos3;
    private List<Vector3> positions = new List<Vector3>();

    Vector3 forward;
    Vector3 toPlayer;

    LayerMask layerMask;
    AudioLowPassFilter audioPassFilter;
    public float raycastAngle; //DEFINIR NO INSPECTOR
    Vector3 raycastDirection;

    bool wasIChasing = false;
    bool onSearchLastPos = false;
    AbilityController abilityController;
    bool playerAbilityOnUse = false;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        abilityController = target.GetComponent<AbilityController>();
        layerMask = LayerMask.GetMask("Player", "Walls");
        audioPassFilter = GetComponentInChildren<AudioLowPassFilter>();
        spawn_pos = this.transform.position;
        //thisCollider = GetComponent<Collider>();
    }
    private void FixedUpdate()
    {
        playerAbilityOnUse = abilityController.abilityInUse;
        //if (playerAbilityOnUse)
        //{
        //    thisCollider.isTrigger = true;
        //}
        //else
        //    thisCollider.isTrigger = false;
        frames++;
        RayCastToMuffleSound();
        UpdateTargetPosition();
        CheckIfImFacingPlayer();
        if (IsPlayerInRange() && !playerAbilityOnUse) //&& ability is not used)
        {
            Chase();

        }
        else
        {
            if (wasIChasing)
            {
                wasIChasing = false;
                onSearchLastPos = true;

            }
            if (onSearchLastPos)
            {
                PatrolLastPlayerPosition();
                StartCoroutine(PatrolLastPosDuration(SearchLastPosDuration));
            }
            else
                Patrol();
        }

        if (frames >= 5000)
            frames = 0;
    }
    private void UpdateTargetPosition()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private bool CheckIfImFacingPlayer()
    {
        forward = transform.TransformDirection(Vector3.forward);
        toPlayer = Vector3.Normalize(target.transform.position - transform.position);
        print(Vector3.Dot(forward, toPlayer));
        if (Vector3.Dot(forward, toPlayer) < 0.70f)
        {
            print("where is player me dont know");
            currDetectionRadius = offSightDetectionRadius;
            return true;
        }
        if (Vector3.Dot(forward, toPlayer) >= 0.70f)
        {
            print("hmmm me know where is player");
            currDetectionRadius = onSightDetectionRadius;
        }
        return false;
    }
    private void Chase()
    {
        wasIChasing = true;
        print("CHASE MODE - " + currDetectionRadius + " - ");
        agent.SetDestination(target.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (raycastDirection - transform.position), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, (raycastDirection - transform.position) * hit.distance, Color.red);


        }
        targetLastPos = target.transform.position;
    }
    IEnumerator PatrolLastPosDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        onSearchLastPos = false;

    }
    private void Patrol()
    {
        print("PATROL MODE - " + currDetectionRadius + " - ");
        if (frames % 300 == 0) // cada X frames anda
        {
            agent.SetDestination(spawn_pos + Random.insideUnitCircle * 5); //random circle onde vão dar patrol é num círculo de raio 5 e centro no spawn_point
        }

    }
    private void PatrolLastPlayerPosition()
    {
        agent.SetDestination(spawn_pos + Random.insideUnitCircle * 5); //random circle onde vão dar patrol é num círculo de raio 5 e centro no spawn_point
    }

    private bool IsPlayerInRange()
    {
        //Debug.Log(Vector2.Distance(this.transform.position, target));
        if (Vector2.Distance(this.transform.position, target.transform.position) <= currDetectionRadius)
        {
            return true;
        }
        return false;

    }
    void RayCastToMuffleSound()
    {
        if (frames % 2 == 0)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer

            if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    print("Player Hit by raycast");
                    audioPassFilter.enabled = false;
                }
                if (hit.transform.gameObject.CompareTag("Wall"))
                {
                    print("Walls Hit by raycast");
                    audioPassFilter.enabled = true;

                }
                Debug.DrawRay(transform.position, (target.transform.position - transform.position) * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(transform.position, (target.transform.position - transform.position) * 1000, Color.white);
                Debug.Log("Did not Hit");

            }
        }
    }
}