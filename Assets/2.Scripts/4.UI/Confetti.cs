using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confetti : MonoBehaviour
{
    [SerializeField] Image _confetti;
    bool _isConfettSizePlus;
    public void ConfettiEffect()
    {
        if (_confetti.color.a <= 0)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(500, Random.Range(-400, 400));
            _isConfettSizePlus = true;
        }

        if (_isConfettSizePlus)
        {
            _confetti.color += Color.black * 3 * Time.deltaTime;
            if (_confetti.color.a >= 1) _isConfettSizePlus = false;
        }
        else
        {
            _confetti.color -= Color.black * Time.deltaTime / 2;
            if (_confetti.color.a <= 0) _isConfettSizePlus = true;
        }
    }
}
