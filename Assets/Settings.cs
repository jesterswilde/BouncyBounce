using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    static Settings t;
    [SerializeField]
    float ySpeed = 1;
    public static float YSpeed { get { return t.ySpeed; } }
    // Start is called before the first frame update
    void Awake()
    {
        t = this;
    }
}
