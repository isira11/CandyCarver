using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonStateBase 
{
    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void FixedUpdate() { }

    public virtual void Update() { }

    public virtual void OnCollisionEnter(Collision collision) { }

    public virtual void OnTriggerEnter(Collider collider) { }

    public override string ToString() { return GetType().ToString(); }
}
