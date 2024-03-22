using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class InformManager : MonoSingleton<InformManager>
{
    public Queue<eCharacter> _unlockChars { get; set; }
    public Queue<eLockThings> _unlockThings { get; set; }
    public bool _tutorialComplete { get; set; }
    public bool _firstEnter { get; set; }

    public void Initialize()
    {
        _unlockChars = new Queue<eCharacter>();
        _unlockThings = new Queue<eLockThings>();
        _tutorialComplete = false;
        _firstEnter = true;
    }

    public void EnqueueCharacter(eCharacter chararcter)
    {
        BackendGameData.Instance.SetBodyState((int)chararcter, 1);
        _unlockChars.Enqueue(chararcter);
    }
    public void EnqueueOthers(eLockThings other)
    {
        _unlockThings.Enqueue(other);
    }
}
