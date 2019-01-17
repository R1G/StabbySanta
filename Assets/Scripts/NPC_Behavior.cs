using UnityEngine;

public class NPC_Behavior : MonoBehaviour
{
    //COMPONENTS
    Movement movement;
    FindTarget searchBehavior;
    GlobalManager gm;
    DialogueSystem ds;
    public GameObject nameTitle;
    public GameObject deadBody;
    public Crime crime;

    //CONSTANTS
    const float interactionDistance = 2f; 
    const float focusTime = 20f; //How long npc will do something until they switch states

    //FINITE STATE MANAGEMENT
    enum npcState {Idle, Talkative, Bored, Provocative, Panic, Search, Violent, Interrogated};
    enum arrTypes {Prop, Guest, Convo}
    npcState currentState = npcState.Idle;
    bool isBusy;

    //INTERACTIBLES
    GameObject targetGuest;
    GameObject targetConvo;
    GameObject targetProp;
    GameObject player; 

    void Start() {
        movement = GetComponent<Movement>();
        searchBehavior = GetComponent<FindTarget>();
        gm = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
        ds = GameObject.Find("PlayerCanvas").GetComponent<DialogueSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = npcState.Idle;
        isBusy = false;
        nameTitle = Instantiate(nameTitle, transform.position+Vector3.up, Quaternion.identity) as GameObject;
        nameTitle.GetComponent<TextMesh>().text=gameObject.name;
    }

    void Update()
    {
        switch(currentState)
        {
            case (npcState.Idle): {
                    isBusy = false;
                    ResetAITargets();
                    currentState = PickState(); 
                    break;
                }

            case (npcState.Talkative): {
                    if (targetConvo==null)
                    {
                        targetConvo = GetRandomFromArray(arrTypes.Convo);
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

            case (npcState.Bored): {
                    if(targetProp==null)
                    {
                        targetProp = GetRandomFromArray(arrTypes.Prop);
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

            case (npcState.Violent): {
                if(targetGuest==null) {
                    currentState=npcState.Idle;
                } else if(GetDistanceFrom(targetGuest)>interactionDistance) {
                    movement.Approach(targetGuest);
                } else {
                    Kill();
                }
                break;
            }

            case (npcState.Panic): {
                if(player==null) {
                    currentState = npcState.Idle;
                } else if(GetDistanceFrom(player)>interactionDistance) {
                    movement.Approach(player);
                } else {
                    Invoke("GoIdle", 3f);
                }
                break;
            }

            case(npcState.Interrogated): {
                if(GetDistanceFrom(player)>interactionDistance*3f) {
                    currentState = npcState.Idle;
                    ds.SetDialogue(gameObject.name+": Goodbye");
                    ds.DelayClearLog();
                } else {
                    movement.CancelMovement();
                    movement.FaceToward(player);
                    ds.SetDialogue(gameObject.name+": I haven't seen anything");
                }
                break;
            }

            default:{
                    movement.Roam();
                    break;
                }
        }
        nameTitle.transform.position=transform.position+Vector3.up;
    }

    void SetWitness()
    {
        currentState = npcState.Panic;
        Invoke("GoIdle", 10f);
    }

    public void SendToCrime(GameObject _prop) {
        currentState = npcState.Bored;
        targetProp = _prop;
    }

    public void SendToVictim(GameObject _guest) {
        currentState = npcState.Violent;
        targetGuest = _guest;
    }

    void ToggleInterrogate() {
        if(currentState==npcState.Interrogated) {
            currentState = npcState.Idle;
            ds.SetDialogue(gameObject.name+": Goodbye");
            ds.DelayClearLog();
        } else {
            currentState = npcState.Interrogated;
        }
    }

    npcState PickState()
    {
        return (npcState)Random.Range(0,3);
    }

    GameObject GetRandomFromArray(arrTypes _type)
    {
        GameObject[] arr;
        switch(_type)
        {
            case (arrTypes.Convo):
                {
                    arr = gm.convos;
                    break;
                }

            case (arrTypes.Guest):
                {
                    arr = gm.guests;
                    break;
                }

            case (arrTypes.Prop):
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
        currentState = npcState.Idle;
    }

    void ResetAITargets()
    {
        targetProp = null;
        targetGuest = null;
        targetConvo = null;
    }

    void Kill() {
        targetGuest.GetComponent<Health>().TakeDamage(1000);
        targetGuest = null;
        crime.ExecuteCrime();
    }
}





