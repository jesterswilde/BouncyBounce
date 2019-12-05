using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField]
    bool isDraggable = false; 
    [SerializeField]
    bool movedByWater = false;
    float size; 
    public float Size {get{return size;}}
    Collider coll; 
    Rigidbody rigid;

    public void GotGrabbed(){
        rigid.isKinematic = true; 
    }
    public void GotReleased(){
        rigid.isKinematic = false; 
    }

    void Awake(){
        coll = GetComponent<Collider>(); 
        size = coll.bounds.extents.z * 2;
        rigid = GetComponent<Rigidbody>(); 
    }

    void Buoy(){
        if(movedByWater){
            
        }
    }
}
