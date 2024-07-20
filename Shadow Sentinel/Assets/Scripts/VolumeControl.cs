using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volParam = "MasterVolume";
    [SerializeField] AudioMixer audMixer;
    [SerializeField] Slider currSlider;

    [SerializeField] float multiplier = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(volParam))
        {
            currSlider.value = PlayerPrefs.GetFloat(volParam, currSlider.value);
        }
        else
        {
            currSlider.value = PlayerPrefs.GetFloat(volParam, 0.50f);
        }
    }
    private void Awake()
    {
        currSlider.onValueChanged.AddListener(SetVolume);
    }
    public void SetVolume(float volume)
    {
        audMixer.SetFloat(volParam, Mathf.Log10(volume) * multiplier);
        PlayerPrefs.SetFloat(volParam, volume);
    }

}
