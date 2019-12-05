using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Water : MonoBehaviour
{

    public Transform waterPrefab;
    public int width = 10;
    public int height = 10;
    public float gridSize = 1;
    Transform[,] waterTrans;
    List<IWave> waves = new List<IWave>();
    List<IWave> toRemove = new List<IWave>(); 
    List<ArcadeWave> arcadeWaves = new List<ArcadeWave>(); 
    HashSet<Vector2> toReset = new HashSet<Vector2>(); 

    // Start is called before the first frame update
    void Start()
    {
        CreateWaterGrid();
        CreateSinWave(3, 0.5f, 0f);
        CreateSinWave(1f, 0.2f, -0.5f);
    }

    void RemoveWave(IWave wave){
        toRemove.Add(wave); 
    }
    void RemoveDeadWaves(){
        if(toRemove.Count > 0){
            toRemove.ForEach(wave => waves.Remove(wave));
            toRemove.Clear();
        }
    }

    void CreateWaterGrid()
    {
        waterTrans = new Transform[height, width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform trans = Instantiate(waterPrefab);
                trans.position = new Vector3(x * gridSize, 0, y * gridSize);
                waterTrans[y, x] = trans;
                if (x == 0)
                {
                    trans.gameObject.AddComponent<WaveFront>();
                }
            }
        }
    }
    void CreateSinWave(float amp, float freq, float angle)
    {
        var wave = new SinWave(freq, amp, angle);
        waves.Add(wave);
    }
    // void UpdateWaves()
    // {
    //     waves.ForEach(wave => wave.TimePass(Time.deltaTime));
    // }
    void OnDrawGizmos()
    {
        float centerX = (width / 2f) * gridSize;
        float centerZ = (height / 2f) * gridSize;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(centerX - gridSize / 2 ,0, centerZ - gridSize / 2), new Vector3(width, gridSize * 2, height));
    }
    void CreateSpringWave()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int x, y;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Transform trans = hit.collider.transform;
            x = Mathf.FloorToInt(trans.position.x / gridSize);
            y = Mathf.FloorToInt(trans.position.z / gridSize);
        }
        else
        {
            return;
        }
        var wave = new SpringWave(x, y);
        wave.SetRemoveFunc(RemoveWave); 
        waves.Add(wave);
    }
    void CreateArcadeWave()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int x, y;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Transform trans = hit.collider.transform;
            x = Mathf.FloorToInt(trans.position.x / gridSize);
            y = Mathf.FloorToInt(trans.position.z / gridSize);
        }
        else
        {
            return;
        }
        var wave = new ArcadeWave(x, y, width, height, this);
        arcadeWaves.Add(wave);
    }
    void UpdateWater()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform trans = waterTrans[y, x];
                float yPos = 0;
                waves.ForEach(wave => yPos += wave.WeightAtPoint(x, y));
                trans.position = new Vector3(trans.position.x, yPos, trans.position.z);
            }
        }
    }
    void UpdateArcadeWave(){
        arcadeWaves.ForEach(wave => wave.UpdateWave()); 
    }
    public void AddToWaterHeight(int x, int y, float posMod){
        if(x < 0 || x >= width || y < 0 || y >= height){
            return; 
        }
        toReset.Add( new Vector2(x,y)); 
        var trans = waterTrans[y,x].position; 
        waterTrans[y,x].position = new Vector3(trans.x, posMod + trans.y, trans.z);  
    }
    void ResetWaves(){
        for(int i = 0; i < toReset.Count; i++){
            var val = toReset.ElementAt(i);
            var x = (int) val.x; 
            var y = (int) val.y; 
            var pos = waterTrans[y,x].position;
            waterTrans[y, x].position = new Vector3(pos.x, 0, pos.z); 
        }
        toReset.Clear(); 
    }
    // Update is called once per frame
    void Update()
    {
        // UpdateWaves();
        // UpdateWater();
        ResetWaves();
        UpdateArcadeWave();
        // RemoveDeadWaves();
        if (Input.GetMouseButtonDown(0))
        {
            CreateArcadeWave();
        }
    }
}
