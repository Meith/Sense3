﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIAnimation : MonoBehaviour {
    public GameObject player;
    public float speed;
    public List<GameObject> menuElements;
    public Vector3[] positions;
    public Button startButton;
    public Button quitButton;
    public Button creditsButton;
    public Button tutorialsButton;
    public Button flowButton;
    public Vector3 temp;
    float time;
    public bool isRunning;
    public float smooth;

    private GameObject currentItem;
    // Use this for initialization
    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(() => { StartGame(); });

        if (quitButton != null)
            quitButton.onClick.AddListener(() => { QuitGame(); });

        if (creditsButton != null)
            creditsButton.onClick.AddListener(() => { LoadScene("Credits");});

        if (tutorialsButton != null)
            tutorialsButton.onClick.AddListener(() => { LoadScene("Tutorial"); });

        if (flowButton != null)
            flowButton.onClick.AddListener(() => { LoadScene("Oculus"); });
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0.0f)
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0.0f)
        {
            MoveRight();
        }

        if (Input.GetButton("Submit"))
        {
            foreach (GameObject menuItem in menuElements)
            {
                if (Mathf.Abs(menuItem.transform.position.z - 300.0f) <= 1.0f)
                {
                    currentItem = menuItem;
                    break;
                }
            }

            currentItem.GetComponentInChildren<Button>().onClick.Invoke();
        }

        time += Time.deltaTime;
        player.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.Self);
    }

    public void StartGame()
    {
        StartCoroutine("StartPlayerTakeOff");
      
    }
    public IEnumerator StartPlayerTakeOff()
    {   while (player.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) < 100)
        {
            player.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.Lerp(player.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0), 100, time * Time.deltaTime));
            yield return null;
            if (player.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) > 98)
                LoadScene("Infinite");

        }
    }
    public void MoveLeft()
    {
        StartCoroutine("MoveUIRight");

        isRunning = true;
    }
    public void MoveRight()
    {
        StartCoroutine("MoveUILeft");
        isRunning = true;
    }
    public IEnumerator MoveUILeft()
    {
        if (!isRunning)
        {
          
            time = 0;
            positions[0] = menuElements[0].transform.position;
            positions[1] = menuElements[1].transform.position;
            positions[2] = menuElements[2].transform.position;
            positions[3] = menuElements[3].transform.position;
            positions[4] = menuElements[4].transform.position;

            while (Vector3.Distance(menuElements[0].transform.position, positions[1]) > 0.001)
            {
                for (int i = 0; i < 5; i++)
                {
                    menuElements[i % 5].transform.position = Vector3.Lerp(menuElements[i % 5].transform.position, positions[(i + 1) % 5], 10*time * Time.deltaTime);
                }

                yield return null;  
            }
            isRunning = false;
        }
    }
    public IEnumerator MoveUIRight()
    {
        if (!isRunning)
        {

            time = 0;
            positions[0] = menuElements[0].transform.position;
            positions[1] = menuElements[1].transform.position;
            positions[2] = menuElements[2].transform.position;
            positions[3] = menuElements[3].transform.position;
            positions[4] = menuElements[4].transform.position;
            while (Vector3.Distance(menuElements[1].transform.position, positions[0]) > 0.001)
            {
                for (int i = 5; i >0; i--)
                {
                    menuElements[i % 5].transform.position = Vector3.Lerp(menuElements[i % 5].transform.position, positions[(i - 1) % 5], 10 * time * Time.deltaTime);
                }

                yield return null;
            }
            isRunning = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
