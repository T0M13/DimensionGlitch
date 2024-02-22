using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera Camera;
    [SerializeField] float RadiusToReachMaxFollowSpeed = 2;
    [SerializeField] float MaxFollowSpeed = 5;

    private float ZOffset = 0;
    PlayerController PlayerToFollow;
    private void Start()
    {
        ZOffset = Camera.transform.position.z;
        PlayerToFollow = GameManager.Instance.GetPlayerControllerRef;
    }
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector2 CameraPosition = Camera.transform.position;
        Vector2 PlayerPosition = PlayerToFollow.transform.position;
        
        float DistanceFromCircleCenter = Vector2.Distance(CameraPosition, PlayerPosition);

        float CurrentFollowSpeedMultiplier = Mathf.InverseLerp(0, RadiusToReachMaxFollowSpeed, DistanceFromCircleCenter);
        float CurrentFollowSpeed = CurrentFollowSpeedMultiplier * MaxFollowSpeed;

        Vector2 NewPosition = Vector2.Lerp(CameraPosition, PlayerPosition, Time.deltaTime * CurrentFollowSpeed);
        
        Camera.transform.position = NewPosition;
        Camera.transform.position += new Vector3(0, 0, ZOffset);
    }
}
