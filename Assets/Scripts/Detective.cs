using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detective : MonoBehaviour
{
    public float interactionRange;
    void Update() {
        if(Input.GetButtonDown("Fire2")) {
            RaycastHit hit;
            
            if(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane)), out hit, interactionRange)) {
                Debug.Log("Clicked on " + hit.transform.gameObject.name);
                hit.transform.gameObject.SendMessage("ToggleInterrogate", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
