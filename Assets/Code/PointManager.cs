using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PointManager : MonoBehaviour
{
    [SerializeField]
    Text timer;
    [SerializeField]
    Text pointsText; 
    int points = 0; 
    static PointManager t; 
    Dictionary<string, int> pointDict = new Dictionary<string, int>(); 

    void Update(){
        timer.text = "Time: " + Time.time.ToString("0");
        pointsText.text = "Points = " + points;
    }

    public static void AddPoints(int points, string pointType){
        t.points += points; 
        if(t.pointDict.ContainsKey(pointType)){
            t.pointDict[pointType] += points; 
        }else{
            t.pointDict[pointType] = points; 
        }
        t.pointsText.text = "Points: " + t.points; 
    }
    void Awake(){
        t = this; 
    }
}
