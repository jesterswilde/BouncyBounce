using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Boat boat; 
    void LateUpdate(){
        transform.position = boat.transform.position; 
        // transform.forward = boat.Foward; 
    }
}
