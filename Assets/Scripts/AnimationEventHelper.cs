using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAnimationEventTriggered, OnAttackPeformed_1, OnAttackPeformed_2;

    public void TriggerEvent()
    {
        OnAnimationEventTriggered?.Invoke();
    }
    public void TriggerAttack_1()
    {
        OnAttackPeformed_1?.Invoke();
    }
    public void TriggerAttack_2()
    {
        OnAttackPeformed_2?.Invoke();
    }
}
