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
    [SerializeField]
    float boatSize = 2; 
    [SerializeField]
    float maxPitch = 10f; 
    [SerializeField]
    float pitchAcceleration = 1f; 
    
    [SerializeField]

    float lerpSpeed = 0.5f; 
    Vector3 forwardDir = Vector3.forward;
    public Vector3 Foward {get{return forwardDir;}}
    // Start is called before the first frame update
    Rigidbody rigid;
    AudioSource sound; 
    List<Draggable> dragging = new List<Draggable>(); 
    void Start()
    {
        sound = GetComponentInChildren<AudioSource>();
        rigid = GetComponent<Rigidbody>(); 
        if(sound != null){
            sound.loop = true; 
            sound.Play(); 
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        DragThings(); 
    }

    void Update(){
        HandleRelease();
        LookForward();
        HandlePitch();  
    }

    void HandlePitch(){
        if(sound == null){
            return; 
        }
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            sound.pitch = Mathf.Min(maxPitch, sound.pitch + pitchAcceleration * Time.deltaTime);
        }else{
            sound.pitch = Mathf.Max(1, sound.pitch - pitchAcceleration * Time.deltaTime * 2); 
        }
    }

    void LookForward(){
        Vector3 tempVel = new Vector3(rigid.velocity.x, rigid.velocity.y * 0.5f, rigid.velocity.z); 
        Vector3 look = Vector3.Lerp(transform.forward, tempVel, lerpSpeed * Time.deltaTime);
        transform.forward = look; 
    }

    void Move(){
        float x = Input.GetAxisRaw("Horizontal"); 
        float z = Input.GetAxisRaw("Vertical");
        forwardDir = new Vector3(x,0,z); 
        float y = 0;
        float posAtPoint = GameManager.Water.getHeightAtPointScaled((int)transform.position.x, (int)transform.position.z);
        float forwardY =  GameManager.Water.getHeightAtPointScaled((int)transform.position.x+1, (int)transform.position.z);
        float rightY =  GameManager.Water.getHeightAtPointScaled((int)transform.position.x, (int)transform.position.z+1);
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

    void HandleRelease(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            dragging.ForEach(thing => thing.GotReleased());
            dragging.Clear(); 
        }
    }

    void DragThings(){
        float backDist = boatSize;
        Vector3 back = transform.forward * -1;  
        for(int i = 0; i < dragging.Count; i++){
            var thing = dragging[i]; 
            thing.SetPosAndDir(transform.position + backDist * back, transform.forward); 
            backDist += thing.Size; 
        }
    }

    void OnCollisionEnter(Collision collision){
        var thingToDrag = collision.gameObject.GetComponent<Draggable>(); 
        if(thingToDrag != null && thingToDrag.CanBeGrabbed){
            thingToDrag.GotGrabbed(); 
            dragging.Add(thingToDrag); 
        }
    }


}
