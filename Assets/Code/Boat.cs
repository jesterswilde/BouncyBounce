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
        float y = 0;
        float posAtPoint = GameManager.Water.GetHeightAtPoint((int)transform.position.x, (int)transform.position.z);
        float forwardY =  GameManager.Water.GetHeightAtPoint((int)transform.position.x+1, (int)transform.position.z);
        float rightY =  GameManager.Water.GetHeightAtPoint((int)transform.position.x, (int)transform.position.z+1);
        Vector3 waveNorm = new Vector3(forwardY - posAtPoint, 0, rightY - posAtPoint); 
        float yDif = transform.position.y - posAtPoint; 
        Vector3 dir = new Vector3(x, 0, z).normalized * acceleration * Time.deltaTime;
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
