using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject cam;
    public Slider speedSlider;
    public Text speedText;
    private MoveMain moveMain;
    private float lastUpdateTime;
    

    void Start()
    {
        moveMain = cam.GetComponent<MoveMain>();

        speedSlider.minValue = 100;
        speedSlider.maxValue = 400;
        speedSlider.onValueChanged.AddListener(SpeedChanged);
        
        
    }

    private void SpeedChanged(float value)
    {
        
        

        if (Time.time - lastUpdateTime >= 0.1f)
        {
            speedText.text = value.ToString();
            moveMain.speed = value;
            lastUpdateTime = Time.time;
        }
    }



    
}
