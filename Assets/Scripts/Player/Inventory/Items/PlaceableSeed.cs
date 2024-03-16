using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Useable/PlaceableSeed", fileName = "PlaceableSeed")]
public class PlaceableSeed : UseableItem
{
    [SerializeField] Crop CropPrefabToPlace;

    public Crop GetCropPrefab() => CropPrefabToPlace;
    public override void OnUseItem(GameObject User)
    {
        //enter the planting mode
       PlacementSystem.Instance.EnterPlantingMode(this);
    }
}
