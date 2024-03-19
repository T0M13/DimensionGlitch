using UnityEngine;

namespace Building
{
    public abstract class Building : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField, Tooltip("Amount of cells in the Y Axis to lock")] int Height;
        [SerializeField, Tooltip("Amount of cells in the X Axis to lock")] int Width;

        public Vector2Int GetBuildingGrid() => new Vector2Int(Height, Width);
    }
}