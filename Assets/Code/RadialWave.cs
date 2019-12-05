using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialWave : IWave
{
    float phase = 0;
    float speed; 
    float targetDistance;
    float minDistSqr;
    float maxDistSqr; 
    float leadingLen = 2; 
    float trailingLen = 6;
    Vector2 center; 
    float height; 
    Water water; 
    float maxTime = 8; 
    float phaseDif = 0; 

    public RadialWave(Vector2 _center, float _height, float _speed, float _phaseDif = 0){
        center = _center;
        height = _height;
        speed = _speed;  
        phaseDif = _phaseDif; 
    }


    public void TimePass(float time)
    {
        phase += Time.deltaTime;
        targetDistance = speed * phase;
        minDistSqr = (targetDistance - trailingLen) * (targetDistance - trailingLen); 
        maxDistSqr = (targetDistance + leadingLen) * (targetDistance + leadingLen);
        // Debug.Log("Dists:" + minDistSqr + " | " + maxDistSqr); 
        if(phase > maxTime){
            GameManager.Water.RemoveWave(this); 
        }  
    }


    public float WeightAtPoint(int x, int y, float timeDif = 0)
    {
        var dir = new Vector2(x-center.x,y-center.y);
        var distSqr = dir.SqrMagnitude();
        if(distSqr < minDistSqr || distSqr > maxDistSqr){
            return 0; 
        }
        var dist = dir.magnitude; 
        float mod = 1; 
        float modTargetDist = targetDistance - timeDif; 
        float distDif = Mathf.Abs(dist - modTargetDist); 
        if(dist >= modTargetDist){
            mod = 1 - ((distDif * distDif) / (trailingLen * trailingLen));
        }else{
            mod = 1 - (distDif * distDif) / (leadingLen * leadingLen); 
        }
        mod = ((height * 0.5f) / dist) * mod; 
        return mod; 
    }

}
