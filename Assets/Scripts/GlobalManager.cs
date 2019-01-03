using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public GameObject[] guests;
    public GameObject[] props;
    public GameObject[] convos;

    private void Start()
    {
        guests = GameObject.FindGameObjectsWithTag("GUEST");
        props = GameObject.FindGameObjectsWithTag("PROP");
        convos = GameObject.FindGameObjectsWithTag("CONVERSATION");
    }

    public void CheckForWitnesses(Vector3 crimeLocation)
    {
        foreach(GameObject guest in guests)
        {
            if(guest==null) {
                continue;
            }
            guest.SendMessage("SetWitness", crimeLocation);
        }
    }
}
