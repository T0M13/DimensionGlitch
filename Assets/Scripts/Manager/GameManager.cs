using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    [Header("PlayerSetup")]
    [SerializeField] PlayerController PlayerPrefab;
    [SerializeField] GameObject PlayerSpawnPosition;

    PlayerController GlobalPlayerControllerRef;
    [Header("References")]
    [SerializeField] private FragmentController fragmentController;
    [SerializeField] private VolumeManager volumeManager;
    public PlayerController GetPlayerControllerRef => GlobalPlayerControllerRef;

    public FragmentController FragmentController { get => fragmentController; set => fragmentController = value; }
    public VolumeManager VolumeManager { get => volumeManager; set => volumeManager = value; }


    private void OnValidate()
    {
        GetFragmentController();
        GetVolumeManager();
    }

    private void Awake()
    {
        GetFragmentController();
        GetVolumeManager();
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
        if (FragmentController == null)
            FragmentController = FindAnyObjectByType<FragmentController>();
    }

    private void GetVolumeManager()
    {
        if (VolumeManager == null)
            VolumeManager = FindAnyObjectByType<VolumeManager>();
    }
}
