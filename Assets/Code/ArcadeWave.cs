using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeWave
{
    float trailingLen = 8;
    float leadingLen = 2;
    float baseWidth = 5; 
    float apexWdith = 1; 
    float height = 4; 
    float centerX;
    float centerY; 
    float speed = 5;
    Vector3 speedVec; 
    float maxX; 
    float maxY; 
    Water water;
    public ArcadeWave(int _centerX, int _centerY, int _maxX, int _maxY, Water _water){
        water = _water; 
        centerY = _centerY; 
        centerX = _centerX; 
        maxX = _maxX; 
        maxY = _maxY; 
    }
    public void UpdateWave(){
        centerY -= Time.deltaTime * speed; 
        CalcHeights(); 
    }
    void CalcHeights(){
        for(int x = Mathf.RoundToInt(centerX - baseWidth); x < centerX + baseWidth; x++){
            for(int y = Mathf.RoundToInt(centerY - leadingLen); y < centerY + trailingLen; y++){
                float xDist = Mathf.Abs(centerX - x);
                float yDist = Mathf.Abs(centerY - y); 
                float xMod = 1;
                float yMod = 1; 
                if(xDist >= apexWdith){
                    var curWidth = xDist - apexWdith;
                    var maxWidth = baseWidth - apexWdith;
                    xMod = 1 - ((curWidth * curWidth) / (maxWidth * maxWidth));
                }else{
                    xMod = 1; 
                }
                if(y > centerY){
                    yMod = 1 - ((yDist * yDist) / (trailingLen * trailingLen));
                }else if(y < centerY){
                    yMod = 1 - (yDist / leadingLen);
                }
                water.AddToWaterHeight(x, y, height * yMod * xMod);
            }
        }
    }
}
