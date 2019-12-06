using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField]
    GameObject onPlaceEffect;
    AudioSource source; 
    [SerializeField]
    bool isDraggable = false; 
    [SerializeField]
    bool movedByWater = false;
    [SerializeField]
    float buoyancy = 10; 
    [SerializeField]
    float waveDrag = 1; 
    [SerializeField]
    public int foodValue = 1; 
    public int partyValue = 1;
    public int bargeValue = 1; 
    bool isPlaced = false; 
    [SerializeField] 
    float size = 2; 
    public float Size {get{return size;}}
    Collider coll; 
    Rigidbody rigid;
    bool isGrabbed = false; 
    public bool IsGrabbed {get{return isGrabbed;}}
    public bool CanBeGrabbed{get{return !isGrabbed && !isPlaced;}}
    public void GotGrabbed(){ 
        rigid.isKinematic = true;
        isGrabbed = true;  
    }
    public void GotPlaced(){
        rigid.isKinematic = true; 
        isPlaced = true; 
        if(onPlaceEffect != null){
            Instantiate(onPlaceEffect, transform.position, Quaternion.identity); 
        }
        if(source != null){
            source.Play(); 
        }
    }
    public void GotReleased(){
        rigid.isKinematic = false;
        isGrabbed = false;  
    }
    void Awake(){
        coll = GetComponent<Collider>(); 
        size = coll.bounds.extents.z * 2;
        rigid = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>(); 
    }

    public void SetPosAndDir(Vector3 pos, Vector3 forward){
        if(isGrabbed && isDraggable && !isPlaced){
            transform.position = pos; 
            transform.forward = forward; 
        }
    }

    void Buoy(){
        if(movedByWater){
            float y = 0;
            float posAtPoint = GameManager.Water.getHeightAtPointScaled((int)transform.position.x, (int)transform.position.z);
            float forwardY =  GameManager.Water.getHeightAtPointScaled((int)transform.position.x+1, (int)transform.position.z);
            float rightY =  GameManager.Water.getHeightAtPointScaled((int)transform.position.x, (int)transform.position.z+1);
            Vector3 waveNorm = new Vector3(forwardY - posAtPoint, 0, rightY - posAtPoint); 
            float yDif = transform.position.y - posAtPoint; 
            Vector3 dir = Vector3.zero; 
            if(yDif < 0){
                rigid.useGravity = false; 
                y = (1 - (Mathf.Min(1,yDif)/1)) * buoyancy; 
            }else{
                rigid.useGravity = true; 
            }
            dir += waveNorm * waveDrag; 
            dir.y = y; 
            rigid.AddForce(dir, ForceMode.Acceleration);
        }
    }

    void Update(){
        if(!isGrabbed && movedByWater){
            Buoy(); 
        }
    }
}
