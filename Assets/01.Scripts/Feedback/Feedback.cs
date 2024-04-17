using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void CreateFeedback();
    public abstract void FinishFeedback();
    
    protected Agent _owner;

    private void Awake()
    {
        _owner = transform.parent.GetComponent<Agent>();
    }

    protected virtual void OnDisable()
    {
        FinishFeedback();
    }
}
