using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWave: IWave
{
    float[,] weights; 
    float frequency; 
    float amplitude;
    float decay; 
    float angle; 
    float phase = 0; 

    public SinWave(float _frequency, float _amplitude, float _angle){ 
        frequency = _frequency;
        amplitude = _amplitude; 
        angle = _angle;
    }
    public void TimePass(float time)
    {
        phase += time; 
    }

    public float WeightAtPoint(int x, int y, float timeDif = 0)
    {
        float pos = x * 1-angle + y * angle; 
        return Mathf.Sin((pos + phase - timeDif) * frequency) * amplitude; 
    }
}
