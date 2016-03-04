﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIAnimation : MonoBehaviour {
    public GameObject player;
    public float speed;

    public List<GameObject> menuElements;
    public Vector3[] positions;
    public GameObject firstObject;
    public Vector3 temp;
    float time;
    public bool isRunning;
    // Use this for initialization
    void Start()
    {

    }
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

       if(Input.GetKeyDown(KeyCode.A))
        {
           
            StartCoroutine("MoveUILeft");
            isRunning = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            StartCoroutine("MoveUIRight");
            isRunning = true;
        }
    }

    IEnumerator MoveUILeft()
    {
        if (!isRunning)
        {
          
            time = 0;
            positions[0] = menuElements[0].transform.position;
            positions[1] = menuElements[1].transform.position;
            positions[2] = menuElements[2].transform.position;
            positions[3] = menuElements[3].transform.position;

            while (Vector3.Distance(menuElements[0].transform.position, positions[1]) > 0.1)
            {
                for (int i = 0; i < 4; i++)
                {
                    menuElements[i % 4].transform.position = Vector3.Lerp(menuElements[i % 4].transform.position, positions[(i + 1) % 4], 10*time * Time.deltaTime);
                }

                yield return null;  
            }
            isRunning = false;
        }
    }
    IEnumerator MoveUIRight()
    {
        if (!isRunning)
        {

            time = 0;
            positions[0] = menuElements[0].transform.position;
            positions[1] = menuElements[1].transform.position;
            positions[2] = menuElements[2].transform.position;
            positions[3] = menuElements[3].transform.position;

            while (Vector3.Distance(menuElements[1].transform.position, positions[0]) > 0.1)
            {
                for (int i = 4; i >0; i--)
                {
                    menuElements[i % 4].transform.position = Vector3.Lerp(menuElements[i % 4].transform.position, positions[(i - 1) % 4], 10 * time * Time.deltaTime);
                }

                yield return null;
            }
            isRunning = false;
        }
    }
}