using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Inventory
{
    public class ItemDescription : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ItemDescriptionText;
        [SerializeField] TextMeshProUGUI ItemName;
        [SerializeField] TextMeshProUGUI ItemAmount;
        [SerializeField] Canvas Canvas;
        
        private void OnEnable()
        {
            Canvas.worldCamera = Camera.main;
        }

        public void SetDescriptionActive(bool Active)
        {
            gameObject.SetActive(Active);           
        }
        public void SetItemDescription(ItemData ItemData, int Amount)
        {
            ItemName.SetText(ItemData.ItemName);
            ItemDescriptionText.SetText(ItemData.ItemDescription);
            ItemAmount.SetText(Amount.ToString());
        }
    }
}