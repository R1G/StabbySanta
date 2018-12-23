using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float stabRange;
    public Animator armAnim;
    LayerMask playerMask;

    private void Start() {
        playerMask = ~LayerMask.GetMask("Player");
    }

    void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Stab();
        }
    }

    void Stab() {
        Ray stabRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0, 0.5f));
        RaycastHit hit;

        if(Physics.Raycast(stabRay, out hit, stabRange, playerMask)) {
            if(hit.transform.gameObject.tag=="BACK") {
                Destroy(hit.transform.parent.gameObject);
            }
        }

        armAnim.SetTrigger("Stab");
    }

}

