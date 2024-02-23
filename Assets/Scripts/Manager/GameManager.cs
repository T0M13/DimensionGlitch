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
    [SerializeField] private UIManager uiManager;
    public PlayerController GetPlayerControllerRef => GlobalPlayerControllerRef;
    protected override void Awake()
    {
        GetFragmentController();
        GetVolumeManager();
         GetUIManager();
        SpawnPlayer();
    }

    public FragmentController FragmentController { get => fragmentController; set => fragmentController = value; }
    public VolumeManager VolumeManager { get => volumeManager; set => volumeManager = value; }
    public UIManager UiManager { get => uiManager; set => uiManager = value; }

    private void OnValidate()
    {
        GetFragmentController();
        GetVolumeManager();
        GetUIManager();
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

    private void GetUIManager()
    {
        if (UiManager == null)
            UiManager = FindAnyObjectByType<UIManager>();
    }
}
