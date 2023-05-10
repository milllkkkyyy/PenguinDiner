using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Food", order = 1)]
public class FoodData : ScriptableObject
{
    [SerializeField] string menuName;
    [SerializeField] Color color;

    public string GetMenuName()
    {
        return menuName;
    }

    public Color GetColor()
    {
        return color;
    }
}
