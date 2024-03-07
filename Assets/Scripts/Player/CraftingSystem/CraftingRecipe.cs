using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/NewRecipe",fileName = "NewRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [System.Serializable]
    public struct Ingredient
    {
        [SerializeField] public int AmountOfItem;
        [SerializeField] public Item NeededItem;
    }

    [SerializeField] Ingredient[] Ingredients;
    [SerializeField] Item Output;
    [SerializeField, Min(0)] int AmountOfOutputItems;
    [SerializeField, ShowOnly] int RecipeHash = 0;

    public int GetAmountOfOutputItems() => AmountOfOutputItems;
    public Item GetOutputItem() => Output;
    public Ingredient[] GetIngredients() => Ingredients;
    public int HashRecipe()
    {
        RecipeHash = 0;
        
        foreach (var Ingredient in Ingredients)
        {
            unchecked
            {
                RecipeHash += Ingredient.NeededItem.GetHashCode();
            }
        }

        return RecipeHash;
    }
}
