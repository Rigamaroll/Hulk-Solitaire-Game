using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour{
    Vector2 mousePosition;
    Vector3 cardOrigin; 
    GameObject clickedObject;
    GameObject targetObject;
    bool isDragged = false;
    Solitaire solitaire;
    GameObject dropLocation;
    
    // Start is called before the first frame update
    void Start(){
        solitaire = FindObjectOfType<Solitaire>();
    }

    // Update is called once per frame
    void Update(){
        if (isDragged){
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            clickedObject.transform.Translate(mousePosition);
        }
    }

    private void OnMouseDown(){
        //To store the mouse's position
        mousePosition = Camera.main.ScreenToWorldPoint(
                               new Vector2(Input.mousePosition.x,
                               Input.mousePosition.y));
        // https://docs.unity3d.com/ScriptReference/RaycastHit-collider.html 
        RaycastHit2D hit;

        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        // What we will do depends on what has been clicked
        if (hit){
            //get the object that is hit            
            clickedObject = hit.collider.gameObject;

        // check if card is face up 
        if(clickedObject.GetComponent<Selectable>().IsFaceUp()){
            isDragged = true;
            clickedObject.GetComponent<SpriteRenderer>().color = Color.grey;
        }

        //testing for card click
        TheLogger.PrintLog("card clicked");

        cardOrigin = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y, clickedObject.transform.position.z);
        }
    }

    private void OnMouseUp(){
        isDragged = false;
        clickedObject.GetComponent<SpriteRenderer>().color = Color.white;
        
        if (GetCardPlaceLocation() == null){
            clickedObject.transform.position = cardOrigin;
            return;
        }
        else{
            targetObject = GetCardPlaceLocation();
        }

        dropLocation = DropLocation();
        
        switch (clickedObject.transform.parent.gameObject.name){
            case "Top0":
            case "Top1":
            case "Top2":
            case "Top3":
                TheLogger.PrintLog("Got to Tops");
                Foundation();
                break;
            case "Bottom0":
            case "Bottom1":
            case "Bottom2":
            case "Bottom3":
            case "Bottom4":
            case "Bottom5":
            case "Bottom6":
                TheLogger.PrintLog("Got to Bottoms");
                Tableau();
                break;
            default:
                TheLogger.PrintLog("got to default");
                break;
        }
    }

    // This is where we will call the algorithm for if Deck is touched
    void StockPile(){
        List<GameObject> stockPile = solitaire.GetStockPileArray();
        // if (GameRules.IsEmpty(stockPile)){
        //     TheLogger.PrintLog("turn over Talons");
        // } else {
        //     TheLogger.PrintLog("Deal Card");
        //     GameObject nextCard = stockPile[stockPile.Count - 1];
        //     TheLogger.PrintLog(nextCard.name);
        //     stockPile.RemoveAt(stockPile.Count - 1);

        //     //add ^^Card to Talon Pile
        //     solitaire.SetStockPileArray(stockPile);
        // }
        
        /*isMouseClick(0) ?
        *  !isStockPileEmpty ? flipTopCard to Talon Pile : putTalonBackToStock 
        */
        print("Hit Deck");
        // Deal cards
        print("Deal 1 or 3 more cards");
    }

    // This is where we will call the algorithm for if a card is selected
    void TalonPile(){
        print("Hit Card");
        // select the card for moving somewhere
        // if double clicked, and can go to top spot, go there
        print("Pick this card up to move it somewhere");
        print("But if double clicked, and can go to top, move it there");
    }

    // Call algorithm for if top spot is selected
    void Foundation() {
        // Get the pile that card(s) was selected from
        GameObject parentStack = clickedObject.transform.parent.gameObject; 

        // Get index of card selected card
        int cardIndex = clickedObject.transform.GetSiblingIndex();
        // print("card Index of selected card is:" + cardIndex);

        // Get target stack
        GameObject targetStack;
        if ((targetObject.transform.parent.gameObject.name.Equals("Top"))||(targetObject.transform.parent.gameObject.name.Equals("Bottom"))){
            // Case where we have selected an empty pile
            targetStack = targetObject;
        } else {
            // Case where pile is not empty
            targetStack = targetObject.transform.parent.gameObject;
        }

        if (targetStack.transform.parent.gameObject.name.Equals("Bottom")){
            string stackCard = targetStack.transform.GetChild(targetStack.transform.childCount - 1).gameObject.name;

            // Case where we have selected an empty stack
            if (GameRules.IsEmpty(targetStack)){
                // print("Cannot move to an empty tableau stack");
                UpdateLocation(false, false); // return to origin
                return;
            }

            // Case where we have not selected an empty stack
            if (GameRules.IsAlternating(stackCard, clickedObject.name)){
                // print("Card colour is opposite to the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom")){
                    // print("Card rank is one less than target card, it can be moved.");
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                // print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            // print("Card colour is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
        }

        if (targetStack.transform.parent.gameObject.name.Equals("Top")){
            // Case where we have selected an empty pile
            if (GameRules.IsEmpty(targetStack)){
                // print("Check Rank, can only move an Ace to an empty foundation slot");
                if(GameRules.IsEmptyRank(clickedObject.name, "top")){
                    // print("Card is an Ace, movingto new slot.");
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                // print("Card is not an Ace, connot be put in an empty slot.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            // print("Cannot move between non-empty foundations.");
            UpdateLocation(false, false); // return to origin
            return;
        }
    }

    // Call algorithm for if bottom spot is selected
    void Tableau()
    {
        // Get the pile that card(s) was selected from
        GameObject parentStack = clickedObject.transform.parent.gameObject; 

        // Get index of card selected card
        int cardIndex = clickedObject.transform.GetSiblingIndex();
        // print("card Index of selected card is:" + cardIndex);

        // Get number of cards to see how many have been selected
        int numCards = clickedObject.transform.parent.gameObject.transform.childCount - cardIndex;
        // print("Number of cards selected is: " + numCards);

        // Get target stack
        GameObject targetStack;
        if ((targetObject.transform.parent.gameObject.name.Equals("Top"))||(targetObject.transform.parent.gameObject.name.Equals("Bottom"))){
            // Case where we have selected an empty pile
            targetStack = targetObject;
        } else {
            // Case where pile is not empty
            targetStack = targetObject.transform.parent.gameObject;
        }

        if (targetStack.transform.parent.gameObject.name.Equals("Bottom")){
            //print("We're moving the card to the bottom");

            // Case 1, new pile is empty, must be a king
            //print("Case 1 - Moving to empty stack");
            if (GameRules.IsEmpty(targetStack)){
                //print("Target stack is empty");
                if(GameRules.IsEmptyRank(clickedObject.name, "bottom")){
                    //print("Card is a King and can be moved");
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                //print("Card is not a King, cannot be moved here");
                UpdateLocation(false, false); // return to origin
                return;
            }

            //print("Case 2 - moving to a stack with cards");
            // Case 2 Moving onto a tableau pile alt colours and rank -1
            string stackCard = targetStack.transform.GetChild(targetStack.transform.childCount - 1).gameObject.name;
            if (GameRules.IsAlternating(stackCard, clickedObject.name)){
                //print("Card colour is opposite to the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom")){
                    //print("Card rank is one less than target card, it can be moved.");
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            // print("Card colour is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
        }

        if (targetStack.transform.parent.gameObject.name.Equals("Top")){
            // Make sure only one card is selected
            if (numCards != 1){
                // print("Can only move one card to the top at a time.");
                UpdateLocation(false, false); // return to origin
                return;
            }

            // Case 1: Foundation Pile is empty
            if (GameRules.IsEmpty(targetStack)){
                // print("Target stack is empty");
                if(GameRules.IsEmptyRank(clickedObject.name, "top")){
                    // print("Card is an Ace and can be moved");
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                // print("Card is not an Ace, cannot be moved here");
                UpdateLocation(false, false); // return to origin
                return;
            }

            // Case 2: Foundation Pile is not empty
            print("Case 2 - moving to a stack with cards");
            // Case 2 Moving onto a foundation pile same suit and rank +1
            string stackCard = targetStack.transform.GetChild(targetStack.transform.childCount - 1).gameObject.name;
            if (GameRules.IsSameSuit(stackCard, clickedObject.name)){
                // print("Card suit is same as the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "top")){
                    // print("Card rank is one more than target card, it can be moved.");
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                // print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            // print("Card suit is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
        }
        print("This is the bad place");
    }

    private GameObject GetCardPlaceLocation(){
        clickedObject.layer = 2;
        RaycastHit2D hit;
        Rigidbody2D targetBody = null;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null){
            targetBody = hit.transform.GetComponent<Rigidbody2D>();
            TheLogger.PrintLog("we got the targetBody");
        } else {
            clickedObject.layer = 0;
            return null;
        }

        GameObject targetLocation;

        if (targetBody.transform.gameObject.transform.parent.gameObject.name.Equals("Bottom"
            ) || targetBody.transform.gameObject.transform.parent.gameObject.name.Equals("Top")){
            targetLocation = targetBody.gameObject;
        } else {
            targetLocation = targetBody.gameObject.transform.parent.gameObject;
        }
        
        clickedObject.layer = 0;
        return targetLocation;
    }

    public GameObject DropLocation(){
        Transform targetPosition = targetObject.transform;
        // this grabs the last child index numnber
        int lastChild;

        if (targetPosition.childCount -1 < 0){
            lastChild = 0;
        } else {
            lastChild = targetPosition.childCount - 1;
        }

        // this grabs the GameObject which is either the last child or the parent if there is no children
        GameObject dropLocation;
        if (targetPosition.childCount == 0){
            dropLocation = targetPosition.gameObject;
        }else{
            dropLocation = targetPosition.GetChild(lastChild).transform.gameObject;
        }
       
        return dropLocation;
    } 

    private void UpdateLocation(bool isMoving, bool isTop){
        if (isMoving){
            if (isTop){
                // Foundation: offset the z-index only
                clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                dropLocation.transform.position.y, dropLocation.transform.position.z - 0.03f);
            }else{
                // Tableau: offset y and z-index
                float yOffSet;
                if (targetObject.transform.childCount != 0)
                {
                     yOffSet = 0.40f;
                }
                else
                {
                    yOffSet = 0.0f;
                }
                clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                dropLocation.transform.position.y - yOffSet, dropLocation.transform.position.z - 0.03f);
            }

        }else{
            print("Card cannot move there!");
            // return the card to its original location
            clickedObject.transform.position = cardOrigin;
        }
    }

    private void UpdateGameObjects(int cardIndex, int numCards)
    {
        print("Inside update Game Objects");
        // Get the target pile type
        GameObject ts;
        // Check to see if the game object is a card, or an empty spot
        if ((targetObject.transform.parent.gameObject.name.Equals("Top")) || (targetObject.transform.parent.gameObject.name.Equals("Bottom")))
        {
            // Case where we have selected an empty spot
            ts = targetObject.transform.gameObject;
        }
        else
        {
            // Case where pile is not empty, want to get parent pile
            ts = targetObject.transform.parent.gameObject;
        }
        print("ts is : " + ts.name);

        // Get parent pile 
        print("clicked object is: " + clickedObject.transform.name);
        print("clicked object's parent is: " + clickedObject.transform.parent.gameObject.name);
        GameObject ps = clickedObject.transform.parent.gameObject;
        print("ps is: " + ps.name);

        // Parent Game Object
        GameObject mom = ps;

        print("Number of cards is: " + numCards);
        // if the card can go in the location selected remove it from the pile
        for (int i = 0; i < numCards;  i++)
        {
            print("card index is: " + cardIndex);
            // print("moving card: " + ps.transform.GetChild(cardIndex).name);
            ps.transform.GetChild(cardIndex).SetParent(ts.transform);
        }

        // print("is not faceup: " + !mom.transform.GetChild(mom.transform.childCount - 1).GetComponent<Selectable>().IsFaceUp());
        if (mom.transform.childCount == 0)
        {
            print("No cards to flip!");
            return;
        }
        if (!mom.transform.GetChild(mom.transform.childCount - 1).GetComponent<Selectable>().IsFaceUp())
        {
            print("Next card should flip!");
            mom.transform.GetChild(mom.transform.childCount - 1).GetComponent<Selectable>().FlipCard();
        }
    }
}
