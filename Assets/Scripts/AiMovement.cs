using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    GameObject[] wayPoints;
    NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        wayPoints = GameObject.FindGameObjectsWithTag("WAYPOINT");
    }

    void Update()
    {
        if(ReachedDestination()) {
            agent.SetDestination(GetRandomWayPointLocation());
        }
    }

    Vector3 GetRandomWayPointLocation() {
        int numPoints = wayPoints.Length;
        int randIndex = Random.Range(0, numPoints);
        return wayPoints[randIndex].transform.position;
    }

    bool ReachedDestination() {
        return !agent.pathPending & 
                agent.remainingDistance < agent.stoppingDistance &&
                     (!agent.hasPath || !(agent.velocity.sqrMagnitude > 0f));
    }
}
