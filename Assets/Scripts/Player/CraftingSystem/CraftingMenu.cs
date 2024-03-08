using System.Collections.Generic;
using Manager;
using Player.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
   [SerializeField] Inventory CraftingInventory;
   [SerializeField] ItemDescription OutPutItemDescription;
   [SerializeField] Image OutputCraftingImage;
   [SerializeField] Sprite NoValidItemSprite;
   [SerializeField] List<CraftingRecipe> CraftingRecipes;

   Dictionary<int, CraftingRecipe> IngredientsMappedToRecipes = new();

   [Header("UIElements")] 
   [SerializeField] Button CraftButton;
   
   void OnEnable()
   {
      CraftButton.onClick.AddListener(CraftItem);
      CreateCraftingRecipeHashes();
   }

   private void Start()
   {
      InventoryContextManager.Instance.CreateContext(HUDManager.Instance.GetPlayerInventory(), CraftingInventory);
      OutputCraftingImage.sprite = NoValidItemSprite;
      BindToSlotEvents();
   }

   public void SetMenuActive(bool Active)
   {
      gameObject.SetActive(Active);
   }
   void BindToSlotEvents()
   {
      foreach (var InventorySlot in CraftingInventory.GetInventorySlots())
      {
         InventorySlot.OnItemDroppedIntoSlot += ShowOutputItem;
         InventorySlot.OnEmptySlot += ShowOutputItem;
      }
   }

   void ShowOutputItem(InventorySlot _)
   {
      if (TryGetMatchingRecipe(out CraftingRecipe FoundCraftingRecipe))
      {
         OutputCraftingImage.sprite = FoundCraftingRecipe.GetOutputItem().GetItemData().ItemSprite;
         OutPutItemDescription.SetItemDescription(FoundCraftingRecipe.GetOutputItem().GetItemData(), FoundCraftingRecipe.GetAmountOfOutputItems());
         OutPutItemDescription.SetDescriptionActive(true);
      }
      else
      {
         OutPutItemDescription.SetDescriptionActive(false);
         OutputCraftingImage.sprite = NoValidItemSprite;
      }
   }
   void CreateCraftingRecipeHashes()
   {
      foreach (var CraftingRecipe in CraftingRecipes)
      {
         int RecipeHash = CraftingRecipe.HashRecipe();
         IngredientsMappedToRecipes.Add(RecipeHash, CraftingRecipe);
      }
   }
   void OnDisable()
   {
      CraftButton.onClick.RemoveListener(CraftItem);
      InventoryContextManager.Instance.CloseContext();
   }

   void CraftItem()
   {
      if(!InventoryContextManager.Instance.HasValidContext()) return;
      
      if(TryGetMatchingRecipe(out CraftingRecipe FoundRecipe))
      {
         if (CanCraftItem(FoundRecipe))
         {
            foreach (var Ingredient in FoundRecipe.GetIngredients())
            {
               CraftingInventory.RemoveAmountOfItems(Ingredient.NeededItem.GetItemData().ItemID, Ingredient.AmountOfItem);
            }
            
            InventoryContextManager.Instance.GetCommunicatingInventory().TryAddItem(FoundRecipe.GetOutputItem(), FoundRecipe.GetAmountOfOutputItems());
         }
         else
         {
            Debug.Log("Not EnoughItems");
         }
      }
   }

   bool TryGetMatchingRecipe(out CraftingRecipe MatchingRecipe)
   {
      int CombinedItemHash = 0;
      MatchingRecipe = null;
      
      //Combine the hashes of all items in the crafting slots
      foreach (var CraftingSlot in CraftingInventory.GetInventorySlots())
      {
         if (!ItemDataBaseManager.Instance.IsNullItemOrInvalid(CraftingSlot.GetCurrentItem().ItemID))
         {
            unchecked
            {
               CombinedItemHash += ItemDataBaseManager.Instance.GetItemFromDataBase(CraftingSlot.GetCurrentItem().ItemID).GetHashCode();
            }
         }
      }

      //If we find a recipe that has a matching hashcode return this recipe
      if (IngredientsMappedToRecipes.TryGetValue(CombinedItemHash, out var Recipe))
      {
         MatchingRecipe = Recipe;
         return true;
      }

      return false;
   }
   bool CanCraftItem(CraftingRecipe CraftingRecipe)
   {
      bool CraftItem = false;
      //Loop through each ingredient in the crafting recipe 
      foreach (var Ingredient in CraftingRecipe.GetIngredients())
      {
         if (CraftingInventory.GetAmountOfItemInInventory(Ingredient.NeededItem.GetItemData().ItemID) >= Ingredient.AmountOfItem)
         {
            CraftItem = true;
         }
         else
         {
            CraftItem = false;
            return CraftItem;
         }
      }
      
      return CraftItem;
   }
}
