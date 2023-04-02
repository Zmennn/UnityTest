using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDispersion : MonoBehaviour
{
    private Slider slider;
    public Text text;
    private GameObject cam;
    private MoveMain moveMain;
    void Start()
    {
        slider = GetComponent<Slider>();
        cam = GameObject.Find("Camera");
        moveMain = cam.GetComponent<MoveMain>();

        slider.minValue = 0;
        slider.maxValue = 6;
        slider.onValueChanged.AddListener(Changed);
        text.text = slider.value.ToString();
    }

    private void Changed(float value)
    {
        text.text = value.ToString();
        moveMain.dispersion = value;
    }

}



