using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
   [Header("PlayerSetup")]
   [SerializeField] PlayerController PlayerPrefab;
   [SerializeField] GameObject PlayerSpawnPosition;
   
   PlayerController GlobalPlayerControllerRef;

   PlayerController GetPlayerControllerRef => GlobalPlayerControllerRef;
   
   private void Start()
   {
      SpawnPlayer();
   }

   void SpawnPlayer()
   {
      GlobalPlayerControllerRef = Instantiate(PlayerPrefab, PlayerSpawnPosition.transform.position, PlayerPrefab.transform.rotation);
   }
}