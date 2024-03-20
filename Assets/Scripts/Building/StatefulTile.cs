using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New StatefulTile", menuName = "Tiles/StatefulTile")]
public class StatefulTile : Tile
{
    [SerializeField] public bool CanBuildOn = false;
    [SerializeField] public bool IsArable = false;
    [SerializeField] public bool IsWater = false;
}
