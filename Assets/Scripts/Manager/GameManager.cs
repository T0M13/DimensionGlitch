using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
   [Header("PlayerSetup")]
   [SerializeField] PlayerController PlayerPrefab;
   [SerializeField] GameObject PlayerSpawnPosition;
   
   PlayerController GlobalPlayerControllerRef;

    public PlayerController GetPlayerControllerRef => GlobalPlayerControllerRef;
   
   private void Awake()
   {
      SpawnPlayer();
   }

   void SpawnPlayer()
   {
      GlobalPlayerControllerRef = Instantiate(PlayerPrefab, PlayerSpawnPosition.transform.position, PlayerPrefab.transform.rotation);
   }
}
