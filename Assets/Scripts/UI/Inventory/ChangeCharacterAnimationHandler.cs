using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacterAnimationHandler : MonoBehaviour {

    public event Action OnEntryFinishEvent;

    public void OnEntryFinishFunc()
    {
        if (OnEntryFinishEvent != null)
            OnEntryFinishEvent();
    }
}
