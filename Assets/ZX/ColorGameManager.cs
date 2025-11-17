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

    public System.Action OnPotionGameCompleted;

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
        if (combinationsDB == null)
            Debug.LogError("falta ScriptableObject");
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
            potionButtons[i].onClick.AddListener(() => AddIngredient(potionColors[index]));
        }
    }

    // ---------------- LÓGICA -------------------
    private void AddIngredient(Color color)
    {
        selectedIngredients.Add(color);
        UpdateMixPreview();
    }

    public void MixIngredients()
    {
        Debug.Log($"color seleccionados ({selectedIngredients.Count}):");
        for (int i = 0; i < selectedIngredients.Count; i++)
            Debug.Log($"  {i}: {selectedIngredients[i]}");

        Color? result = TryGetCombinationResult();

        if (result == null)
        {
            mixColorImage.color = Color.black;
            Debug.LogWarning("No válido");
            return;
        }

        mixColorImage.color = result.Value;
        Debug.Log($"Resultado: {result.Value}");
        Debug.Log($"Color objetivo: {targetColor}");

        if (ColorsMatch(result.Value, targetColor))
        {
            Debug.Log("Ganaste");
            CompleteMiniGame();
        }
        else
        {
            Debug.Log("Siguelo intentando.");
        }
    }

    private Color? TryGetCombinationResult()
    {
        foreach (var combo in combinationsDB.combinations)
        {
            if (ListsMatchFlexible(combo.ingredients, selectedIngredients))
            {
                if (combo.resultColors.Count > 0)
                    return combo.resultColors[UnityEngine.Random.Range(0, combo.resultColors.Count)];
            }

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
        var validResults = combinationsDB.combinations
            .SelectMany(c => c.resultColors)
            .Distinct()
            .ToList();

        if (validResults.Count == 0)
        {
            Debug.LogError("No hay colores R en el ScriptableObject");
            return;
        }

        targetColor = validResults[UnityEngine.Random.Range(0, validResults.Count)];
        targetColorImage.color = targetColor;
        Debug.Log($"Nuevo color generado: {targetColor}");
    }

    private bool ListsMatchFlexible(List<Color> comboColors, List<Color> selected, float tolerance = 0.02f)
    {
        if (comboColors.Count != selected.Count) return false;

        List<Color> remaining = new List<Color>(selected);

        foreach (var c in comboColors)
        {
            bool found = false;
            for (int i = 0; i < remaining.Count; i++)
            {
                if (ColorsMatch(c, remaining[i], tolerance))
                {
                    remaining.RemoveAt(i);
                    found = true;
                    break;
                }
            }
            if (!found) return false;
        }

        return remaining.Count == 0;
    }

    private bool ColorsMatch(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
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

// ---------------- Comparador de Color con tolerancia -------------------
public class ColorComparer : IEqualityComparer<Color>
{
    private readonly float tolerance;

    public ColorComparer(float tolerance = 0.01f)
    {
        this.tolerance = tolerance;
    }

    public bool Equals(Color x, Color y)
    {
        return Mathf.Abs(x.r - y.r) < tolerance &&
               Mathf.Abs(x.g - y.g) < tolerance &&
               Mathf.Abs(x.b - y.b) < tolerance;
    }

    public int GetHashCode(Color obj)
    {
        int r = Mathf.RoundToInt(obj.r / tolerance);
        int g = Mathf.RoundToInt(obj.g / tolerance);
        int b = Mathf.RoundToInt(obj.b / tolerance);
        return r * 73856093 ^ g * 19349663 ^ b * 83492791;
    }
}


