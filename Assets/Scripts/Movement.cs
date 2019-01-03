using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    GameObject[] wayPoints;
    NavMeshAgent agent;
    public Animator anim;
    const float defaultStoppingDistance = 1f;

    private void Start()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("WAYPOINT");
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = defaultStoppingDistance;
    }

    private void Update()
    {
        float vel = agent.velocity.sqrMagnitude;
        anim.SetFloat("speed", vel);
    }

    public void Approach(GameObject moveTarget, float stoppingDistance=defaultStoppingDistance)
    {
        //CancelMovement();
        agent.SetDestination(moveTarget.transform.position);
    }

    public void Roam(bool force=false)
    {
        if(force || ReachedDestination())
        {
            agent.SetDestination(GetRandomWayPointLocation());
        }
    }

    public void FaceToward(GameObject target)
    {
        CancelMovement();
        transform.LookAt(target.transform.position);
        Vector3 difference = target.transform.position - transform.position;
        float rotationY = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
    }

    Vector3 GetRandomWayPointLocation()
    {
        int numPoints = wayPoints.Length;
        int randIndex = Random.Range(0, numPoints);
        return wayPoints[randIndex].transform.position;
    }

    public bool ReachedDestination()
    {
        if (agent.isStopped)
            return true;
        return !agent.pathPending &
                agent.remainingDistance < agent.stoppingDistance &&
                     (!agent.hasPath || !(agent.velocity.sqrMagnitude > 0f));
    }

    void CancelMovement()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }


}
