using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject Inventory;
    public GameObject ItemPrefab;
    public static GameController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ActiveGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    
    public void DisableGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
}
