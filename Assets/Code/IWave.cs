using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWave
{
    float WeightAtPoint(int x, int y);
    void TimePass(float time);
}
