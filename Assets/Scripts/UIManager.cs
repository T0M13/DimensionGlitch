using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;


    private void OnValidate()
    {
        GetGameManager();
    }

    private void Awake()
    {
        GetGameManager();
    }

    private void GetGameManager()
    {
        if (gameManager == null)
            gameManager = GameManager.Instance;
    }

}
