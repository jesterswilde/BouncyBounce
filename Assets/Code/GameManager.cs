using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int thumpers = 5; 
    public static int Thumpers{get{return t.thumpers;}}
    static GameManager t;
    [SerializeField]
    Water water;
    public static Water Water {get{return t.water;}}

    [SerializeField]
    Thumper thumperPrefab; 
    public static bool CanAddThumper(){
        return t.thumpers > 0; 
    }

    public static void AddThumperToStorage(){
        t.thumpers++; 
    }

    public static void RemoveThumperFromStorage(){
        t.thumpers--; 
    }

    // Start is called before the first frame update
    void Awake()
    {
        t = this; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void CreateThumper(Vector2 center, float force){
        if(CanAddThumper()){
            // Debug.Log(center.x  + " | " + center.y); 
            RemoveThumperFromStorage(); 
            Vector3 pos = new Vector3(center.x, 0, center.y); 
            Thumper thumper = Instantiate(t.thumperPrefab, pos, Quaternion.identity);
            thumper.SetVel(force);  
        }
    }
}
