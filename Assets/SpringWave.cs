using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpringWave : IWave
{
    Action<IWave> removeSelf;
    float height;
    int centerX;
    int centerY;
    bool isPlaying = false;
    float springAmount = 1;
    float timer = 0;

    float yChange = 0;
    float phase = 0;
    float decay = 0.6f;
    float speed = 3;

    public SpringWave(int x, int y)
    {
        centerX = x;
        centerY = y;
    }
    public void SetRemoveFunc(Action<IWave> wave)
    {
        removeSelf = wave;
    }
    void ReadInput()
    {
        if (!isPlaying)
        {
            yChange += Input.GetAxisRaw("Mouse Y") * Settings.YSpeed;
        }
    }

    public void TimePass(float time)
    {
        if (isPlaying)
        {
            phase += time;
            CountdownTimer();
        }
        else
        {
            ReadInput();
            if (Input.GetMouseButtonUp(0))
            {
                isPlaying = true;
                timer = yChange * 2;
            }
        }
    }

    void CountdownTimer()
    {
        if (isPlaying)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                removeSelf(this);
            }
        }
    }

    float WeightWhileHolding(int x, int y)
    {
        if (x == centerX && y == centerY)
        {
            return yChange;
        }
        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
        return (1 / ((dist * dist) + 1)) * yChange;
    }

    float WeightAfterReleasing(int x, int y)
    {
        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
        if (phase * speed - dist < 0)
        {
            return 0;
        }
        float distDecay = (1 / Mathf.Log(dist + 3));
        float timeDecay = (1 / (phase + 1)) * decay;
        return Mathf.Cos(-phase * speed + dist) * yChange * timeDecay;
    }

    public float WeightAtPoint(int x, int y)
    {
        if (isPlaying)
        {
            return WeightAfterReleasing(x, y);
        }
        return WeightWhileHolding(x, y);
    }
}
