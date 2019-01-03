using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindTarget : MonoBehaviour
{
    public float visionRange;
    public float visionAngle; //Between (1,-1) with higher values being more restrictive
    public bool searchEnabled;

    GameObject[] wayPoints;
    NavMeshAgent agent;
    GameObject obj1;
    LayerMask victimMask;
    Movement movement;

    public GameObject target;
    public bool isFound;


    void Start()
    {
        victimMask = ~LayerMask.GetMask("Victim");
        movement = GetComponent<Movement>();
    }

    void DiscoverTarget()
    {
        movement.Approach(target);
    }

}