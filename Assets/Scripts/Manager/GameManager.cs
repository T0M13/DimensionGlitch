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
      GlobalPlayerControllerRef.GetComponent<Stats>().OnDeath += OnPlayerDie;
      
      Vector3 PlayerPosition = GlobalPlayerControllerRef.transform.position;
      GlobalPlayerControllerRef.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y, 0);
   }

   void OnPlayerDie()
   {
      //Implement respawn logic or deathscreen etc
      Debug.Log("Player died");
   }
}
