using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{//Mouse starting and current position
    Vector3 leftMouseStart;
    Vector3 leftMouseCurrent;

    Vector3 rightMouseStart;
    Vector3 rightMouseCurrent;
    //Checking if the mouse button is being held down
    bool leftMouseHold;
    bool rightMouseHold;

    Player playerMovement;

    //Related to free flowing controls
    //The minimum and maximum distance the mouse can move
    [Tooltip("The maximum distance the mouse can move")]
    public float maxMouseMoveDistance;

    [Tooltip("The minimum distance the mouse can move")]
    public float minMouseMoveDistance;

    //Related to constrained controls
    //The vector2 strong contrains for mouse position
    public Vector2 constraint;

    [Tooltip("Used to control the speed of the character")]

    public int distanceDevide;

    //The UI dot that spawns
    public GameObject UIobject;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the left mouse button is being held down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            leftMouseHold = true;
            leftMouseStart = Input.mousePosition;
        }

        //Checking when the left mouse button is released
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            leftMouseHold = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            rightMouseStart = Input.mousePosition;
            rightMouseHold = true;

            playerMovement.playerJump();

        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            rightMouseCurrent = Input.mousePosition;
            rightMouseHold = false;
        }



        //Is active while the mouse is being held down
        if (leftMouseHold)
        {
            leftMouseCurrent = Input.mousePosition;
            MouseCheck(leftMouseStart, leftMouseCurrent);
            UIMousePosition();
        }
        else
            UIobject.SetActive(false);

        if (rightMouseHold)
        {

        }

    }

    void MouseCheck(Vector3 start, Vector3 release)
    {
        //Getting the direction of the mouses current position to the starting position
        Vector2 direction = (release - start).normalized;
        //Getting the distance from the current mouse position from the starting position
        Vector2 distance = new Vector2((release.x - start.x), (release.y - start.y));

        float totalDistance = Vector3.Distance(start, release);

        distance /= distanceDevide;

        //Checks for min and max movement
        if (totalDistance > maxMouseMoveDistance)
            totalDistance = maxMouseMoveDistance;

        if (totalDistance < minMouseMoveDistance)
            return;

        if (distance.x > maxMouseMoveDistance)
            distance.x = maxMouseMoveDistance;

        if (distance.y > maxMouseMoveDistance)
            distance.y = maxMouseMoveDistance;

        if (distance.x < -maxMouseMoveDistance)
            distance.x = -maxMouseMoveDistance;

        if (distance.y < -maxMouseMoveDistance)
            distance.y = -maxMouseMoveDistance;

        //Mouse Up
        if (direction.y > constraint.y)
        {
            playerMovement.GetDirection(distance, direction);
        }

        //Turn Right
        if (direction.x > constraint.x)
        {
            playerMovement.GetDirection(distance, direction);
        }

        //Turn Left
        if (direction.x < -constraint.x)
        {
            playerMovement.GetDirection(distance, direction);
        }

        //Mouse Down
        if (direction.y < -constraint.y)
        {
            playerMovement.GetDirection(distance, direction);
        }

    }

    //Used to create the UI dot
    public void UIMousePosition()
    {
        if (leftMouseHold)
        {
            UIobject.SetActive(true);

            UIobject.transform.position = leftMouseStart;
        }
        
    }
}
