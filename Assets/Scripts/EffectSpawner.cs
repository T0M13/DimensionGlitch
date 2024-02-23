using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSpawner : BaseSingleton<EffectSpawner>
{
    [Header("Effects")]
    [SerializeField] private int amountEffect = 5;
    [SerializeField] private GameObject itemPickUpEffect;
    [SerializeField] private GameObject fragmentShiftEffect;
    [SerializeField] private List<GameObject> itemPickUpEffectList;
    [SerializeField] private List<GameObject> fragmentShiftEffectList;

    public GameObject ItemPickUpEffect { get => itemPickUpEffect; set => itemPickUpEffect = value; }

    private void Start()
    {
        InstantiateEffects(amountEffect, ItemPickUpEffect, itemPickUpEffectList);
        InstantiateEffects(amountEffect, fragmentShiftEffect, fragmentShiftEffectList);
    }

    public void InstantiateEffects(int amount, GameObject prefab, List<GameObject> list)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject effect = Instantiate(prefab);
            effect.SetActive(false);
            list.Add(effect);
        }
    }

    public void SpawnItemPickUpEffect(Transform positionGoal)
    {
        List<GameObject> activeEffects = itemPickUpEffectList.Where(effect => !effect.gameObject.activeSelf).ToList();
        var randEffect = activeEffects[Random.Range(0, itemPickUpEffectList.Count)];
        randEffect.transform.position = positionGoal.position;
        randEffect.SetActive(true);
    }

    public void SpawnFragmentShiftEffect(Transform positionGoal)
    {
        List<GameObject> activeEffects = itemPickUpEffectList.Where(effect => !effect.gameObject.activeSelf).ToList();
        var randEffect = activeEffects[Random.Range(0, itemPickUpEffectList.Count)];
        randEffect.transform.position = positionGoal.position;
        randEffect.SetActive(true);
    }

}
