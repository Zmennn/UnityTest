using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRateOfFire : MonoBehaviour
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

        slider.minValue = 300;
        slider.maxValue = 700;
        slider.onValueChanged.AddListener(Changed);
        text.text = Mathf.RoundToInt(slider.value).ToString();
    }

    private void Changed(float value)
    {
        text.text = Mathf.RoundToInt(slider.value).ToString();
        moveMain.rateOfFire = Mathf.RoundToInt(slider.value);
    }

}
