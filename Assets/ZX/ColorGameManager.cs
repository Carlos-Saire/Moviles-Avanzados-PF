using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class ColorGameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CombinationColorSO combinationsDB;

    [Header("UI References")]
    [SerializeField] private Image targetColorImage;
    [SerializeField] private Image mixColorImage;
    [SerializeField] private GameObject miniGamePanel;

    [Header("Color Buttons")]
    [SerializeField] private List<Button> potionButtons = new List<Button>();
    [SerializeField] private List<Color> potionColors = new List<Color>();

    private readonly List<Color> selectedIngredients = new List<Color>();
    private Color targetColor;

    public Action OnPotionGameCompleted;

    private void Awake()
    {
        ValidateSetup();
        SetupButtons();
    }

    private void OnEnable()
    {
        ResetMix();
        GenerateNewTargetColor();
    }

    // ---------------- VALIDACIÓN -------------------

    private void ValidateSetup()
    {
        if (potionButtons.Count != potionColors.Count)
        {
            Debug.LogError("PotionGameManager: El número de botones y colores no coincide.");
        }

        if (combinationsDB == null)
        {
            Debug.LogError("PotionGameManager: No se asignó el ScriptableObject de combinaciones.");
        }
    }

    // ---------------- CONFIGURACIÓN -------------------

    private void SetupButtons()
    {
        for (int i = 0; i < potionButtons.Count; i++)
        {
            int index = i;

            Image img = potionButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = potionColors[i];

            potionButtons[i].onClick.RemoveAllListeners();
            potionButtons[i].onClick.AddListener(() =>
            {
                AddIngredient(potionColors[index]);
            });
        }
    }

    // ---------------- LÓGICA PRINCIPAL -------------------

    private void AddIngredient(Color color)
    {
        selectedIngredients.Add(color);
        UpdateMixPreview();
    }

    public void MixIngredients()
    {
        Color? result = TryGetCombinationResult();

        if (result == null)
        {
            mixColorImage.color = Color.black; // combinación inválida
            return;
        }

        mixColorImage.color = result.Value;

        if (ColorsMatch(result.Value, targetColor))
            CompleteMiniGame();
    }

    private Color? TryGetCombinationResult()
    {
        foreach (var combo in combinationsDB.combinations)
        {
            if (ListsMatch(combo.ingredients, selectedIngredients))
                return combo.resultColor;
        }

        return null;
    }

    private void CompleteMiniGame()
    {
        OnPotionGameCompleted?.Invoke();
        miniGamePanel.SetActive(false);
    }

    // ---------------- UTILIDADES -------------------

    private void UpdateMixPreview()
    {
        if (selectedIngredients.Count == 0)
        {
            mixColorImage.color = Color.white;
            return;
        }

        Color avg = AverageColor(selectedIngredients);
        mixColorImage.color = avg;
    }

    private Color AverageColor(List<Color> colors)
    {
        Color total = new Color(0, 0, 0, 0);

        foreach (var c in colors)
            total += c;

        return total / colors.Count;
    }

    private void GenerateNewTargetColor()
    {
        int index = UnityEngine.Random.Range(0, combinationsDB.combinations.Count);
        targetColor = combinationsDB.combinations[index].resultColor;
        targetColorImage.color = targetColor;
    }

    private bool ListsMatch(List<Color> a, List<Color> b)
    {
        if (a.Count != b.Count) return false;
        return !a.Except(b).Any();
    }

    private bool ColorsMatch(Color a, Color b)
    {
        return a.Equals(b);
    }

    // ---------------- ACCIONES UI -------------------

    public void ResetMix()
    {
        selectedIngredients.Clear();
        UpdateMixPreview();
    }

    public void CloseMiniGame()
    {
        miniGamePanel.SetActive(false);
    }
}
