using System;
using System.Collections;
using UnityEngine;

public class GenericCommad : ICommand
{
    private Action action;
    public GenericCommad(Action action  )
    {
        this.action = action;
    }
    public IEnumerator Execute()
    {
        action.Invoke();
        yield break;
    }
}
