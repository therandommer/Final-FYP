using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetSliderDefault : MonoBehaviour
{
    private Slider thisSlider;
    void Start()
    {
        thisSlider = GetComponent<Slider>();
        thisSlider.value = FindObjectOfType<AudioSource>().volume;
    }
}
