using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PsybersGestureRecognizer;

public class DrawToScreen : MonoBehaviour
{
    public Transform gesturePrefab;

    private List<Drawn> gestureCheckList = new List<Drawn>();

    private List<Vector> vectors = new List<Vector>();
    private int strokeNum = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    public Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;


    private bool recognized;
    private string message;

    public void clearPoint()
    {
        recognized = false;
        strokeNum = -1;

        vectors.Clear();
        //for all the drawns in our list clear then list and destroy the drawn.
        foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        {

            lineRenderer.SetVertexCount(0);
            Destroy(lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear();
    }
    void Start()
    {

        Debug.Log("datapath : " + Application.persistentDataPath); // path if you use demo to create defined gestures.
        platform = Application.platform;
        drawArea = new Rect(0, 0, Screen.width, Screen.height);

        //Load pre-made gestures
        TextAsset[] drawnsXml = Resources.LoadAll<TextAsset>("GestureSet/");
        foreach (TextAsset drawnXml in drawnsXml)
            gestureCheckList.Add(DrawReader.ReadDrawnFromXML(drawnXml.text));

        
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
                    strokeNum = -1;

                    vectors.Clear();
                    //for all the drawns in our list clear then list and destroy the drawn.
                    foreach (LineRenderer lineRenderer in gestureLinesRenderer)
                    {

                        lineRenderer.SetVertexCount(0);
                        Destroy(lineRenderer.gameObject);
                    }

                    gestureLinesRenderer.Clear();
                }

                ++strokeNum;
                // if nothing has been drawn then start drawing.
                Transform currGesture = Instantiate(gesturePrefab, transform.position, transform.rotation) as Transform;
                //set the current drawn gesture to a variable.
                currentGestureLineRenderer = currGesture.GetComponent<LineRenderer>();
                //add the current drawn gesture to the list.
                gestureLinesRenderer.Add(currentGestureLineRenderer);

                vertexCount = 0;
            }
            // are we holding the mouse button down
            if (Input.GetMouseButton(0))
            {
                //we add all the x and y positions and strokeID to points 
                vectors.Add(new Vector(virtualKeyPosition.x, -virtualKeyPosition.y, strokeNum));
                //and increase the drawn line vertex count to increase until drawn is complete.
                currentGestureLineRenderer.SetVertexCount(++vertexCount);
                currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
            }
            
                Drawn candidate = new Drawn(vectors.ToArray());
                RcnzdGesture gestureResult = VectorRcnzr.GetType(candidate, gestureCheckList.ToArray());

                message = gestureResult.GestureType + " " + gestureResult.PercentMatch;
            

                FindObjectOfType<PlayerMovement>().castSpell(gestureResult.GestureType, gestureResult.PercentMatch);
                
            
            recognized = true;

        }
        else {
            recognized = false;
            strokeNum = -1;

            vectors.Clear();
            //for all the drawns in our list clear then list and destroy the drawn.
            foreach (LineRenderer lineRenderer in gestureLinesRenderer)
            {

                lineRenderer.SetVertexCount(0);
                Destroy(lineRenderer.gameObject);
            }

            gestureLinesRenderer.Clear();
        }
    }

  
}
