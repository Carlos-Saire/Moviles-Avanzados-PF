using System.Collections;
using UnityEngine;

public class TurnMicOnCommand : ICommand
{
    private readonly VivoxManager vivox;

    public TurnMicOnCommand(VivoxManager vivox)
    {
        this.vivox = vivox;
    }
    IEnumerator ICommand.Execute()
    {
        vivox.SetMicrophone(true);
        yield break;
    }

}
