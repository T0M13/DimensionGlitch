using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSpawner : MonoBehaviour 
{
    [Header("Effects")]
    [SerializeField] private int amountEffect = 5;
    [SerializeField] private GameObject itemPickUpEffect;
    [SerializeField] private List<GameObject> itemPickUpEffectList;

    private void Start()
    {
        for (int i = 0; i < amountEffect; i++)
        {
            GameObject effect = Instantiate(itemPickUpEffect);
            effect.SetActive(false);
            itemPickUpEffectList.Add(effect);
        }
    }

    public void SpawnEffect(Transform positionGoal, GameObject effect)
    {

        List<GameObject> activeEffects = itemPickUpEffectList.Where(effect => effect.gameObject.activeSelf).ToList();

    }

}
