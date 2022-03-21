using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class which finds which objects are selected and locations to be dropped
public class LocationRetriever : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Raycast to get a clicked card
    public Transform GetTargetBody()
    {  //https://docs.unity3d.com/ScriptReference/RaycastHit-collider.html 
       //get the object to be dropped on
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        Transform targetBody = null;
        if (hit.transform != null)
        {
            targetBody = hit.transform;
        }
        return targetBody;
    }

    // gets the location where you are trying to place the card
    // returns null if not on a game object
    public Transform GetCardPlaceLocation(Transform clickedObject)
    {
        Transform cardsInStack;
        //makes sure the cards in the stack can't be raycast, so the object underneath can be selected
        for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
        {
            cardsInStack = clickedObject.parent.GetChild(numMoves);
            cardsInStack.gameObject.layer = 2;
        }

        //get where the card is trying to go
        Transform targetBody = GetTargetBody();

        //get object when hits something
        if (targetBody != null)
        {
            //TheLogger.PrintLog("we got the targetBody: " + targetBody.transform.parent.name);

            // return null if hits talonpile or stock pile
            if (targetBody.root.name.Equals("Deck"))
            {
                clickedObject.gameObject.layer = 0;
                return null;
            }
        }
        //hits nothing returns null
        else
        {
            clickedObject.gameObject.layer = 0;
            return null;
        }

        Transform targetLocation;
        //find out what it's hitting is it bottom or top
        if (targetBody.parent.name.Equals("Bottom"
            ) || targetBody.parent.name.Equals("Top"))
        {
            targetLocation = targetBody;
        }
        else
        {
            targetLocation = targetBody.parent;
        }
        for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
        {
            cardsInStack = clickedObject.parent.GetChild(numMoves);
            cardsInStack.gameObject.layer = 0;
        }

        return targetLocation;
    }
    public Transform DropLocation(Transform targetObject)
    {
        Transform targetPosition = targetObject;
        //this grabs the last child index numnber
        int lastChild;

        if (targetPosition.childCount - 1 < 0)
        {
            lastChild = 0;
        }
        else
        {
            lastChild = targetPosition.childCount - 1;
        }

        //this grabs the GameObject which is either the last child or the parent if there is no children
        Transform dropLocation;
        if (targetPosition.childCount == 0)
        {
            dropLocation = targetPosition;
        }
        else
        {

            dropLocation = targetPosition.GetChild(lastChild);
        }

        return dropLocation;
    }
}
