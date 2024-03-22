using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtn : MonoBehaviour
{
    public Sprite soundOnImage;
    public Sprite soundOffImage;
    private Image buttonImage;
    private bool isImageSoundOn;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        isImageSoundOn = true;
    }

    public void ToggleSoundImage()
    {
        if (isImageSoundOn)
            buttonImage.sprite = soundOffImage;
            
        else
            buttonImage.sprite = soundOnImage;

        isImageSoundOn = !isImageSoundOn;
    }
}
