using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    int _index;

    public IEnumerator BarMove(float time)
    {
        _index = -1;
        GameObject nowGo;

        while (true)
        {
            _index = (_index + 1) % transform.childCount;
            nowGo = transform.GetChild(_index).gameObject;
            nowGo.GetComponent<Image>().color += Color.black * 0.9f;
            nowGo.GetComponent<RectTransform>().anchoredPosition += Vector2.up * nowGo.GetComponent<RectTransform>().sizeDelta.y / 2;
            yield return new WaitForSeconds(time);
            nowGo.GetComponent<Image>().color -= Color.black * 0.9f;
            nowGo.GetComponent<RectTransform>().anchoredPosition += Vector2.down * nowGo.GetComponent<RectTransform>().sizeDelta.y /2;
        }
    }
}
