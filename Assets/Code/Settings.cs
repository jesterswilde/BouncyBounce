using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    static Settings t;
    [SerializeField]
    float ySpeed = 1;
    public static float YSpeed { get { return t.ySpeed; } }
    [SerializeField]
    float thumpStartForce = 5;
    public static float ThumpStartForce {get{return t.thumpStartForce;}}
    [SerializeField]
    float thumpModForce = 4;
    public static float ThumpModForce {get{return t.thumpStartForce;}}
    [SerializeField]
    float thumpCooldown = 1;
    public static float ThumpCooldown {get{return t.thumpCooldown;}}
    // Start is called before the first frame update
    void Awake()
    {
        t = this;
    }
}
