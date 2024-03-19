using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New StatefulTile", menuName = "Tiles/StatefulTile")]
public class StatefulTile : Tile
{
    [SerializeField] public bool CanBuildOn;
    [SerializeField] public bool IsArable;
}
