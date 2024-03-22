using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfettiUIEffect : MonoBehaviour
{
    [SerializeField] Image[] _confetti;
    public IEnumerator ConfettiEffect()
    {

        while (true)
        {
            for (int i = 0; i < _confetti.Length; i++)
            {
                _confetti[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-400, 400), Random.Range(-400, 400));
            }

            while (_confetti[0].color.a < 1)
            {
                for (int i = 0; i < _confetti.Length; i++)
                {
                    _confetti[i].color += Color.black * 3 * Time.deltaTime;
                }
                yield return null;
            }

            while (_confetti[0].color.a > 0)
            {
                for (int i = 0; i < _confetti.Length; i++)
                {
                    _confetti[i].color -= Color.black * Time.deltaTime / 2;
                }
                yield return null;
            }
        }
    }
}
