using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBox : MonoBehaviour
{
    [SerializeField]
    string pointType; 

    void OnCollisionEnter(Collision coll){
        Draggable thing = coll.gameObject.GetComponent<Draggable>(); 
        if(thing != null && thing.CanBeGrabbed){
            thing.GotPlaced(); 
        }
    }
}
