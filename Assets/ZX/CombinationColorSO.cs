using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "Color/Combinations")]
public class CombinationColorSO : ScriptableObject
{
    [System.Serializable]
    public class IngredientData
    {
        [Tooltip("Sprite de la poción")]
        public Sprite sprite;

        [Tooltip("Color interno")]
        public Color colorValue;
    }
    public class Combination
    {
        [Tooltip("sprites + color interno")]
        //public List<Color> ingredients = new List<Color>();
        public List<IngredientData> ingredients = new List<IngredientData>();
        [Tooltip("Sprite resultado final")]
        public Sprite resultSprite;
        [Tooltip("Color resultante preview")]
        public Color resultColor;

        //public List<Color> resultColors = new List<Color>();
    }

    [Tooltip("Lista de combinaciones ")]
    public List<Combination> combinations = new List<Combination>();

    
}

