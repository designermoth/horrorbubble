using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_NavMesh : MonoBehaviour
{
    [SerializeField] GameObject target;
    NavMeshAgent agent;
    bool enemyInRange;
    private float detectionRadius = 5.0f;
    Vector2 spawn_pos;
    private int frames = 0; //exclusive for patrolling every x frames

    // Raycasting
    private Vector3 pos1, pos2, pos3;
    private List<Vector3> positions = new List<Vector3>();

    Vector3 forward;
    Vector3 toPlayer;

    LayerMask layerMask;
    public float raycastAngle; //DEFINIR NO INSPECTOR
    Vector3 raycastDirection;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        // Raycasting
        raycastAngle = raycastAngle * Mathf.Deg2Rad;
        raycastDirection = new Vector3(Mathf.Sin(raycastAngle), 0, Mathf.Cos(raycastAngle));

        pos1 = gameObject.transform.GetChild(0).transform.GetChild(0).transform.position;
        pos1 = gameObject.transform.GetChild(0).transform.GetChild(1).transform.position;
        pos1 = gameObject.transform.GetChild(0).transform.GetChild(2).transform.position;

        layerMask = LayerMask.GetMask("Player", "Walls");
        spawn_pos = this.transform.position;
    }
    private void FixedUpdate()
    {
        if (frames % 3 == 0)
        {
            forward = transform.TransformDirection(Vector3.forward);
            toPlayer = Vector3.Normalize(target.transform.position - transform.position);
            print(Vector3.Dot(forward, toPlayer));
            if (Vector3.Dot(forward, toPlayer) < 0)
            {
                print("The other transform is behind me!");
            }


        }



        if (IsPlayerInRange() == true)
        {
            Chase();

        }
        else
        {
            Patrol();
        }

    }

    private void Chase()
    {
        agent.SetDestination(target.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (raycastDirection - transform.position), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, (raycastDirection - transform.position) * hit.distance, Color.red);


        }
    }
    private void Patrol()
    {

        frames++;
        if (frames % 300 == 0) // cada X frames anda
        {
            agent.SetDestination(spawn_pos + Random.insideUnitCircle * 5); //random circle onde vão dar patrol é num círculo de raio 5 e centro no spawn_point
            frames = 0;
        }

    }
    private bool IsPlayerInRange()
    {
        //Debug.Log(Vector2.Distance(this.transform.position, target.transform.position));
        if (Vector2.Distance(this.transform.position, target.transform.position) <= detectionRadius)
        {
            return true;
        }
        return false;

    }
}