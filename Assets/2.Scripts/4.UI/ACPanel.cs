using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACPanel : MonoBehaviour
{
    [HideInInspector] public bool _beforePanelClose;
    public abstract void Initialize();
    public abstract IEnumerator OpenMove();
    public abstract IEnumerator CloseMove();
    public abstract void PanelUpdate();


    //public override void Initialize()
    //{
    //}
    //public override IEnumerator OpenMove()
    //{
    //}
    //public override IEnumerator CloseMove()
    //{
    //}
    //public override void PanelUpdate()
    //{
    //}
}
