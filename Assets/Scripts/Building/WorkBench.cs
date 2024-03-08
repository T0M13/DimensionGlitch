using System.Collections;
using Interfaces;
using UnityEngine;

namespace Building
{
    public class WorkBench : Building, IInteractable
    {
        [SerializeField] CraftingMenu CraftingMenuToOpen;
        [SerializeField] float MaxUseRange = 5;

        GameObject User = null;
        public void OnInteract(GameObject Interactor)
        {
            //if we interact again the previous check routine should be stopped
            StopAllCoroutines();
            
            Debug.Log("Interacted with a workbench");
            CraftingMenuToOpen.SetMenuActive(true);
            User = Interactor;

            StartCoroutine(CheckForInteractorInRange());
        }

        IEnumerator CheckForInteractorInRange()
        {
            while (Vector2.Distance(transform.position, User.transform.position) < MaxUseRange)
            {
                yield return null;
            }
            
            CraftingMenuToOpen.SetMenuActive(false);
            User = null;
        }
    }
}