using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class ThumperSelector : MonoBehaviour
{
    float cooldownTimer = 0; 
    List<Thumper> thumpers = new List<Thumper>(); 
    Thumper selectedThumper; 
    void OnTriggerEnter(Collider coll){
        var thumper = coll.gameObject.GetComponent<Thumper>();
        if(thumper != null){
            thumpers.Add(thumper); 
        }
    }

    void OnTriggerExit(Collider coll){
        var thumper = coll.gameObject.GetComponent<Thumper>(); 
        if(thumper != null){
            thumpers.Remove(thumper); 
        }
    }

    void UpdateSelectedThumper(){
        Thumper oldThumper = selectedThumper; 
        if(thumpers.Count == 0){
            if(selectedThumper != null){
                selectedThumper.Deselect(); 
                selectedThumper = null; 
            }
            return; 
        }
        if(thumpers.Count == 1 && selectedThumper == null){
            if(selectedThumper != null && selectedThumper != thumpers[0]){
                selectedThumper.Deselect(); 
            }
            selectedThumper = thumpers[0]; 
            selectedThumper.Select(); 
            return;
        }
        float closetDist = 10000; 
        Thumper closestThump = null; 
        for(int i = 0; i < thumpers.Count; i++){
            var thump = thumpers[i]; 
            var dist = Vector3.Distance(transform.position, thump.transform.position); 
            if(dist < closetDist){
                closetDist= dist; 
                closestThump = thump; 
            }
        }
        if(selectedThumper != closestThump && selectedThumper != null){
            selectedThumper.Deselect(); 
        }
        selectedThumper = closestThump;
        selectedThumper.Select(); 
    }

    void IncreaseThump(){
        if(cooldownTimer < Settings.ThumpCooldown){
            return;
        }
        cooldownTimer = 0; 
        if(selectedThumper == null){
            Vector2 back = new Vector2(transform.forward.x, transform.forward.z) * -5;
            GameManager.CreateThumper(back + new Vector2(transform.position.x, transform.position.z), Settings.ThumpStartForce);
        }else{
            selectedThumper.AddMomentum(Settings.ThumpModForce);
        }
    }

    void DecreaseThump(){
        if(selectedThumper == null){
            return;
        }
        selectedThumper.AddMomentum(Settings.ThumpModForce * -1);
        if(selectedThumper.WillBeDeleted){
            thumpers.Remove(selectedThumper); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelectedThumper(); 
        cooldownTimer += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.E)){
            IncreaseThump(); 
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            DecreaseThump(); 
        }
    }
}
