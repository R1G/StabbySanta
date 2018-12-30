using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForPlayer : MonoBehaviour {

    public float visionRange;
    public float visionAngle; //Between (1,-1) with higher values being more restrictive
    public bool searchEnabled;

    public Animator anim;
    public GameObject deadBody;

    GameObject[] wayPoints;
    NavMeshAgent agent;
    GameObject player;
    LayerMask victimMask;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        victimMask = ~LayerMask.GetMask("Victim");
        agent = GetComponent<NavMeshAgent>();
        wayPoints = GameObject.FindGameObjectsWithTag("WAYPOINT");
    }

    void Update() {
        if (searchEnabled && IsInLoS()) {
            DiscoverPlayer();
            anim.SetFloat("speed", 0f);
        } else if (ReachedDestination()) {
            agent.isStopped = false;
            agent.SetDestination(GetRandomWayPointLocation());
            anim.SetFloat("speed", 1f);
        }
    }

    void DiscoverPlayer() {
        Debug.Log("Player spotted!");
        Stealth.IncreaseThreat();
        agent.isStopped = true;
        transform.LookAt(player.transform.position);
    }

    bool IsInLoS() {
        if (Vector2.Distance(player.transform.position, transform.position) > visionRange)
            return false;

        Vector3 playerDir = player.transform.position - transform.position;
        float playerMgn = Vector3.Magnitude(player.transform.position);
        float thisMgn = Vector3.Magnitude(transform.position);

        float angle = Vector3.Dot(player.transform.position.normalized, transform.forward);

        if (angle < visionAngle)
            return false;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, playerDir, out hit, visionRange, victimMask)) {
            if(hit.transform.gameObject.tag=="Player") {
                return true;
            }
        }

        return false;
    }

    Vector3 GetRandomWayPointLocation() {
        int numPoints = wayPoints.Length;
        int randIndex = Random.Range(0, numPoints);
        return wayPoints[randIndex].transform.position;
    }

    bool ReachedDestination() {
        if (agent.isStopped)
            return true;
        return !agent.pathPending &
                agent.remainingDistance < agent.stoppingDistance &&
                     (!agent.hasPath || !(agent.velocity.sqrMagnitude > 0f));
    }

    public void Die() {
        Instantiate(deadBody, transform.position, Quaternion.identity);
        Stealth.AddKill();
        Destroy(gameObject);
    }
}
