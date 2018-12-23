using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForPlayer : MonoBehaviour {

    public float visionRange;
    public float visionAngle; //Between (1,-1) with higher values being more restrictive

    GameObject player;
    LayerMask victimMask;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        victimMask = ~LayerMask.GetMask("Victim");
    }

    void Update() {
        if (IsInLoS()) 
            DiscoverPlayer();
    }

    void DiscoverPlayer() {
        Debug.Log("Player spotted!");
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
}
