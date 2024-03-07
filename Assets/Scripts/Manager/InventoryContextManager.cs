using UnityEngine;

namespace Manager
{
    public class InventoryContextManager : BaseSingleton<InventoryContextManager>
    {
        [SerializeField] Inventory CommunicatingInventory;
        [SerializeField] Inventory ContextInventory;
        
        public void CreateContext(Inventory CommunicatingInventory, Inventory ContextInventory)
        {
            this.CommunicatingInventory = CommunicatingInventory;
            this.ContextInventory = ContextInventory;
        }

        public void CloseContext()
        {
            CommunicatingInventory = null;
            ContextInventory = null;
        }
        
        public bool HasValidContext()
        {
            return ContextInventory && CommunicatingInventory;
        }
        
        public Inventory GetContextInventory()
        {
            return ContextInventory;
        }

        public Inventory GetCommunicatingInventory()
        {
            return CommunicatingInventory;
        }
    }
}