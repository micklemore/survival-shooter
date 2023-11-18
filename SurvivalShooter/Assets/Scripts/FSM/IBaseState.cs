using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseState
{
    public void OnEnter();

    public void OnExit();

    public void UpdateLogic();

    public void UpdatePhysics();
}
