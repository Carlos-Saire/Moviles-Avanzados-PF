using UnityEngine;
using System.Collections;

public class SetActiveCommand : ICommand
{
    private GameObject objectToActivate;
    private bool state;

    public SetActiveCommand(GameObject obj, bool state)
    {
        this.objectToActivate = obj;
        this.state = state;
    }

    public IEnumerator Execute()
    {
        objectToActivate.SetActive(state);
        yield break; 
    }
}
