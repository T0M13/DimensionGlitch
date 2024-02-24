using UnityEngine;

public class GameManager : BaseSingleton<GameManager>
{
    [Header("PlayerSetup")]
    [SerializeField] PlayerController PlayerPrefab;
    [SerializeField] HUDManager HudPrefab;
    [SerializeField] GameObject PlayerSpawnPosition;

    PlayerController GlobalPlayerControllerRef;
    HUDManager PlayerHud;
    
    [Header("References")]
    [SerializeField] private FragmentController fragmentController;
    [SerializeField] private VolumeManager volumeManager;

    [Header("Game Settings")]
    [SerializeField] private float defaultTimeFlow = 1f;

    public PlayerController GetPlayerControllerRef => GlobalPlayerControllerRef;
    public HUDManager GetPlayerHud => PlayerHud;
    protected override void Awake()
    {
        GetFragmentController();
        GetVolumeManager();
        SpawnPlayer();
        SpawnHud();
    }

    public FragmentController FragmentController { get => fragmentController; set => fragmentController = value; }
    public VolumeManager VolumeManager { get => volumeManager; set => volumeManager = value; }

    private void OnValidate()
    {
        GetFragmentController();
        GetVolumeManager();
    }
    void SpawnPlayer()
    {
        GlobalPlayerControllerRef = Instantiate(PlayerPrefab, PlayerSpawnPosition.transform.position, PlayerPrefab.transform.rotation);
        GlobalPlayerControllerRef.GetComponent<Stats>().OnDeath += OnPlayerDie;

        Vector3 PlayerPosition = GlobalPlayerControllerRef.transform.position;
        GlobalPlayerControllerRef.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y, 0);
    }

    void SpawnHud()
    {
        PlayerHud = Instantiate(HudPrefab, Vector2.zero, Quaternion.identity);
        
        Canvas Canvas =  PlayerHud.GetComponentInChildren<Canvas>();
        if (Canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Canvas.worldCamera = Camera.main;
        }
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

    public void ResetSettings()
    {
        //Time
        Time.timeScale = defaultTimeFlow;

    }

    public void SetGameTime(float multiplier)
    {
        Time.timeScale = 1 * multiplier;
    }


}
