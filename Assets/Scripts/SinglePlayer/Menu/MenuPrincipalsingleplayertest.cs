using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiPanelNuevo : MonoBehaviour
{
    [SerializeField] private Button botonInicial;

    private void OnEnable()
    {
        UISelector.SeleccionarInicial(botonInicial);
    }
}
