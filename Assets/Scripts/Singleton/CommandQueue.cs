using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Command
{
    public class CommandQueue : MonoBehaviour
    {
        public static CommandQueue Instance;

        private readonly Queue<ICommand> _commandsToExecute = new Queue<ICommand>();
        private ICommand currentComand;
        private bool _runningCommand = false;
        private void Reset()
        {
            gameObject.name = "CommandQueue";

        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddCommand(ICommand commandToEnqueue)
        {
            _commandsToExecute.Enqueue(commandToEnqueue);

            if (!_runningCommand)
            {
                StartCoroutine(RunNextCommand());
            }
        }

        private IEnumerator RunNextCommand()
        {
            _runningCommand = true;

            while (_commandsToExecute.Count > 0)
            {
                currentComand = _commandsToExecute.Dequeue();
                yield return StartCoroutine(currentComand.Execute());
            }

            _runningCommand = false;
        }
    }
}
