using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DefineHelper;

public class StoryTeller : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] int _finish;
    [SerializeField] int[] _imgFadeOutTime;
    [SerializeField] Image _fade;
    int index = 0;

    public bool UpdatePanel(float speed)
    {
        if (_rect.anchoredPosition.y < _finish)
        {
            _rect.anchoredPosition -= Vector2.down * speed * Time.deltaTime;
        }
        if (index < _imgFadeOutTime.Length)
        {
            if (_imgFadeOutTime[index] <= _rect.anchoredPosition.y)
            {
                transform.GetChild(index).GetComponent<Image>().color -= Color.black * Time.deltaTime;
                if (transform.GetChild(index).GetComponent<Image>().color.a <= 0)
                {
                    index++;
                }
            }
        }
        if (index >= _imgFadeOutTime.Length && _rect.anchoredPosition.y >= _finish)
        {
            if (transform.GetChild(index - 1).GetComponent<Image>().color.a > 0)
            {
                transform.GetChild(index).GetComponent<Image>().color += Color.black * Time.deltaTime;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}
