using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Represents a mode for the palcement system 
    /// </summary>
    public abstract class Mode<T> : ScriptableObject
    {
        [SerializeField, ShowOnly] protected T ModeItem;
    
        //Here we can assign the item 
        public abstract void OnModeEntered(T ModeItem);
    
        //here we define what happens when we update the mode 
        public abstract void UpdateMode(PlacementSystem PlacementSystem);
    
        //here we say what happens when we exit the mode
        public abstract void OnModeExited();
    }
}
