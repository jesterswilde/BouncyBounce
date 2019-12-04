using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFront : MonoBehaviour
{
    float timeSinceLastCreate = 0; 
    float creationFreq = 0.1f; 
    float prevY;
    bool wasGoingUp = false; 
    float floor = 0; 
    float gridSize = 1; 
    // Start is called before the first frame update
    void Start()
    {
        prevY = transform.position.y; 
    }

    // void CreateCrash(float speed){
    //     float height = transform.position.y; 
    //     while(height > 0){
    //         Vector3 pos = new Vector3(transform.position.x, height, transform.position.z); 
    //         Transform trans = Instantiate(Settings.CrashPrefab, pos, Quaternion.identity);
    //         Spray spray = trans.GetComponent<Spray>(); 
    //         spray.SetDir(Vector3.left * speed); 
    //         height -= Settings.CrashHeight;
    //     }
    // }

    void UpdateDir(){
        float dif = transform.position.y - prevY; 
        if(dif < 0 && timeSinceLastCreate > creationFreq){
            timeSinceLastCreate = 0;
            // CreateCrash(5); //THIS IS TEMP SPEED
        }
        wasGoingUp = dif > 0;
        prevY = transform.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastCreate += Time.deltaTime; 
        UpdateDir(); 
    }
}
