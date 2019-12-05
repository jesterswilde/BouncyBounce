using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField]
    float maxSpeed = 5; 
    [SerializeField]
    float acceleration = 2; 
    [SerializeField]
    float buoyancy = 5; 
    Vector3 forwardDir = Vector3.forward;
    public Vector3 Foward {get{return forwardDir;}}
    // Start is called before the first frame update
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move(){
        float x = Input.GetAxisRaw("Horizontal"); 
        float z = Input.GetAxisRaw("Vertical");
        forwardDir = new Vector3(x,0,z); 
        float y = 0;
        float posAtPoint = GameManager.Water.GetHeightAtPoint((int)transform.position.x, (int)transform.position.z);
        float forwardY =  GameManager.Water.GetHeightAtPoint((int)transform.position.x+1, (int)transform.position.z);
        float rightY =  GameManager.Water.GetHeightAtPoint((int)transform.position.x, (int)transform.position.z+1);
        Vector3 waveNorm = new Vector3(forwardY - posAtPoint, 0, rightY - posAtPoint); 
        float yDif = transform.position.y - posAtPoint; 
        Vector3 dir = new Vector3(x, 0, z).normalized * acceleration * Time.deltaTime;
        // Vector3 velForward = transform.forward;
        // velForward.y = 0; 
        // velForward = velForward.normalized; 
        // rigid.velocity += velForward * Time.fixedDeltaTime *acceleration;
        if(yDif < 0){
            rigid.useGravity = false; 
            y = (1 - (Mathf.Min(1,yDif)/1)) * buoyancy; 
            // y += GameManager.Water.GetVelocityOfPoint((int)transform.position.x, (int)transform.position.z);
            
            // Debug.Log(" up"); 
        }else{
            rigid.useGravity = true; 
            // Debug.Log(" down"); 
        }
        dir += waveNorm; 
        dir.y = y; 
        rigid.AddForce(dir, ForceMode.Acceleration);
    }
}
