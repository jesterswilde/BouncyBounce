using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    bool isDragging = false; 
    float draggingStartedTime; 
    float draggingStartedY;
    Vector2 clickPos; 
    float dragForce = 0; 
    Thumper targetThumper = null; 
    // Update is called once per frame
    [SerializeField]
    LayerMask layerMask; 
    void CheckUnderClick(){
        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        if(Physics.Raycast(ray, out hit, 10000, layerMask)){
            var go = hit.collider.gameObject; 
            targetThumper = go.GetComponent<Thumper>(); 
            if(targetThumper == null){
                clickPos = new Vector2(hit.point.x, hit.point.z); 
                // clickPos = new Vector2(hit.point.x / (GameManager.Water.gridSize + GameManager.Water.spacing), hit.point.z / (GameManager.Water.gridSize + GameManager.Water.spacing)); 
                // Debug.Log("Click Pos:  " + hit.point); 
            }
        }
    }
    
    void Update()
    {
        if(isDragging == false && Input.GetMouseButtonDown(1)){
            CheckUnderClick(); 
            isDragging = true; 
            draggingStartedTime = Time.time; 
            draggingStartedY = Input.mousePosition.y; 
        }else if(isDragging == true && Input.GetMouseButtonUp(1)){
            isDragging = false; 
            float timeDif = Time.time - draggingStartedTime; 
            float distDif = Input.mousePosition.y - draggingStartedY; 
            dragForce = distDif /timeDif;
            if(targetThumper != null){
                targetThumper.AddMomentum(dragForce); 
            }else{
                GameManager.CreateThumper(clickPos, dragForce);
            }
        }
    }
}
