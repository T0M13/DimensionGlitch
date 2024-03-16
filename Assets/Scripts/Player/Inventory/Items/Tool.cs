using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Useable/Tool", fileName = "New Tool")]
public class Tool : UseableItem
{

    [SerializeField]
    private int damage;
    public int Damage => damage;


    [SerializeField] private ResourceType resourceType;

    public ResourceType ResourceType => resourceType;

    public override void OnUseItem(GameObject User)
    {
        PlacementSystem.Instance.EnterHarvestingMode(this);
    }
}
