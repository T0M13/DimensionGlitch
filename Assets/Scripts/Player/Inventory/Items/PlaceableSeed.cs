using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Useable/PlaceableSeed", fileName = "PlaceableSeed")]
public class PlaceableSeed : UseableItem
{
    [SerializeField] GameObject CropPrefabToPlace;

    public GameObject CropPrefab() => CropPrefabToPlace;
    public override void OnUseItem(GameObject User)
    {
        //enter the farming mode
    }
}
