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
    
    //Start is called before the first frame update
    void Start(){
        Scoring.instance.ResetScore(); // Resets the score whenever User starts a new game
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
        //To store the mouse's position
        mousePosition = Camera.main.ScreenToWorldPoint(
                               new Vector2(Input.mousePosition.x,
                               Input.mousePosition.y));
        //https://docs.unity3d.com/ScriptReference/RaycastHit-collider.html 
        RaycastHit2D hit;

        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        //What we will do depends on what has been clicked
        if (hit){
            //get the object that is hit            
            clickedObject = hit.collider.transform;
            cardOrigin = new Vector3(clickedObject.position.x, clickedObject.position.y, clickedObject.position.z);
            //checks if the stockpile has been hit (so that is will deal)
            if (clickedObject.parent.name.Equals("DeckButton") || clickedObject.name.Equals("DeckButton"))
            {
                isStockpileCard = true;
                StockPile();
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
        TheLogger.PrintLog("card clicked");

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
        if (GetCardPlaceLocation() == null){
            //is the object in a stack, returns to origin
            if ((clickedObject.GetSiblingIndex() < clickedObject.parent.childCount - 1) && (clickedObject.GetComponent<Selectable>().IsFaceUp()))
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
            targetObject = GetCardPlaceLocation();
        }

        dropLocation = DropLocation();
        
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


    // gets the location where you are trying to place the card
    // returns null if not on a game object
    private Transform GetCardPlaceLocation()
    {
        Transform cardsInStack;
        //makes sure the cards in the stack can't be raycast, so the object underneath can be selected
        for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
        {
            cardsInStack = clickedObject.parent.GetChild(numMoves);
            cardsInStack.gameObject.layer = 2;
        }

        //get the object to be dropped on
        RaycastHit2D hit;
        Rigidbody2D targetBody = null;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        //get object when hits something
        if (hit.transform != null)
        {
            targetBody = hit.transform.GetComponent<Rigidbody2D>();
         
            //TheLogger.PrintLog("we got the targetBody: " + targetBody.transform.parent.name);

            // return null if hits talonpile or stock pile
            if (targetBody.transform.parent.name.Equals("TalonPile")||targetBody.transform.parent.name.Equals("DeckButton")){
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
        if (targetBody.transform.parent.name.Equals("Bottom"
            ) || targetBody.transform.parent.name.Equals("Top"))
        {
            targetLocation = targetBody.transform;
        }
        else
        {
            targetLocation = targetBody.transform.parent;
        }
        for (int numMoves = clickedObject.GetSiblingIndex(); numMoves < clickedObject.parent.childCount; numMoves++)
        {
            cardsInStack = clickedObject.parent.GetChild(numMoves);
            cardsInStack.gameObject.layer = 0;
        }

        return targetLocation;
    }

    //checks if more than one card being moved at a time.
    void CardMove()
    {
        // if it is a group of cards
        if ((clickedObject.GetSiblingIndex() < clickedObject.parent.childCount - 1) && (clickedObject.GetComponent<Selectable>().IsFaceUp()))
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

    //This is where we will call the algorithm for if Deck is touched
    void StockPile(){
        //this is 'deck' object which contains both 'deckButton' and 'talonPile'
        Transform deckRoot = clickedObject.root;
        //this is 'talonPile' GameObject
        Transform talonPile = deckRoot.GetChild(1);
        float zOffSet;

        //checks to see what the zOffSet will be depending on how big the talonPile is
        if (talonPile.transform.childCount > 0)
        {
            zOffSet = talonPile.GetChild(talonPile.childCount - 1).position.z - 0.03f;
        } else
        {
            zOffSet = -0.03f;
        }
       
        //checks to see if it restocks the Stockpile or flips onto the talonpile
        if (!clickedObject.name.Equals("DeckButton"))
        {
            //move the card to the talonpile
            
            clickedObject.SetParent(talonPile);
            clickedObject.position = new Vector3(talonPile.position.x, talonPile.position.y, talonPile.position.z + zOffSet);
            clickedObject.GetComponent<Selectable>().FlipCard();

        } 
        else 
        {
            // refresh from the talonpile
            // flips cards over if not in Vegas mode
            if (!MainMenu.GetOnVegas())
            {
                RestockDeck(deckRoot, talonPile);
            }
        }
       
        print("Deal 1 or 3 more cards");
    }

    //Restocks the stockpile from the talonpile
    void RestockDeck(Transform deckRoot, Transform talonPile)
    {
      
        //print("isVegas is set to: " + MainMenu.GetOnVegas());
        
            Transform talonCard;
            Transform stockPile = deckRoot.GetChild(0);

            float zOffSet = 0.03f;

            //goes through each card in the talonpile and puts it back in the stockpile
            for (int talonLength = talonPile.transform.childCount; talonLength > 0; talonLength--)
            {

                talonCard = talonPile.GetChild(talonPile.childCount - 1);
                talonCard.SetParent(deckRoot.GetChild(0));
                talonCard.position = new Vector3(stockPile.position.x, stockPile.position.y, stockPile.position.z - zOffSet);
                talonCard.GetComponent<Selectable>().FlipCard();
                zOffSet += 0.03f;

            }
            for (int i = 0; i <= 19; i++)
            {
                Scoring.instance.ReduceScore();
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
        if (targetObject.name.Equals("DeckButton")||(targetObject.name.Equals("TalonPile")))
        {   
            //tries to move onto the stockpile or talon pile
            UpdateLocation(false, false); // return to origin
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
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    // Add 5 points for moving to the tableau from the Talon
                    Scoring.instance.AddScore();
                    return;
                }
                print("Card is not a King, cannot be moved here");
                UpdateLocation(false, false); // return to origin
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
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    // Add 5 points for moving to the tableau from the Talon
                    Scoring.instance.AddScore();
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            //print("Card colour is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
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
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                //print("Card is not an Ace, connot be put in an empty slot.");
                UpdateLocation(false, false); // return to origin
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
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            //print("Card suit is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
            //print("Cannot move between non-empty foundations.");
            //UpdateLocation(false, false); // return to origin
            //return;
        }
    }

    //Call algorithm for if top spot is selected
    void Foundation() {
        //Get the pile that card(s) was selected from
        //GameObject parentStack = clickedObject.transform.parent.gameObject; 

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

        if (targetStack.parent.name.Equals("Bottom")){
            //string stackCard = targetStack.transform.GetChild(targetStack.transform.childCount - 1).gameObject.name;

            //Case where we have selected an empty stack
            if (GameRules.IsEmpty(targetStack)){
                //print("Cannot move to an empty tableau stack");
                UpdateLocation(false, false); // return to origin
                return;
            }
            string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
            //Case where we have not selected an empty stack
            if (GameRules.IsAlternating(stackCard, clickedObject.name)){
                //print("Card colour is opposite to the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom")){
                    //print("Card rank is one less than target card, it can be moved.");
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, 1);
                    // Card moving from Foundation To Tableau Piles. Remove Points
                    Scoring.instance.ReduceScore();
                    Scoring.instance.ReduceScore();
                    Scoring.instance.ReduceScore();
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            //print("Card colour is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
        }

        if (targetStack.parent.name.Equals("Top")){
            //Case where we have selected an empty pile
            if (GameRules.IsEmpty(targetStack)){
                //print("Check Rank, can only move an Ace to an empty foundation slot");
                if(GameRules.IsEmptyRank(clickedObject.name, "top")){
                    //print("Card is an Ace, movingto new slot.");
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, 1);
                    return;
                }
                //print("Card is not an Ace, connot be put in an empty slot.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            //print("Cannot move between non-empty foundations.");
            UpdateLocation(false, false); // return to origin
            return;
        }
    }

    //Call algorithm for if bottom spot is selected
    void Tableau()
    {
        //Get the pile that card(s) was selected from
        //GameObject parentStack = clickedObject.transform.parent.gameObject; 

        //Get index of card selected card
        int cardIndex = clickedObject.GetSiblingIndex();
        //print("card Index of selected card is:" + cardIndex);

        //Get number of cards to see how many have been selected
        int numCards = clickedObject.parent.transform.childCount - cardIndex;
        //print("Number of cards selected is: " + numCards);
        //print("target object is " + targetObject.name);
        //Get target stack
        Transform targetStack;
        if ((targetObject.parent.name.Equals("Top"))||(targetObject.parent.name.Equals("Bottom"))){
            //Case where we have selected an empty pile
            targetStack = targetObject;
        } else {
            //Case where pile is not empty
            targetStack = targetObject.parent;
        }

        //print("targetStack name is " + targetStack.name);
        if (targetStack.parent.name.Equals("Bottom")){
            //print("We're moving the card to the bottom");
            
            //Case 1, new pile is empty, must be a king
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

            print("Case 2 - moving to a stack with cards");
            //Case 2 Moving onto a tableau pile alt colours and rank -1
            string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
            if (GameRules.IsAlternating(stackCard, clickedObject.name)){
                print("Card colour is opposite to the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "bottom")){
                   //print("clickedObject's name is " + clickedObject.name);
                   //print("Card rank is one less than target card, it can be moved.");
                    UpdateLocation(true, false); //move, not to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
               //print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
           //print("Card colour is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
        }

        if (targetStack.parent.name.Equals("Top")){
            //Make sure only one card is selected
            if (numCards != 1){
                //print("Can only move one card to the top at a time.");
                UpdateLocation(false, false); // return to origin
                return;
            }

            // Case 1: Foundation Pile is empty
            if (GameRules.IsEmpty(targetStack)){
                //print("Target stack is empty");
                if(GameRules.IsEmptyRank(clickedObject.name, "top")){
                    //print("Card is an Ace and can be moved");
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                //print("Card is not an Ace, cannot be moved here");
                UpdateLocation(false, false); // return to origin
                return;
            }

            //Case 2: Foundation Pile is not empty
            //print("Case 2 - moving to a stack with cards");
            //Case 2 Moving onto a foundation pile same suit and rank +1
            string stackCard = targetStack.GetChild(targetStack.childCount - 1).name;
            if (GameRules.IsSameSuit(stackCard, clickedObject.name)){
                //print("Card suit is same as the target card");
                if(GameRules.IsRankGoood(stackCard, clickedObject.name, "top")){
                    //print("Card rank is one more than target card, it can be moved.");
                    UpdateLocation(true, true); //move, to the top
                    UpdateGameObjects(cardIndex, numCards);
                    return;
                }
                //print("Card rank is incorrect and cannot be moved.");
                UpdateLocation(false, false); // return to origin
                return;
            }
            //print("Card suit is incorrect and cannot be moved.");
            UpdateLocation(false, false); // return to origin
            return;
        }
        //print("This is the bad place");
    }

    
    public Transform DropLocation(){
        Transform targetPosition = targetObject;
        //this grabs the last child index numnber
        int lastChild;

        if (targetPosition.childCount -1 < 0){
            lastChild = 0;
        } else {
            lastChild = targetPosition.childCount - 1;
        }

        //this grabs the GameObject which is either the last child or the parent if there is no children
        Transform dropLocation;
        if (targetPosition.childCount == 0){
            dropLocation = targetPosition;
        }else{
          
            dropLocation = targetPosition.GetChild(lastChild);
        }
       
        return dropLocation;
    } 

    private void UpdateLocation(bool isMoving, bool isTop){
        if (isMoving){
            if (isTop){
                //Foundation: offset the z-index only
                clickedObject.position = new Vector3(dropLocation.position.x,
                    dropLocation.position.y, dropLocation.position.z - 0.03f);
                // Card moves to the top + 10 points
                Scoring.instance.AddScore();
                Scoring.instance.AddScore();
            }
            else
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
                if ((clickedObject.GetSiblingIndex() < clickedObject.parent.childCount - 1) && (clickedObject.GetComponent<Selectable>().IsFaceUp()))
                {
                    float stackYoffSet;
                    Transform cardsInStack;
                    print("targetObject is " + targetObject.name);
                    print("targetObject childcount is  " + targetObject.childCount);
                    //if ((targetObject.transform.parent.gameObject.name.Equals("Top")) || (targetObject.transform.parent.gameObject.name.Equals("Bottom")))
                    if (targetObject.childCount == 0) 
                    {
                        
                        print("Got to childCount 0");
                        stackYoffSet = 0.0f;
                    } else
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

        }else{
            print("Card cannot move there!");
            //return the card to its original location
            if ((clickedObject.GetSiblingIndex() < clickedObject.parent.childCount - 1) && (clickedObject.GetComponent<Selectable>().IsFaceUp()))
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
            //clickedObject.transform.position = cardOrigin;
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
        if (mom.childCount == 0)
        {
            print("No cards to flip!");
            return;
        }
        if (!mom.GetChild(mom.childCount - 1).GetComponent<Selectable>().IsFaceUp())
        {
            print("Next card should flip!");
            mom.GetChild(mom.childCount - 1).GetComponent<Selectable>().FlipCard();
            Scoring.instance.AddScore();
        }
    }
}
