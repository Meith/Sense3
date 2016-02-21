﻿using UnityEngine;
using System.Collections;

public class PlayerStateScript : MonoBehaviour {

    public string LossScenario_sceneName;
    public string WinScenario_sceneName;
    public int finalRunTime = 15;
    public GameObject playerAudio;

    private static int maxLevel;
    private static int playerLevel; // Changed it to static, might affect player sound levels.
    private bool finalState;
    private LevelLoader levelLoader;
    private GameObject player;


    // UI EVENTS
    public delegate void HUDeventHandler(string message);
    // All the functions that needs to be called when a sound is picked will be subscribed to this event
    public static event HUDeventHandler updateSoundPickup;

    //Initializing values that will be accessed from other scripts on awake
    void Awake()
    {
        if (playerAudio != null && (playerAudio.GetComponents<Layer>().Length > 0))
            maxLevel = playerAudio.GetComponents<Layer>().Length;
        else
            maxLevel = 7;

        //#--------------------------------------------------------------
        // Order of subscription is the order of execution of events. 
        // Putting it in Awake make sure these functions are given higher priority over other functions that are subscribed to this event in other classes
        //#--------------------------------------------------------------

        CoreSystem.onSoundEvent += incrementPlayerLevel;
        CoreSystem.onObstacleEvent += decrementPlayerLevel;
    }

    // Use this for initialization
    void Start () {
        playerLevel = 0;
      
        player = GameObject.FindWithTag("Player");
        if (player == null)
            Debug.Log("Cannot find object with tag player");

        finalState = false;
        levelLoader = gameObject.AddComponent<LevelLoader>();
       
	}


    void OnDisable()
    {
        CoreSystem.onSoundEvent -= incrementPlayerLevel;
        CoreSystem.onObstacleEvent -= decrementPlayerLevel;
    }

    private void checkState()
    {
        if (playerLevel >= maxLevel)
        {
            finalState = true;
            StartCoroutine(checkWin());
        }
        else if(playerLevel < 0)
        {
            Debug.Log("Loading :" + LossScenario_sceneName);
            levelLoader.LoadScene(LossScenario_sceneName);
        }
        else
        {
            finalState = false;
        }

    }

    public static int ReturnMaxPlayerLevel()
    {
        return maxLevel;
}

    private IEnumerator checkWin()
    {
        int i = 0;
        while( i < finalRunTime && finalState == true)
        {
            i++;
            yield return new WaitForSeconds(1.0f);
        }

        if(finalState)
        {
            Debug.Log("Loading :" + WinScenario_sceneName);
            levelLoader.LoadScene(WinScenario_sceneName);
        }
    }

    private void updatePlayerLevel()
    {
        //Debug.Log(playerLevel);
        checkState();
        if(updateSoundPickup != null)
            updateSoundPickup(playerLevel.ToString());

    }


    public void incrementPlayerLevel()
    {
        playerLevel++;
        updatePlayerLevel();
    }

    public void decrementPlayerLevel()
    {
        playerLevel--;
        updatePlayerLevel();
    }

    public static int getPlayerLevel()
    {
        return playerLevel;
    }

   	// Update is called once per frame
	void Update () {
	
	}
}
