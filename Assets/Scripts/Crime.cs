using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crime : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject crimeLocation;
    GlobalManager gm;
    public GameObject victim;
    public GameObject perp;

    void Start() {
        gm = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        SetVictim();
        Invoke("SetPerp", 3f);
    }

    void SetPerp() {
        perp = gm.GetRandomGuest();
        if(perp==victim) {
            perp = gm.GetRandomGuest(victim);
        }
        NPC_Behavior perpAI = perp.GetComponent<NPC_Behavior>();
        perpAI.SendToVictim(victim);
        perpAI.crime=this;
        Debug.Log(perp.name+" will kill " + victim.name+"...");
    }

    void SetVictim() {
        victim = gm.GetRandomGuest(null);
        crimeLocation = gm.GetRandomProp();
        NPC_Behavior victimAI = victim.GetComponent<NPC_Behavior>();
        victimAI.SendToCrime(crimeLocation);
        Debug.Log(victim.name + " is gonna die!");
    }

    public void ExecuteCrime() {
        Debug.Log(perp.name+" has killed " + victim.name + "!");
        gm.CheckForWitnesses(perp.transform.position);
        gm.KillGuest();
    }
}
