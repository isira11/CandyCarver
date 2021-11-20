using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    internal MonoBehaviour context;

    internal Action<CommonStateBase> StateChanged = delegate { };

    private CommonStateBase _current_state_;

    internal CommonStateBase current_state
    {
        get { return _current_state_; }

        private set
        {

            if (_current_state_ != null)
            {
                _current_state_.OnExit();
            }

            _current_state_ = value;
            context.StartCoroutine(CR_Set());
        }
    }

    IEnumerator CR_Set()
    {
        yield return new WaitForEndOfFrame();
        _current_state_.OnEnter();
        StateChanged(_current_state_);
    }

    public State(MonoBehaviour monoBehaviour)
    {
        context = monoBehaviour;
    }

    public void SwitchState(CommonStateBase state)
    {
        current_state = state;
    }
}
