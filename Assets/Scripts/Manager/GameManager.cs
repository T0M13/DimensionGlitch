using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    [Header("PlayerSetup")]
    [SerializeField] PlayerController PlayerPrefab;
    [SerializeField] GameObject PlayerSpawnPosition;

    PlayerController GlobalPlayerControllerRef;

    public PlayerController GetPlayerControllerRef => GlobalPlayerControllerRef;

    [Header("References")]
    [SerializeField] private FragmentController fragmentController;

    private void OnValidate()
    {
        GetFragmentController();
    }

    private void Awake()
    {
        GetFragmentController();
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

    private void GetFragmentController()
    {
        if (fragmentController == null)
            fragmentController = FindAnyObjectByType<FragmentController>();
    }
}
