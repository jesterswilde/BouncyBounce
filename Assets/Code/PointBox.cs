using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType{
    TRASH,
    FOOD,
    PARTY
}
public class PointBox : MonoBehaviour
{
    [SerializeField]
    PointType pointType; 

    void OnCollisionEnter(Collision coll){
        Draggable thing = coll.gameObject.GetComponent<Draggable>(); 
        if(thing != null && thing.CanBeGrabbed){
            thing.GotPlaced();
            if(pointType == PointType.FOOD){
                PointManager.AddPoints(thing.foodValue, "food");
            }else if(pointType == PointType.PARTY){
                PointManager.AddPoints(thing.partyValue, "party");
            }else if(pointType == PointType.TRASH){
                PointManager.AddPoints(thing.bargeValue, "trash");
            }
        }
    }
}
