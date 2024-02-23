using System;
using System.Collections;
using UnityEngine;

public class PathPointFollower : MonoBehaviour
{
    [SerializeField] Transform[] PathPoints;
    [SerializeField] private float Speed = 0.0f;
    [SerializeField, Tooltip("if false then walk them in reverse")] bool LoopPaths;
    int CurrentPathPoint = 0;
    int Increment = 1;

    private void OnValidate()
    {
        if (LoopPaths)
        {
            PathPoints[GetPathPointsMaxIndex()].position = PathPoints[0].position;
        }

        transform.position = PathPoints[0].position;
    }

    private void OnEnable()
    {
        StartCoroutine(FollowPath());
    }

    private void Start()
    {
        StartCoroutine(FollowPath());
    }

    float GetDistanceToNextPoint()
    {
        Vector2 CurrentPointPosition = PathPoints[CurrentPathPoint].position;
        Vector2 NextPointPosition = PathPoints[CurrentPathPoint + Increment].position;

        return Vector2.Distance(CurrentPointPosition, NextPointPosition);
    }
    
    IEnumerator FollowPath()
    {
        float TimeNeededToReachPoint = GetDistanceToNextPoint() / Speed;
        float PassedTime = 0.0f;

        Vector2 CurrentPointPosition = PathPoints[CurrentPathPoint].position;
        Vector2 NextPointPosition = PathPoints[CurrentPathPoint + Increment].position;
        
        while (PassedTime < TimeNeededToReachPoint)
        {
            PassedTime += Time.deltaTime;

            Vector2 NewPosition = Vector2.Lerp(CurrentPointPosition, NextPointPosition, PassedTime / TimeNeededToReachPoint);
            transform.position = NewPosition;
            
            yield return null;
        }
        
        IncrementPathPoint();

        StartCoroutine(FollowPath());
    }

    void IncrementPathPoint()
    {
        bool IsAtLastPathpoint = CurrentPathPoint + Increment == GetPathPointsMaxIndex();
        bool IsAtFirstPathPoint = CurrentPathPoint + Increment == 0;
        //we arent at the last path point
        if (!IsAtLastPathpoint && !IsAtFirstPathPoint)
        {
            CurrentPathPoint += Increment;
        }
        //we are at the last path point
        else
        {
            if (LoopPaths)
            {
                CurrentPathPoint = 0;
            }
            else
            {
                Increment *= -1;

                CurrentPathPoint = IsAtFirstPathPoint ? 0 : GetPathPointsMaxIndex();
            }
        }
        
    }

    int GetPathPointsMaxIndex()
    {
        return PathPoints.Length - 1;
    }
    private void OnDrawGizmos()
    {
        if (PathPoints.Length <= 1) return;

        Gizmos.color = Color.green;
        
        for (int i = 0; i < PathPoints.Length - 1; i++)
        {
            Vector2 CurrentPointPosition = PathPoints[i].position;
            Vector2 NextPathPointPosition = PathPoints[i + 1].position;
            
            Gizmos.DrawLine(CurrentPointPosition, NextPathPointPosition);
        }
    }
}
