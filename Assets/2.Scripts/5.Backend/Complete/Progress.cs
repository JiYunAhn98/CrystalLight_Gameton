using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Progress : MonoBehaviour
{
    [SerializeField]
    private Slider sliderProgress;
    [SerializeField]
    private TextMeshProUGUI textProgressData;
    [SerializeField]
    private float progressTime;

    public void Play(UnityAction action = null)
    {
        StartCoroutine(Onprogress(action));
    }

    private IEnumerator Onprogress(UnityAction action)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / progressTime;

            //text information
            textProgressData.text = $"Now Loading... {sliderProgress.value*100:F0}%";
            // slider 값
            sliderProgress.value = Mathf.Lerp(0, 1, percent);

            yield return null;
        }

        
        action?.Invoke();
    }
        
  

}
