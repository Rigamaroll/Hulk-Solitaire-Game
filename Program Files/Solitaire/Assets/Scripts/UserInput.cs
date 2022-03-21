using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour{
    Vector2 mousePosition;
    Vector3 cardOrigin; 
    Transform clickedObject;
    Transform targetObject;
    bool isDragged = false;
    bool isStockpileCard = false;
    Transform dropLocation;
    LocationRetriever locationRetriever;
    
    //Start is called before the first frame update
    void Start(){
        Scoring.instance.ResetScore(); // Resets the score whenever User starts a new game
        locationRetriever = FindObjectOfType<LocationRetriever>();
    }

    //Update is called once per frame
    void Update(){
        if (isDragged){
            //checks the mousePosition for moving the cards to that spot
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            CardMove();        
        }
    }

    private void OnMouseDown(){
        //get what's been clicked
        clickedObject = locationRetriever.GetTargetBody();
        //What we will do depends on what has been clicked
        if (clickedObject != null){
            //get the object that is hit            
            cardOrigin = new Vector3(clickedObject.position.x, clickedObject.position.y, clickedObject.position.z);
            //checks if the stockpile has been hit (so that is will deal)
            if (clickedObject.parent.name.Equals("DeckButton") || clickedObject.name.Equals("DeckButton"))
            {
                isStockpileCard = true;
                FindObjectOfType<DeckArea>().StockPile(clickedObject);
                //TheLogger.PrintLog("Hit Stockpile");
                return;          
            }
            
        //check if card is face up 
        if(clickedObject.GetComponent<Selectable>().IsFaceUp() && !isStockpileCard){
            float zOffSet = -0.54f;
            isDragged = true;
            Transform cardsInStack;
            clickedObject.GetComponent<SpriteRenderer>().color = Color.grey;
                //Grabs the stack of cards and moves there zOffSet to the appropriate location, so they are in front of all the other cards
                for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
                {
                    cardsInStack = clickedObject.parent.GetChild(numMoves);
                    cardsInStack.position = new Vector3(cardsInStack.position.x, cardsInStack.position.y, zOffSet);
                    zOffSet -= 0.03f;       

                }
            }

        //testing for card click
        //TheLogger.PrintLog("card clicked");

        }
    }

    private void OnMouseUp(){
        //this means the card goes to the talonpile
        if (isStockpileCard)
        {
            isStockpileCard = false;
            return;
        }
        isDragged = false;
        clickedObject.GetComponent<SpriteRenderer>().color = Color.white;
        
        //if location dropped is green felt
        if (locationRetriever.GetCardPlaceLocation(clickedObject) == null){
            //is the object in a stack, returns to origin
            if (IsStack())
            {
                Transform cardsInStack;
                float stackYoffSet = 0.00f;
                float stackZoffSet = 0.00f;
                //check all cards in stack
                for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
                {
                    cardsInStack = clickedObject.parent.GetChild(numMoves);
                    cardsInStack.position = new Vector3(cardOrigin.x,
                    cardOrigin.y - stackYoffSet, cardOrigin.z - stackZoffSet);
                    stackYoffSet += 0.40f;
                    stackZoffSet += 0.03f;
                }
            }
            else
            //return single card to origin
            {
                clickedObject.position = cardOrigin;
            }

            return;
        }
        //move to new location
        else{
            targetObject = locationRetriever.GetCardPlaceLocation(clickedObject);
        }

        dropLocation = locationRetriever.DropLocation(targetObject);
        
        //checks where the card was clicked and runs the appropriate method
        switch (clickedObject.root.name){
          
            case "Top":
                Foundation();
                break;
            case "Bottom":
                Tableau();
                break;
            case "Deck":
                Talon();
                break;
            default:
                break;
        }
    }

    //Checks if the clicked card is in a stack
    private bool IsStack()
    {
        return ((clickedObject.GetSiblingIndex() < clickedObject.parent.childCount - 1)
            && (clickedObject.GetComponent<Selectable>().IsFaceUp()));

    }

    //checks if more than one card being moved at a time.
    void CardMove()
    {
        // if it is a group of cards
        if (IsStack())
        {
            Transform cardsInStack;

            //moves each card at a time in this for loop
            for (int numMoves = clickedObject.transform.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
            {
                cardsInStack = clickedObject.parent.GetChild(numMoves);
                cardsInStack.Translate(mousePosition);

            }
        }
        //if only 1 card to move do this
        else
        {
            clickedObject.Translate(mousePosition);
        }
    }
    
    //Call algorithm for if top spot is selected
    void Talon()
    {

        //Get index of card selected card
        int cardIndex = clickedObject.GetSiblingIndex();
        //print("card Index of selected card is:" + cardIndex);

        //Get target stack
        Transform targetStack;
        print("target object is: " + targetObject.name);
        if (targetObject.root.name.Equals("Deck"))
        {   
            //tries to move onto the stockpile or talon pile
            CardToOrigin(); // return to origin
            return;
        }
        if ((targetObject.parent.name.Equals("Top")) || (targetObject.parent.name.Equals("Bottom")))
        {
            //Case where we have selected an empty pile
            targetStack = targetObject;
        }
        else
        {
            //Case where pile is not empty
            targetStack = targetObject.parent;
        }

        if (targetStack.parent.name.Equals("Bottom"))
        {
            //Case 1, new pile is empty, must be a king
            print("Case 1 - Moving to empty stack");
            if (GameRules.IsEmpty(targetStack)){
                print("Target stack is empty");
                if(GameRules.IsEmptyRank(clickedObject.name, "bottom")){
                    print("Card is a King and can be moved");
                    CardToTableau(); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    // Add 5 points for moving to the tableau from the Talon
                    Scoring.instance.AddScore(1);
                    return;
                }
                print("Card is not a King, cannot be moved here");
                CardToOrigin(); // return to origin
                return;
            }

            string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;

            //Case where we have not selected an empty stack
            if (GameRules.IsAlternating(stackCard, clickedObject.name))
            {
                //print("Card colour is opposite to the target card");
                if (GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom"))
                {
                    //print("Card rank is one less than target card, it can be moved.");
                    CardToTableau(); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    // Add 5 points for moving to the tableau from the Talon
                    Scoring.instance.AddScore(1);
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                CardToOrigin(); // return to origin
                return;
            }
            //print("Card colour is incorrect and cannot be moved.");
            CardToOrigin(); // return to origin
            return;
        }

        if (targetStack.parent.name.Equals("Top"))
        {
            //Case where we have selected an empty pile
            if (GameRules.IsEmpty(targetStack))
            {
                //print("Check Rank, can only move an Ace to an empty foundation slot");
                if (GameRules.IsEmptyRank(clickedObject.name, "top"))
                {
                    //print("Card is an Ace, movingto new slot.");
                    CardToFoundation(); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                //print("Card is not an Ace, connot be put in an empty slot.");
                CardToOrigin(); // return to origin
                return;
            }

            //Case 2: Foundation Pile is not empty
            //print("Case 2 - moving to a stack with cards");
            //Case 2 Moving onto a foundation pile same suit and rank +1
            string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
            if (GameRules.IsSameSuit(stackCard, clickedObject.name))
            {
                //print("Card suit is same as the target card");
                if (GameRules.IsRankGoood(stackCard, clickedObject.name, "top"))
                {
                    //print("Card rank is one more than target card, it can be moved.");
                    CardToFoundation(); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                CardToOrigin(); // return to origin
                return;
            }
            //print("Card suit is incorrect and cannot be moved.");
            CardToOrigin(); // return to origin
            return;
            //print("Cannot move between non-empty foundations.");
            
        }
    }

    //Call algorithm for if top spot is selected
    void Foundation() {
        
        //Get index of card selected card
        int cardIndex = clickedObject.GetSiblingIndex();
        //print("card Index of selected card is:" + cardIndex);

        //Get target stack
        Transform targetStack;
        if ((targetObject.parent.name.Equals("Top"))||(targetObject.parent.name.Equals("Bottom"))){
            //Case where we have selected an empty pile
            targetStack = targetObject;
        } else {
            //Case where pile is not empty
            targetStack = targetObject.parent;
        }

        if (targetStack.parent.name.Equals("Bottom") && !GameRules.IsEmpty(targetStack)) {

            string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
            //Case where we have not selected an empty stack
            if (GameRules.IsAlternating(stackCard, clickedObject.name)){
                //print("Card colour is opposite to the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom")){
                    //print("Card rank is one less than target card, it can be moved.");
                    CardToTableau(); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    // Card moving from Foundation To Tableau Piles. Remove Points
                    Scoring.instance.ReduceScore(3);
                    return;
                }
            }
        }

        if (targetStack.parent.name.Equals("Top")){
            //Case where we have selected an empty pile
            if (GameRules.IsEmpty(targetStack)){
                //print("Check Rank, can only move an Ace to an empty foundation slot");
                if(GameRules.IsEmptyRank(clickedObject.name, "top")){
                    //print("Card is an Ace, movingto new slot.");
                    CardToFoundation(); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
            }          
        }
        //print("Card can't be placed there.");
        CardToOrigin(); // return to origin
    }

    //Call algorithm for if bottom spot is selected
    void Tableau()
    {
        //Get target stack which is where we're placing the card
        Transform targetStack;
        if ((targetObject.parent.name.Equals("Top"))||(targetObject.parent.name.Equals("Bottom"))){
            //Case where we have selected an empty pile
            targetStack = targetObject;
        } else {
            //Case where pile is not empty
            targetStack = targetObject.parent;
        }
        //check to see if top or bottom
        switch(targetStack.parent.name)
        {
            case "Bottom":
                TableauBottom(targetStack);
                break;
            case "Top":
                TableauTop(targetStack);
                break;
        }
      
        //print("This is the bad place");
    }

    private void TableauBottom(Transform targetStack)
    {
        //Get index of selected card
        int cardIndex = clickedObject.GetSiblingIndex();
        //print("card Index of selected card is:" + cardIndex);

        //Get number of cards to see how many have been selected
        int numCards = clickedObject.parent.transform.childCount - cardIndex;

        //check if Stack is empty
        switch (GameRules.IsEmpty(targetStack))
        {
            //Case 1, new pile is empty, must be a king
            case true:

                //print("Target stack is empty");
                if (GameRules.IsEmptyRank(clickedObject.name, "bottom"))
                {
                    //print("Card is a King and can be moved");
                    CardToTableau(); //move, not to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }            
                break;

            case false:

                print("Case 2 - moving to a stack with cards");
                //Case 2 Moving onto a tableau pile alt colours and rank -1
                string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
                if (GameRules.IsAlternating(stackCard, clickedObject.name))
                {
                    print("Card colour is opposite to the target card");
                    if (GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom"))
                    {
                        //print("clickedObject's name is " + clickedObject.name);
                        //print("Card rank is one less than target card, it can be moved.");
                        CardToTableau(); //move, not to the top
                        UpdateGameObjects(cardIndex, numCards);
                        return;
                    }
                }             
                break;
        }
        CardToOrigin(); // return to origin
    }

    private void TableauTop(Transform targetStack)
    {
        //Get index of selected card
        int cardIndex = clickedObject.GetSiblingIndex();
        //print("card Index of selected card is:" + cardIndex);

        //Get number of cards to see how many have been selected
        int numCards = clickedObject.parent.transform.childCount - cardIndex;
        //Make sure only one card is selected
        if (numCards != 1)
        {
            //print("Can only move one card to the top at a time.");
            CardToOrigin(); // return to origin
            return;
        }

        switch(GameRules.IsEmpty(targetStack))
        {
            // Case 1: Foundation Pile is empty
            case true:
                //print("Target stack is empty");
                if (GameRules.IsEmptyRank(clickedObject.name, "top"))
                {
                    //print("Card is an Ace and can be moved");
                    CardToFoundation(); //move, to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                //print("Card is not an Ace, cannot be moved here");
                break;

            //Case 2: Foundation Pile is not empty
            case false:
                string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
                if (GameRules.IsSameSuit(stackCard, clickedObject.name))
                {
                    //print("Card suit is same as the target card");
                    if (GameRules.IsRankGoood(stackCard, clickedObject.name, "top"))
                    {
                        //Case 2 Moving onto a foundation pile same suit and rank +1
                        //print("Card rank is one more than target card, it can be moved.");
                        CardToFoundation(); //move, to the top
                        UpdateGameObjects(cardIndex, numCards);
                        return;
                    }
                }
                break;
        }
        CardToOrigin();//return to origin 
    }

    //Move card to tableau
    private void CardToTableau()
    {
        //Tableau: offset y and z-index
        float yOffSet;
        if (targetObject.childCount != 0)
        {
            yOffSet = 0.40f;
        }
        else
        {
            yOffSet = 0.0f;
        }
        //checks if the selected card is the last card in the stack.  If not it gets the totals amount of cards and places all of them.
        if (IsStack())
        {
            float stackYoffSet;
            Transform cardsInStack;
            print("targetObject is " + targetObject.name);
            print("targetObject childcount is  " + targetObject.childCount);

            if (targetObject.childCount == 0)
            {
                print("Got to childCount 0");
                stackYoffSet = 0.0f;
            }
            else
            {
                print("Got to childCount not 0");
                stackYoffSet = 0.40f;
            }
            float stackZoffSet = 0.03f;
            for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
            {
                cardsInStack = clickedObject.parent.GetChild(numMoves);
                cardsInStack.position = new Vector3(dropLocation.position.x,
                    dropLocation.position.y - stackYoffSet, dropLocation.position.z - stackZoffSet);
                stackYoffSet += 0.40f;
                stackZoffSet += 0.03f;
            }
        }
        else
        {
            clickedObject.position = new Vector3(dropLocation.position.x,
            dropLocation.position.y - yOffSet, dropLocation.position.z - 0.03f);
        }
    }

    //move card to foundation
    private void CardToFoundation()
    {
        //Foundation: offset the z-index only
        clickedObject.position = new Vector3(dropLocation.position.x,
            dropLocation.position.y, dropLocation.position.z - 0.03f);
        // Card moves to the top + 10 points
        Scoring.instance.AddScore(2);
    }

    //returns cards to origin
    private void CardToOrigin()
    {
        print("Card cannot move there!");
        //return the card to its original location
        if (IsStack())
        {
            Transform cardsInStack;
            float stackYoffSet = 0.00f;
            float stackZoffSet = 0.00f;
            for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
            {
                cardsInStack = clickedObject.parent.GetChild(numMoves);
                cardsInStack.position = new Vector3(cardOrigin.x,
                    cardOrigin.y - stackYoffSet, cardOrigin.z - stackZoffSet);
                stackYoffSet += 0.40f;
                stackZoffSet += 0.03f;
            }
        }
        else
        {
            clickedObject.position = cardOrigin;
        }
    }

    private void UpdateGameObjects(int cardIndex, int numCards)
    {
        print("Inside update Game Objects");
        //Get the target pile type
        Transform ts;
        //Check to see if the game object is a card, or an empty spot
        if ((targetObject.parent.name.Equals("Top")) || (targetObject.parent.name.Equals("Bottom")))
        {
            //Case where we have selected an empty spot
            ts = targetObject;
        }
        else
        {
            //Case where pile is not empty, want to get parent pile
            ts = targetObject.parent;
        }
        print("ts is : " + ts.name);

        //Get parent pile 
        print("clicked object is: " + clickedObject.name);
        print("clicked object's parent is: " + clickedObject.parent.name);
        Transform ps = clickedObject.parent;
        print("ps is: " + ps.name);

        //Parent Game Object
        Transform mom = ps;

        print("Number of cards is: " + numCards);
        //if the card can go in the location selected remove it from the pile
        for (int i = 0; i < numCards;  i++)
        {
            print("card index is: " + cardIndex);
            // print("moving card: " + ps.transform.GetChild(cardIndex).name);
            ps.GetChild(cardIndex).SetParent(ts);
        }

        //print("is not faceup: " + !mom.transform.GetChild(mom.transform.childCount - 1).GetComponent<Selectable>().IsFaceUp());
      
        if (mom.childCount != 0 && !mom.GetChild(mom.childCount - 1).GetComponent<Selectable>().IsFaceUp())
        {
            print("Next card should flip!");
            mom.GetChild(mom.childCount - 1).GetComponent<Selectable>().FlipCard();
            Scoring.instance.AddScore(1);
        }
    } 
}
