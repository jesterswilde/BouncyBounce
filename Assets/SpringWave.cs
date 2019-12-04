using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringWave : IWave
{
    float height; 
    int centerX; 
    int centerY; 
    bool isPlaying = false;
    float springAmount = 1; 

    float yChange = 0; 
    float phase = 0; 
    float decay = 0.6f; 
    float speed = 3; 

    public SpringWave(int x, int y){
        centerX = x;
        centerY = y; 
    }

    void ReadInput(){
        if(!isPlaying){
            yChange += Input.GetAxisRaw("Mouse Y") * Settings.YSpeed;
        }
    }

    public void TimePass(float time)
    {
        if(isPlaying){
            phase += time; 
        }else{
            ReadInput(); 
            if(Input.GetMouseButtonUp(0)){
                isPlaying = true; 
            }
        }
    }

    float WeightWhileHolding(int x, int y){
        if(x == centerX && y == centerY){
            return yChange; 
        }
        float dist = Vector2.Distance(new Vector2(x,y), new Vector2(centerX, centerY));
        return (1 / ((dist * dist) + 1)) * yChange;
    }

    float WeightAfterReleasing(int x, int y){   
        float dist = Vector2.Distance(new Vector2(x,y), new Vector2(centerX, centerY));
        float distDecay = (1 / (dist + 1)) * speed; 
        float timeDecay = (1 / (phase + 1)) * decay;  
        return Mathf.Cos(phase * distDecay) * timeDecay * yChange; 
    }

    public float WeightAtPoint(int x, int y)
    {
        if(isPlaying){
            return WeightAfterReleasing(x,y);
        }
        return WeightWhileHolding(x,y); 
    }   
}
