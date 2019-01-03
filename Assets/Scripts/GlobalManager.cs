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
}
