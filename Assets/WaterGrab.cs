using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGrab : MonoBehaviour
{
    [SerializeField]
    float phase = 0; 
    [SerializeField]
    float speed = 1; 
    [SerializeField]
    float distance = 2; 
    [SerializeField]
    float depth = 1;

    float curX; 
    float startX;
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x; 
        transform.position = new Vector3(startX + Mathf.Sin(phase) * distance, transform.position.y, transform.position.z); 
        curX = transform.position.x; 
    }
    void Move(){
        curX = Mathf.Sin(phase * speed) * distance + startX; 
        transform.position = new Vector3(curX, transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        phase += Time.deltaTime; 
        Move(); 
    }
}
