using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "Color/Combinations")]
public class CombinationColorSO : ScriptableObject
{
    [System.Serializable]
    public class Combination
    {
        [Tooltip("colores ingredientes")]
        public List<Color> ingredients = new List<Color>();

        [Tooltip("Color resultante")]
        public Color resultColor;
    }

    [Tooltip("Lista de combinaciones ")]
    public List<Combination> combinations = new List<Combination>();
}
