using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boat : MonoBehaviour
{
    [SerializeField]
    float minBoostThresh = 0.5f;
    [SerializeField]
    float waterBoostMult = 15;
    [SerializeField]
    float boost = 10; 
    [SerializeField]
    float acceleration = 2; 
    [SerializeField]
    float buoyancy = 5; 
    [SerializeField]
    float boatSize = 2; 
    [SerializeField]
    float maxPitch = 10f; 
    List<ParticleSystem> vfxs; 
    [SerializeField]
    float pitchAcceleration = 1f; 
    
    [SerializeField]

    float lerpSpeed = 0.5f; 
    Vector3 forwardDir = Vector3.forward;
    public Vector3 Foward {get{return forwardDir;}}
    // Start is called before the first frame update
    Rigidbody rigid;
    AudioSource sound; 
    float emissionVal = 0; 
    List<Draggable> dragging = new List<Draggable>(); 
    void Awake(){
        sound = GetComponentInChildren<AudioSource>();
        rigid = GetComponent<Rigidbody>(); 
        if(sound != null){
            sound.loop = true; 
            sound.Play(); 
        }
        vfxs = GetComponentsInChildren<ParticleSystem>().ToList(); 
    }
    void Start()
    {
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
        HandleVFX(); 
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
    void HandleVFX(){
        if(vfxs.Count == 0){
            return;
        }
        float posAtPoint = GameManager.Water.getHeightAtPointScaled((int)transform.position.x, (int)transform.position.z);
        if(transform.position.y > posAtPoint){
            vfxs.ForEach(vfx =>{
                var emission = vfx.emission; 
                ParticleSystem.MinMaxCurve tempCurve = vfx.emission.rateOverTime;
                tempCurve.constant = 0;
                emission.rateOverTime = tempCurve; 
            });
            return; 
        }
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            emissionVal =  Mathf.Min(100, emissionVal + 20 * Time.deltaTime);
            vfxs.ForEach(vfx =>{
                var emission = vfx.emission; 
                ParticleSystem.MinMaxCurve tempCurve = vfx.emission.rateOverTime;
                tempCurve.constant = emissionVal;
                emission.rateOverTime = tempCurve; 
            });
        }else{
            emissionVal = Mathf.Max(0, emissionVal - 40 * Time.deltaTime);
            vfxs.ForEach(vfx =>{
                var emission = vfx.emission; 
                ParticleSystem.MinMaxCurve tempCurve = vfx.emission.rateOverTime;
                tempCurve.constant = emissionVal;
                emission.rateOverTime = tempCurve; 
            });
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
        float yUp = GameManager.Water.GetVelocityOfPoint((int)transform.position.x, (int)transform.position.z);
        float yDif = transform.position.y - posAtPoint; 
        Vector3 dir = new Vector3(x, 0, z).normalized * acceleration * Time.deltaTime;
        if(dir == Vector3.zero && Input.GetKey(KeyCode.Space)){
            dir.x = rigid.velocity.x; 
            dir.z = rigid.velocity.z;
            dir = dir.normalized; 
            dir*= boost;
        }
        // Vector3 velForward = transform.forward;
        // velForward.y = 0; 
        // velForward = velForward.normalized; 
        // rigid.velocity += velForward * Time.fixedDeltaTime *acceleration;
        if(yDif < 0){
            rigid.useGravity = false; 
            y = (1 - (Mathf.Min(1,yDif)/1)) * buoyancy; 
            if(posAtPoint > minBoostThresh){
                y+= posAtPoint * waterBoostMult; 
            }
            // y += GameManager.Water.GetVelocityOfPoint((int)transform.position.x, (int)transform.position.z);
            
            // Debug.Log(" up"); 
        }else{
            rigid.useGravity = true; 
            // Debug.Log(" down"); 
        }
        dir += waveNorm; 
        dir.y = y; 
        dir.y += yUp; 
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
