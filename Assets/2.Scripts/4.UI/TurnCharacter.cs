using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DefineHelper;

public class TurnCharacter : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerExitHandler, IEndDragHandler
{
    [SerializeField] Rigidbody _character;
    [SerializeField] Animator _characterMove;
    float speed = 1;
    Quaternion _rot;
    Vector2 _touchPoint;

    //https://www.youtube.com/watch?v=HRYodomxd-E
    //https://angliss.cc/quaternion/
    public void OnPointerDown(PointerEventData eventData)
    {
        StopCoroutine(RemainMove());
        _character.angularVelocity = Vector3.zero;
        _characterMove.Play("HardJiggle", -1, 0f);
        SoundManager._instance.PlayEffect(eEffectSound.TouchToCharacter);
        //_touchPoint = new Vector2(Input.mousePosition.x - Screen.currentResolution.width / 2, Input.mousePosition.y - Screen.currentResolution.height / 2);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Quaternion rotation = _character.transform.rotation * Quaternion.Euler(new Vector3(eventData.delta.y / 10, -eventData.delta.x / 10, 0));
        //Vector2 point = (_touchPoint - eventData.position) / 10;

        _character.AddTorque(new Vector3(eventData.delta.y / speed, -eventData.delta.x / speed, 0));

        // quartanion은 곱셈을 사용하면 된다. rotate도 같다.
        //_character.transform.Rotate( new Vector3(eventData.delta.y / 10, -eventData.delta.x / 10, 0)); // Quaternion.Euler( _rot + new Vector3(-point.y, point.x, 0));
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(RemainMove());
        //_touchPoint = new Vector2(Input.mousePosition.x - Screen.currentResolution.width / 2, Input.mousePosition.y - Screen.currentResolution.height / 2);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(RemainMove());
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(RemainMove());
    }

    public IEnumerator RemainMove()
    {
        while (_character.angularVelocity.magnitude >= 0.1f)
        {
            _character.angularVelocity = Vector3.Lerp(_character.angularVelocity, Vector3.zero, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        _character.angularVelocity = Vector3.zero;
        yield break;
    }


}
