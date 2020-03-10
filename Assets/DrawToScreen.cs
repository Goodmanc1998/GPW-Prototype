﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

public class DrawToScreen : MonoBehaviour
{
    public Transform gesturePrefab;
    public Canvas drawCanvas;

    private List<Gesture> gestureCheckList = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    public Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;


    private bool recognized;
    private string message;
    void Start()
    {

        Debug.Log("datapath : " + Application.persistentDataPath);
        platform = Application.platform;
        drawArea = new Rect(0, 0, Screen.width, Screen.height);

        //Load pre-made gestures
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
        foreach (TextAsset gestureXml in gesturesXml)
            gestureCheckList.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        //Load user custom gestures
        //string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        //foreach (string filePath in filePaths)
        //    gestureCheckList.Add(GestureIO.ReadGestureFromFile(filePath));
    }

    void Update()
    {
        //cross compatbility is running windows android or iphone set the input to be correct style of input.
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
        
        // is the virtual keys in our draw area ?
        if (drawArea.Contains(virtualKeyPosition) && Input.GetKey(KeyCode.Q))
        {
            //if they are and the mouse left button is being pressed.
            if (Input.GetMouseButtonDown(0))
            {
                //if it been recognised then reset draw area
                if (recognized)
                {

                    recognized = false;
                    strokeId = -1;

                    points.Clear();
                    //for all the drawns in our list clear then list and destroy the drawn.
                    foreach (LineRenderer lineRenderer in gestureLinesRenderer)
                    {

                        lineRenderer.SetVertexCount(0);
                        Destroy(lineRenderer.gameObject);
                    }

                    gestureLinesRenderer.Clear();
                }

                ++strokeId;
                // if nothing has been drawn then start drawing.
                Transform currGesture = Instantiate(gesturePrefab, transform.position, transform.rotation) as Transform;
                //currGesture.SetParent(Camera.main.transform.GetChild(1).transform);
                //set the current drawn gesture to a variable.
                currentGestureLineRenderer = currGesture.GetComponent<LineRenderer>();
                //add the current drawn gesture to the list.
                gestureLinesRenderer.Add(currentGestureLineRenderer);

                vertexCount = 0;
            }
            // are we holding the mouse button down
            if (Input.GetMouseButton(0))
            {
                //Distance to the player
                float dis2Plyr = Vector2.Distance(Camera.main.transform.position, FindObjectOfType<PlayerMovement>().transform.position);
                //adding points as we draw.
                points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
                //and increase the drawn line vertex count to increase until drawn is complete.
                currentGestureLineRenderer.SetVertexCount(++vertexCount);
                // using the mouse postion and the camera to cast a ray to post point location
                Ray mouseRay = Camera.main.ScreenPointToRay(virtualKeyPosition);
                //set the gesture y height using done this way incase of player stading on object etc it will grab the mouse one in screen space then minus 13 give us middle of the player character.
                Vector2 yPos = Camera.main.ScreenToWorldPoint(virtualKeyPosition);
                currentGestureLineRenderer.SetPosition(vertexCount - 1, new Vector3(mouseRay.GetPoint(dis2Plyr).x,yPos.y - 20, mouseRay.GetPoint(dis2Plyr).z));
                
            }
            if(Input.GetMouseButtonUp(0))
            {
                Gesture candidate = new Gesture(points.ToArray());
                Result gestureResult = PointCloudRecognizer.Classify(candidate, gestureCheckList.ToArray());

                message = gestureResult.GestureClass + " " + gestureResult.Score;

                FindObjectOfType<PlayerMovement>().castSpell(currentGestureLineRenderer,gestureResult.GestureClass, gestureResult.Score);
                Debug.Log(message);


                recognized = true;
            }
            
        }
        else {
            recognized = false;
            strokeId = -1;

            points.Clear();
            //for all the drawns in our list clear then list and destroy the drawn.
            foreach (LineRenderer lineRenderer in gestureLinesRenderer)
            {

                lineRenderer.SetVertexCount(0);
                Destroy(lineRenderer.gameObject);
            }

            gestureLinesRenderer.Clear();
        }
    }

    void OnGUI()
    {
        // lables the draw area
        

    }
}
