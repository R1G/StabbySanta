using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public GameObject[] guests;
    public GameObject[] props;
    public GameObject[] convos;
    public GameObject crime;

    private void Start()
    {
        guests = GameObject.FindGameObjectsWithTag("GUEST");
        props = GameObject.FindGameObjectsWithTag("PROP");
        convos = GameObject.FindGameObjectsWithTag("CONVERSATION");
        Invoke("SpawnCrime", 3f);
    }

    public void CheckForWitnesses(Vector3 crimeLocation)
    {
        foreach(GameObject guest in guests)
        {
            if(guest==null) {
                continue;
            }
            if(LoS.IsInLoS(crimeLocation, guest, 15f, 0.6f)) {
                Debug.Log(guest.name + " witnessed a murder!");
                guest.SendMessage("SetWitness");
            }
            
        }
    }

    public GameObject GetRandomProp() {
        int randomIndex = Random.Range(0, props.Length);
        return props[randomIndex];
    }

    public GameObject GetRandomGuest(GameObject ignore=null) {
        int randomIndex = Random.Range(0, guests.Length);
        if(guests[randomIndex]==ignore) {
            return guests[(randomIndex+1)%guests.Length];
        }
        return guests[randomIndex];
    }

    public void KillGuest() {
        guests = GameObject.FindGameObjectsWithTag("GUEST");
    }

    public void SpawnCrime() {
        Instantiate(crime);
    }
}
