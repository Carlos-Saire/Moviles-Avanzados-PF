using System.Collections;
using UnityEngine;

public class TurnMicOffCommand : ICommand
{
    private readonly VivoxManager vivox;

    public TurnMicOffCommand(VivoxManager vivox)
    {
        this.vivox = vivox;
    }
    IEnumerator ICommand.Execute()
    {
        vivox.SetMicrophone(false);
        
        yield break;    
    }
}
