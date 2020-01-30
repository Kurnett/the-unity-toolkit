using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    UnityEngine.AI.NavMeshAgent agent;
    public enum AIState { Idle, Guard, Patrol, Wander }

    public AIState current = AIState.Idle;

    public Transform guardLocation;
    public Transform wanderLocation;
    public float wanderRange = 5f;
    public Vector2 wanderTimeRange = new Vector2(5f,10f);
    public List<Transform> patrolRoute = new List<Transform>();
    public int currentRoutePosition = 0;
    public float patrolWait = 2f;
    public float wanderWait;

    public bool arrived;
    public float arrivalTime;

    void Start () {
        agent = gameObject.GetComponent<NavMeshAgent> ();
    }

    void Update () {
        Act (current);
    }

    void Act (AIState state) {
        switch (state) {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Guard:
                Guard();
                break;
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Wander:
                Wander();
                break;
        }
        current = state;
    }

    void Idle () {
        agent.SetDestination (transform.position);
    }

    void Guard () {
        agent.SetDestination (guardLocation.position);
    }

    void Patrol () {
        agent.SetDestination (patrolRoute[currentRoutePosition].position);
        if (arrived == false && Vector3.Distance(transform.position, patrolRoute[currentRoutePosition].position) < 0.5f) {
            arrivalTime = Time.time;
            arrived = true;
        }
        if (arrived == true && Time.time - arrivalTime > patrolWait) {
            currentRoutePosition = NextRoutePosition();
            arrived = false;
        }
    }
    
    void Wander () {
        if (arrived == false) {
            Vector3 target = wanderLocation.position;
            Vector2 variation = Random.insideUnitCircle * wanderRange;
            target.x += variation.x;
            target.z += variation.y;
            agent.SetDestination (target);
            arrivalTime = Time.time;
            float range = Random.Range(wanderTimeRange.x, wanderTimeRange.y);
            wanderWait = range;
            arrived = true;
        } else if (Time.time - arrivalTime > wanderWait) {
            arrived = false;
        }
    }

    int NextRoutePosition () {
        if (currentRoutePosition < patrolRoute.Count - 1) {
            return currentRoutePosition + 1;
        } else {
            return 0;
        }
    }

    public void SetState (AIState state) {
        current = state;
    }

    public void SetGuardLocation (Transform location) {
        guardLocation = location;
    }
}