namespace VictorGame
{
    using System;
    using Unity.Services.Authentication;
    using UnityEngine;
    public class PlayerProfile : MonoBehaviour
    {
        public static event Action<string> OnNameChanged;

        private string currentName;

        public void SetInitialName(string name)
        {
            currentName = name;
            OnNameChanged?.Invoke(name);
        }

        public async void UpdatePlayerName(string newName)
        {
            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(newName);
                currentName = newName;
                OnNameChanged?.Invoke(newName);
            }
            catch (Exception e)
            {
                Debug.LogError("Error al actualizar nombre: " + e.Message);
            }
        }

        public string GetName() => currentName;
    }
}
