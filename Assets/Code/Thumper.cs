using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thumper : MonoBehaviour
{
    [SerializeField]
    float speed = 3; 
    float dir; 
    [SerializeField]
    float size = 1; 
    float phase = 0;
    bool isAboveWater = true;  
    bool isDragging = false; 
    float draggingStartedTime; 
    float draggingStartedPos; 

    float ClampForce(float force){
        float dir = force > 0 ? 1 :  -1; 
        float modForce = Mathf.Log(Mathf.Abs(force)); 
        return Mathf.Clamp(modForce * dir, -10, 10);
    }
    public void AddMomentum(float intensity){
        float force = ClampForce(intensity); 
        float dirMod = force * dir >= 0 ? 1 : -1; 
        force = Mathf.Abs(force);
        if(size + force < 0){
            GameManager.AddThumperToStorage(); 
            Destroy(gameObject); 
        } 
        size += force; 
        speed += force * 0.1f; 
    }
    public void SetVel(float force){
        float intensity = ClampForce(force); 
        dir = intensity >= 0 ? 1 : -1; 
        speed = speed + intensity * 0.1f; 
        size = intensity; 
    }
    void Move(){
        float oldY = transform.position.y; 
        float yPos = Mathf.Sin(phase * speed) * size;
        Vector3 pos = transform.position; 
        transform.position = new Vector3(pos.x, yPos, pos.z);
        dir = oldY < yPos ? 1 : -1; 
        if(isAboveWater && yPos < 0){
            isAboveWater = false;
            MakeWave(Mathf.PI /2);  
        }else if(!isAboveWater && yPos > 0){
            isAboveWater = true; 
            MakeWave();
        }
    }

    void MakeWave(float phaseDif = 0){
        Vector2 center = new Vector2(transform.position.x, transform.position.z); 
        RadialWave wave = new RadialWave(center, (size/2) * (size/2), speed * 2 + 8, phaseDif);
        GameManager.Water.AddWave(wave); 
    }

    void Update(){
        phase += Time.deltaTime;
        Move(); 
    }
}
