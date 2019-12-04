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

    // Start is called before the first frame update
    void Start()
    {
        CreateWaterGrid();
        CreateSinWave(1, 0.5f, 0f);
        // CreateSinWave(0.5f, 0.2f, -0.5f);
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
            Transform trans = hit.collider.transform;
            x = Mathf.FloorToInt(trans.position.x / gridSize);
            y = Mathf.FloorToInt(trans.position.z / gridSize);
        }
        else
        {
            return;
        }
        var wave = new SpringWave(x, y);
        waves.Add(wave);
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
    // Update is called once per frame
    void Update()
    {
        UpdateWaves();
        UpdateWater();
        if (Input.GetMouseButtonDown(0))
        {
            CreateSpringWave();
        }
    }
}
