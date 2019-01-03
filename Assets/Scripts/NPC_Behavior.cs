using UnityEngine;

public class NPC_Behavior : MonoBehaviour
{
    //COMPONENTS
    Movement movement;
    FindTarget searchBehavior;
    GlobalManager gm;
    public TextMesh statusDebug;
    public GameObject deadBody;

    //CONSTANTS
    const float interactionDistance = 2f; 
    const float focusTime = 20f; //How long npc will do something until they switch states

    //FINITE STATE MANAGEMENT
    enum npcStates {Idle, Talkative, Bored, Provocative, Panic, Search, Violent};
    enum arrTypes {Prop, Guest, Convo}
    int currentState = -1;
    bool isBusy;

    //INTERACTIBLES
    GameObject targetGuest;
    GameObject targetConvo;
    GameObject targetProp;

    void Start() {
        movement = GetComponent<Movement>();
        searchBehavior = GetComponent<FindTarget>();
        gm = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        currentState = (int)npcStates.Idle;
        isBusy = false;
    }

    void Update()
    {
        switch(currentState)
        {
            case ((int)npcStates.Idle):
                {
                    statusDebug.text = "Idle";
                    isBusy = false;
                    ResetAITargets();
                    currentState = PickState(); 
                    break;
                }

            case ((int)npcStates.Talkative):
                {
                    statusDebug.text = "Talkative";
                    if (targetConvo==null)
                    {
                        targetConvo = GetRandomFromArray((int)arrTypes.Convo);
                    }
                    else
                    {
                        if(GetDistanceFrom(targetConvo)>interactionDistance)
                        {
                            movement.Approach(targetConvo);
                        }
                        else if(!isBusy)
                        {
                            Invoke("GoIdle", focusTime);
                            isBusy = true;
                        } 
                    }
                    break;
                }

            case ((int)npcStates.Bored):
                {
                    statusDebug.text = "Bored";
                    if(targetProp==null)
                    {
                        targetProp = GetRandomFromArray((int)arrTypes.Prop);
                    } else if(GetDistanceFrom(targetProp)>interactionDistance)
                    {
                        movement.Approach(targetProp);
                    }
                    else if(!isBusy)
                    {
                        isBusy = true;
                        Invoke("GoIdle", focusTime);
                    }
                    break;
                }

            case ((int)npcStates.Provocative):
                {
                    statusDebug.text = "Provocative";
                    if(targetGuest==null)
                    {
                        targetGuest = GetRandomFromArray((int)arrTypes.Guest);
                    }
                    else if(GetDistanceFrom(targetGuest)>interactionDistance)
                    {
                        movement.Approach(targetGuest);
                    }
                    else if(!isBusy)
                    {
                        targetGuest.GetComponent<NPC_Behavior>().Offend(gameObject);
                        Debug.Log(gameObject.name + "has offended " + targetGuest.name);
                        isBusy = true;
                        Invoke("GoIdle", 0.1f);
                    }
                    break;
                }

            case ((int)npcStates.Panic):
                {
                    statusDebug.text = "Panicking";
                    ResetAITargets();
                    movement.Roam();
                    break;
                }

            case ((int)npcStates.Search):
                {
                    statusDebug.text = "Searching";
                    if(targetGuest==null)
                    {
                        currentState = (int)npcStates.Idle;
                    }
                    else if(searchBehavior.target==null)
                    {
                        searchBehavior.target = targetGuest;
                    }
                    else if(searchBehavior.isFound)
                    {
                        currentState = (int)npcStates.Violent;
                    }
                    break;
                }

            case ((int)npcStates.Violent):
                
                {
                    statusDebug.text = "Violent";
                    if(targetGuest==null)
                    {
                        currentState = (int)npcStates.Idle;
                    }
                    else if(GetDistanceFrom(targetGuest)>interactionDistance)
                    {
                        movement.Approach(targetGuest);
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " has killed " + targetGuest.name);
                        Instantiate(deadBody, targetGuest.transform.position, Quaternion.identity);
                        Destroy(targetGuest);
                        gm.CheckForWitnesses(transform.position);
                        Invoke("GoIdle", 0.1f);
                    }
                    break;
                }

            default:
                {
                    statusDebug.text = "Error";
                    movement.Roam();
                    break;
                }
        }
    }

    void SetWitness(Vector3 crimeLocation)
    {
        //cast a ray from crime location to this gameobject, if in range and LoS, is witness
        if(Vector3.Distance(crimeLocation, transform.position)>8f) {
            CancelInvoke();
            currentState = (int)npcStates.Panic;
            Invoke("GoIdle", 10f);
        }

    }

    public void Offend(GameObject offender)
    {
        targetGuest = offender;
        currentState = (int)npcStates.Violent;
    }

    int PickState()
    {
        int i = Random.Range(1, 7);
        if(i==7) {
            return 4;
        }
        return i%3+1;
        
    }

    GameObject GetRandomFromArray(int arrType)
    {
        GameObject[] arr;
        switch(arrType)
        {
            case ((int)arrTypes.Convo):
                {
                    arr = gm.convos;
                    break;
                }

            case ((int)arrTypes.Guest):
                {
                    arr = gm.guests;
                    break;
                }

            case ((int)arrTypes.Prop):
                {
                    arr = gm.props;
                    break;
                }

            default:
                {
                    return null;
                }
        }

        int size = arr.Length;
        if(size==0)
        {
            return null;
        }

        int i = Random.Range(0, size - 1);

        GameObject output = arr[i];
        if(arr[i]==gameObject)
        {
            return arr[(i + 1) % (size-1)];
        }
        return output;
    }

    float GetDistanceFrom(GameObject dest)
    {
        if (dest == null)
        {
            return -1f;
        }
        return Vector3.Distance(dest.transform.position, transform.position);
    }

    void GoIdle()
    {
        currentState = (int)npcStates.Idle;
    }

    void ResetAITargets()
    {
        targetProp = null;
        targetGuest = null;
        targetConvo = null;
    }
}



