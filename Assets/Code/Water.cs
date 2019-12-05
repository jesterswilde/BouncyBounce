using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public struct WaveParams{
    public float Amplitude; 
    public float Frequency; 
    public float Direction; 

}
public class Water : MonoBehaviour
{

    // public Transform waterPrefab;
    [SerializeField]
    Mesh waterMesh; 
    [SerializeField]
    Material waterMat; 
    public int width = 10;
    public int height = 10;
    public float gridSize = 1;
    float[,] waterHeights;
    List<IWave> waves = new List<IWave>();
    List<IWave> toRemove = new List<IWave>(); 
    List<ArcadeWave> arcadeWaves = new List<ArcadeWave>(); 
    HashSet<Vector2> toReset = new HashSet<Vector2>(); 
    [SerializeField]
    List<WaveParams> baseWaves;

    // Start is called before the first frame update
    void Start()
    {
        CreateWaterGrid();
        baseWaves.ForEach(param => CreateSinWave(param.Amplitude, param.Frequency, param.Direction));
        // CreateSinWave(3, 0.5f, 0f);
        // CreateSinWave(1f, 0.2f, -0.5f);
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
        waterHeights = new float[height, width];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                waterHeights[y, x] = 0;
            }
        }
    }
    void CreateSinWave(float amp, float freq, float angle)
    {
        var wave = new SinWave(freq, amp, angle);
        waves.Add(wave);
    }
    void UpdateWaves()
    {
        waves.ForEach(wave => wave.TimePass(Time.deltaTime));
    }
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

            x = Mathf.FloorToInt(hit.point.x / gridSize);
            y = Mathf.FloorToInt(hit.point.z / gridSize);
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
            x = Mathf.FloorToInt(hit.point.x / gridSize);
            y = Mathf.FloorToInt(hit.point.z / gridSize);
            Debug.Log(x + " | " + y); 
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
                float yPos = 0;
                waves.ForEach(wave => yPos += wave.WeightAtPoint(x, y));
                waterHeights[y,x] = yPos;
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
        waterHeights[y,x] += posMod;   
    }
    // void ResetWaves(){
    //     for(int i = 0; i < toReset.Count; i++){
    //         var val = toReset.ElementAt(i);
    //         var x = (int) val.x; 
    //         var y = (int) val.y; 
    //         var pos = waterHeights[y,x].position;
    //         waterHeights[y, x].position = new Vector3(pos.x, 0, pos.z); 
    //     }
    //     toReset.Clear(); 
    // }
    void DrawWater(){
        int curWidth = Mathf.Min(8,width);
        Matrix4x4[] matrices = new Matrix4x4[curWidth * height]; 
        for(int x = 0; x < width; x++){
            if(x % 8 == 0 && x != 0){
                curWidth = Mathf.Min(width-x, 8);
                Graphics.DrawMeshInstanced(waterMesh, 0, waterMat, matrices);  
                matrices = new Matrix4x4[curWidth * height];
            }
            for(int y = 0; y < height; y++){
                float waterHeight = waterHeights[y,x];
                Matrix4x4 mat = new Matrix4x4();
                mat.SetTRS(new Vector3(x * gridSize, waterHeight, y * gridSize), Quaternion.identity, Vector3.one);
                // Debug.Log(x + " | " + y + " | " + (x%8) + " | " + height + " | " + ((x%8* curWidth) + y));
                matrices[(x%8)* height+ y] = mat; 
            }
        }
        Graphics.DrawMeshInstanced(waterMesh, 0, waterMat, matrices);  
    }
    // Update is called once per frame
    void Update()
    {
        UpdateWaves();
        UpdateWater();
        // ResetWaves();
        UpdateArcadeWave();
        RemoveDeadWaves();
        DrawWater(); 
        if (Input.GetMouseButtonDown(0))
        {
            CreateArcadeWave();
        }
        if(Input.GetMouseButtonDown(1)){
            CreateSpringWave();
        }
    }
}
