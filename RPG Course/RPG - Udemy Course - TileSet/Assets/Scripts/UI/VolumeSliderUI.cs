using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float multiplier;   //To make UI sliders adjust the volumes more dramatatically. Theyre 0 to 1, mixer is 0 to 20.

    public void SliderValue(float _value)          //Adjusts audio mixer with sliders value.
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
    }

    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)              //If value is bigger than minimum slider value which we set to 0.001.
            slider.value = _value;    //Set slider value to value passed.
    }
}
