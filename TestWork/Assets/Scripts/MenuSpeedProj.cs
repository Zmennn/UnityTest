using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSpeedProj : MonoBehaviour
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

        slider.minValue = 500;
        slider.maxValue = 900;
        slider.onValueChanged.AddListener(Changed);
        text.text = slider.value.ToString();
    }

    private void Changed(float value)
    {
        text.text = value.ToString();
        moveMain.projectileSpeed = value / 4f;
    }


}
