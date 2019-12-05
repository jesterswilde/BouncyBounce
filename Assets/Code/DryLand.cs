using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryLand : MonoBehaviour
{
    BoxCollider coll; 

    void Awake(){
        coll = GetComponent<BoxCollider>(); 
    }

    void Start(){
        var min = coll.bounds.min; 
        var max = coll.bounds.max; 
        for(int x = Mathf.FloorToInt(min.x); x < Mathf.CeilToInt(max.x); x++){
            for(int y = Mathf.FloorToInt(min.z); y < Mathf.CeilToInt(max.z); y++){
                GameManager.Water.RegisterDryLand(new Vector2(x,y));
            }
        }

    }
}
