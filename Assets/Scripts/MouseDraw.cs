using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDraw : MonoBehaviour
{
    //Storing the points and directions of the shapes
    List<Vector2> cubePoints = new List<Vector2>();
    List<Vector2> cubeDirections = new List<Vector2>();

    List<Vector2> trianglePoints = new List<Vector2>();
    List<Vector2> triangleDirections = new List<Vector2>();

    List<Vector2> rTrianglePoints = new List<Vector2>();
    List<Vector2> rTriangleDirections = new List<Vector2>();

    List<Vector2> lineDownDirections = new List<Vector2>();

    //Storing the players clicked position
    List<Vector2> playerPositions = new List<Vector2>();

    public GameObject UIObject;
    public Canvas UI;

    //Storing the range the players direction must be in
    [Tooltip("How Acurate the player must be")]
    public float range;
    //Storing the time to destory the UI draw positions
    [Tooltip("The length of time before the draw UI elements are removed")]
    public float destroyTimer;

    //Storing variables regarding checking if the player is still drawing
    [Tooltip("The length of time before the drawing is checked")]
    public float checkDrawingTimer;
    float drawingTimer;
    bool playerDrawing;



    // Start is called before the first frame update
    void Start()
    {

        //Creating and storing the points and directions

        cubePoints.Add(new Vector2(0, 0));
        cubePoints.Add(new Vector2(1,0));
        cubePoints.Add(new Vector2(1, -1));
        cubePoints.Add(new Vector2(0, -1));
        cubePoints.Add(new Vector2(0, 0));

        for (int i = 0; i < cubePoints.Count - 1; i++)
        {
            cubeDirections.Add((cubePoints[i + 1] - cubePoints[i]).normalized);
        }

        trianglePoints.Add(new Vector2(0, 0));
        trianglePoints.Add(new Vector2(2, 0));
        trianglePoints.Add(new Vector2(1, 1));
        trianglePoints.Add(new Vector2(0, 0));

        for (int i = 0; i < trianglePoints.Count - 1; i++)
        {
            triangleDirections.Add((trianglePoints[i + 1] - trianglePoints[i]).normalized);
        }

        rTrianglePoints.Add(new Vector2(0, 0));
        rTrianglePoints.Add(new Vector2(1, 0));
        rTrianglePoints.Add(new Vector2(1, 1));
        rTrianglePoints.Add(new Vector2(0, 0));

        for (int i = 0; i < rTrianglePoints.Count - 1; i++)
        {
            rTriangleDirections.Add((rTrianglePoints[i + 1] - rTrianglePoints[i]).normalized);
        }
    }

    // Update is called once per frame
    void Update()
    {
     
        //CHecking if the player presses the draw key
        if(Input.GetKeyDown(KeyCode.E))
        {
            //storing the players clicked position
            playerPositions.Add(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            GameObject newUIObject = Instantiate(UIObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y), Quaternion.identity);

            //UI drawing points
            newUIObject.transform.SetParent(UI.transform);
            Destroy(newUIObject, destroyTimer);

            //Starting the timer to check when finished drawing
            drawingTimer = checkDrawingTimer;
            playerDrawing = true;
        }

        //Timer to check if the player has stopped drawing
        if(drawingTimer <= 0.1f && playerDrawing == true)
        {
            CheckDrawing();
            playerDrawing = false;
        }
        else
        {
            drawingTimer -= Time.deltaTime;

        }
    }

    //Function used to check the drawing
    public void CheckDrawing()
    {
        bool squareDrawn = false;
        bool triangleDrawn = false;
        bool rTriangleDrawn = false;

        Debug.Log("Check Drawing");

        //Doing a check to see if the players draw points are equal to the amount of directions
        if (playerPositions.Count - 1 == cubeDirections.Count)
            squareDrawn = CheckSquare();

        if (playerPositions.Count - 1 == triangleDirections.Count)
            triangleDrawn = CheckTriangleIso();

        if (playerPositions.Count - 1 == rTriangleDirections.Count)
            rTriangleDrawn = CheckRTriangle();


        //To Do - Make function for these do do, double jump? 
        if (squareDrawn)
            Debug.Log("Square");

        if (triangleDrawn)
            Debug.Log("Triangle");

        if (rTriangleDrawn)
            Debug.Log("Right H triangle");

        if (!squareDrawn && !triangleDrawn && !rTriangleDrawn)
            Debug.Log("Draw Failed");

        //Clearing the players draw points
        playerPositions.Clear();

    }


    //Below Functions used to check the drawing to the stored directions for that shape
    public bool CheckSquare()
    {
        //Variables used to check if drawing is complete
        bool drawn = false;
        int count = 0;
        int playerCount = playerPositions.Count;

        for (int i = 0; i < playerPositions.Count - 1; i++)
        {
            //storing the players draw direction
            Vector2 playerDrawDirection = (playerPositions[i + 1] - playerPositions[i]).normalized;

            //Creating the min and max directions
            Vector2 minDirection = new Vector2(cubeDirections[i].x - range, cubeDirections[i].y - range);
            Vector2 maxDirection = new Vector2(cubeDirections[i].x + range, cubeDirections[i].y + range);

            //Checking if the players draw directions are within the limits
            if (playerDrawDirection.x >= minDirection.x && playerDrawDirection.y >= minDirection.y && playerDrawDirection.x <= maxDirection.x && playerDrawDirection.y <= maxDirection.y)
            {
                //Adding to the count of correct points
                count++;
            }

        }

        //if the correct points are equal to the amount of directions then this is drawn
        if (count == cubeDirections.Count)
            drawn = true;

        return drawn;
    }

    public bool CheckTriangleIso()
    {
        //Variables used to check if drawing is complete
        bool drawn = false;
        int count = 0;
        int playerCount = playerPositions.Count;

        for (int i = 0; i < playerPositions.Count - 1; i++)
        {
            //storing the players draw direction
            Vector2 playerDrawDirection = (playerPositions[i + 1] - playerPositions[i]).normalized;

            //Checking if the players draw directions are within the limits
            Vector2 minDirection = new Vector2(triangleDirections[i].x - range, triangleDirections[i].y - range);
            Vector2 maxDirection = new Vector2(triangleDirections[i].x + range, triangleDirections[i].y + range);

            //Checking if the players draw directions are within the limits
            if (playerDrawDirection.x >= minDirection.x && playerDrawDirection.y >= minDirection.y && playerDrawDirection.x <= maxDirection.x && playerDrawDirection.y <= maxDirection.y)
            {
                //Adding to the count of correct points
                count++;
            }
        }

        //if the correct points are equal to the amount of directions then this is drawn
        if (count == triangleDirections.Count)
            drawn = true;

        return drawn;
    }

    public bool CheckRTriangle()
    {
        //Variables used to check if drawing is complete
        bool drawn = false;
        int count = 0;
        int playerCount = playerPositions.Count;

        for (int i = 0; i < playerPositions.Count - 1; i++)
        {
            //storing the players draw direction
            Vector2 playerDrawDirection = (playerPositions[i + 1] - playerPositions[i]).normalized;

            //Checking if the players draw directions are within the limits
            Vector2 minDirection = new Vector2(rTriangleDirections[i].x - range, rTriangleDirections[i].y - range);
            Vector2 maxDirection = new Vector2(rTriangleDirections[i].x + range, rTriangleDirections[i].y + range);

            //Checking if the players draw directions are within the limits
            if (playerDrawDirection.x >= minDirection.x && playerDrawDirection.y >= minDirection.y && playerDrawDirection.x <= maxDirection.x && playerDrawDirection.y <= maxDirection.y)
            {
                //Adding to the count of correct points
                count++;
            }
        }

        //if the correct points are equal to the amount of directions then this is drawn
        if (count == rTriangleDirections.Count)
            drawn = true;

        return drawn;
    }

}
