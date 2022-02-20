using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    Vector2 mousePosition;
    Vector3 cardOrigin; //when card hit by mouseDown startLocation in case has to 
    Selectable cardFace = null;
    GameObject clickedObject;
    GameObject targetObject;
    bool isDragged;
    Solitaire solitaire;
    
    // Start is called before the first frame update
    void Start(){
        solitaire = FindObjectOfType<Solitaire>();
    }

    // Update is called once per frame
    void Update(){
        
        if (Input.GetMouseButtonDown(0))
        {                
            GetMouseClick();
        }

        if (isDragged)
        {

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            clickedObject.transform.Translate(mousePosition);

        }

        /*if (Input.GetMouseButton(0))
        {
            Rigidbody2D cardBody = clickedObject.GetComponent<Rigidbody2D>();
            cardBody.position = mousePosition;
        }*/
    }

    /*void OnMouseClick()
    {

        mousePosition = Camera.main.ScreenToWorldPoint(
                               new Vector3(Input.mousePosition.x,
                               Input.mousePosition.y, -10));
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit)
        {
            Rigidbody2D wasHit = hit.rigidbody;
            if (wasHit.gameObject.tag == "Deck")
            {

                TheLogger.PrintLog(wasHit.tag);

            }

        }
    }*/

    // Might want to change to on click down, need to investigate this
    void GetMouseClick(){
        
        //To store the mouse's position

        mousePosition = Camera.main.ScreenToWorldPoint(
                               new Vector2(Input.mousePosition.x,
                               Input.mousePosition.y));
        // https://docs.unity3d.com/ScriptReference/RaycastHit-collider.html 
        RaycastHit2D hit;

    //    if (Input.GetMouseButtonDown(0)){
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            // What we will do depends on what has been clicked
            if (hit){
            
            //get the object that is hit
            clickedObject = hit.collider.gameObject;
            cardOrigin = new Vector3(clickedObject.transform.position.x, clickedObject.transform.position.y, clickedObject.transform.position.z);
            clickedObject.GetComponent<SpriteRenderer>().color = Color.grey;



            // cardFace = wasHit.GetComponent<Selectable>();

            // wasHit.transform.forward = Vector3.forward;

            /* if (!cardFace.IsFaceUp())  
             {

                 return;
                 //cardFace.FlipCard();
             }*/

            /*Rigidbody2D cardBody = hit.rigidbody;
            cardBody.position = mousePosition;*/
            // The tag is associated with various game objects
           /* string whatHit = hit.collider.tag;
                print(whatHit);

                switch(whatHit){
                    case "Deck":
                        TheLogger.PrintLog("Hit Deck");
                        StockPile(clickedObject);
                        break;
                    case "Card":
                        TheLogger.PrintLog("Hit Card");
                        TalonPile(clickedObject);
                        break;
                    case "Top":
                        TheLogger.PrintLog("Hit Top");
                        Foundation(clickedObject);
                        break;
                    case "Bottom":
                        TheLogger.PrintLog("Hit Bottom");
                        Tableau(clickedObject);
                        break;
                    default:
                    //This is the bad place
                        print("You missed HAHAHAHAHAHAHA");
                        break;
                }*/
            }
        // }
    }

    // This is where we will call the algorithm for if Deck is touched
    void StockPile(GameObject clicky){

       // Solitaire solitaire = FindObjectOfType<Solitaire>();
        List<GameObject> stockPile = solitaire.GetStockPileArray();
        if (GameRules.IsEmpty(stockPile)){
            TheLogger.PrintLog("turn over Talons");
        } else {

            TheLogger.PrintLog("Deal Card");
            GameObject nextCard = stockPile[stockPile.Count - 1];
            TheLogger.PrintLog(nextCard.name);
            stockPile.RemoveAt(stockPile.Count - 1);

            //add ^^Card to Talon Pile
            solitaire.SetStockPileArray(stockPile);

        }
        
        /*isMouseClick(0) ?
        *  !isStockPileEmpty ? flipTopCard to Talon Pile : putTalonBackToStock 
        */
        print("Hit Deck");
        // Deal cards
        print("Deal 1 or 3 more cards");
    }

    // This is where we will call the algorithm for if a card is selected
    void TalonPile(GameObject clicky)
    {
        print("Hit Card");
        // select the card for moving somewhere
        // if double clicked, and can go to top spot, go there
        print("Pick this card up to move it somewhere");
        print("But if double clicked, and can go to top, move it there");
    }

    // Call algorithm for if top spot is selected
    void Foundation()
    {

        // NEED TO GET THE PILE SELECTED
        int pileSelected = 2;

        // Probably want one solitaire game object for whole class
        //Solitaire solitaire = FindObjectOfType<Solitaire>();
        List<string>[] foundations = solitaire.GetFoundations();
        List<string> foundationPile = foundations[pileSelected];
        if (GameRules.IsEmpty(foundationPile)){
            // You can't remove a card from an empty pile
            // TheLogger.PrintLog("No Action");
        } else {
            // Pick up the card
            // TheLogger.PrintLog("Grab Card");
            string topCard = foundationPile[foundationPile.Count - 1]; // Game object??
            // TheLogger.PrintLog(topCard);

            //wait for mouse up
            // get card pile we are trying to add it onto
            int BottomPileSelected = 3;

            //NOTE This should be the pile we are trying to add our card to
            List<string>[] tableaus =solitaire.GetTableaus();
            List<string> tableauPile = tableaus[BottomPileSelected];

            // Check if suits are alternating and card is 1 rank lower than 
            // bottom of stack card
            if (GameRules.IsAlternating(tableauPile, topCard) &&
                GameRules.IsRankGoood(tableauPile, topCard, "bottom")){
                    
                // if the card can go in the location selected remove it from the foundation pile
                foundationPile.RemoveAt(foundationPile.Count - 1);
                foundations[pileSelected] = foundationPile;
                solitaire.setFoundations(foundations);

                //add Card to Tableau Pile
                tableauPile.Add(topCard);
                tableaus[BottomPileSelected] = tableauPile;
                solitaire.setTableaus(tableaus);
            }
        }
        
        /*isMouseClick(0) ?
        *  !isStockPileEmpty ? flipTopCard to Talon Pile : putTalonBackToStock 
        */
        print("Hit Deck");
        // Deal cards
        print("Deal 1 or 3 more cards");
        /*
         * isEmpty ? RETURN : dragCard
         * !isMouseButton(0) ? LookForClosestElement
         * canCardBePlaced ? TRUE (SWITCH ELEMENTS) : return cards to origin
         */
        print("Hit Top");
        // cards on top can be moved back to the bottom
        print("Pick this card up to move it somewhere");
    }

    // Call algorithm for if bottom spot is selected
    void Tableau()
    {
         // NEED TO GET THE PILE SELECTED
        int pileSelected = 2;
        // Probably want one solitaire game object for whole class
       // Solitaire solitaire = FindObjectOfType<Solitaire>();
        List<string>[] tableaus = solitaire.GetTableaus();
        List<string> tableauPile = tableaus[pileSelected];
        /*
         *isCard Faceup ? getStackConnectedCardsBelow : return
         *!isMouseButton(0) ? LookForClosestElement
         *canCardBePlaced ? TRUE (SWITCH ELEMENTS) (isCardUncovered && faceDown ? flipCard : return): return cards to origin
         */
         // This will tell us where in our list the card is
        int cardIndex = -1;
        List<string> cardsSelected = new List<string>();

        if (GameRules.IsEmpty(tableauPile)){
            // You can't remove a card from an empty pile
            // TheLogger.PrintLog("No Action");
            // Check if card is face up
            // cardsSelected[0].IsFaceUp--------------------------------------------------
        } else if(cardsSelected[0] == cardsSelected[0]){
            // TheLogger.PrintLog("Find Card in Pile");
            for (int i = 0; i < tableauPile.Count; i++){
                if (tableauPile[i] == cardFace.name){
                    cardIndex = i;
                }
                // add the cards to the new list starting at the selected card
                if (cardIndex != -1){
                    cardsSelected.Add(cardFace.name);
                }
            }

            //wait for mouse up
            // get card pile we are trying to add it onto

            string selectedSecond = "bottom";
            int newPileSelected = 3;

            if (selectedSecond == "bottom"){
                //NOTE This should be the pile we are trying to add our card to
                // List<string>[] tableaus =solitaire.GetTableaus();
                List<string> newTableauPile = tableaus[newPileSelected];

                // if empty || cards go on top of stack (alternating, rank one less)
                if ((GameRules.IsEmpty(newTableauPile) && GameRules.IsCardCorrect(cardsSelected[0], "bottom")) 
                    ||
                   (GameRules.IsAlternating(newTableauPile, cardsSelected[0]) 
                   && GameRules.IsRankGoood(newTableauPile, cardsSelected[0], "bottom"))){
                        
                    // if the card can go in the location selected remove it from the foundation pile
                    for (int i = cardsSelected.Count - 1; i >= cardIndex; i--){
                        tableauPile.RemoveAt(i);
                    }
                    // add the cards to the new pile
                    for (int i = 0; i < cardsSelected.Count; i++){
                        newTableauPile.Add(cardsSelected[i]);
                    }

                    // update the lists in the game
                    tableaus[pileSelected] = tableauPile;
                    tableaus[newPileSelected] = newTableauPile;
                    
                    // send updates to game
                    solitaire.setTableaus(tableaus);

            } else if (selectedSecond == "top"){
                List<string>[] foundations = solitaire.GetFoundations();
                List<string> newFoundationPile = foundations[newPileSelected];
                // make sure only one card selected
                if (cardsSelected.Count == 1){
                    // check if it can be placed
                    if ((GameRules.IsEmpty(newTableauPile) 
                    && 
                    GameRules.IsCardCorrect(cardsSelected[0], "top"))
                    || (GameRules.IsSameSuit(newFoundationPile, cardsSelected[0]) 
                    && GameRules.IsRankGoood(newFoundationPile, cardsSelected[0], "top"))){
                        // remove card
                        tableauPile.RemoveAt(cardIndex);
                        // place card
                        newFoundationPile.Add(cardsSelected[0]);

                    // update the lists in the game
                    tableaus[pileSelected] = tableauPile;
                    foundations[newPileSelected] = newFoundationPile;
                    
                    // send updates to game
                    solitaire.setTableaus(tableaus);
                    solitaire.setFoundations(foundations);
                    }


                }
            } else {
                // return cards to pile (do nothing) let go of cards
            }


            }
        }

        // MUST TURN OFF THE NEW COLOUR HIGHLIGHTING WHAT CARD WE CLICKED!!!!

        /*isMouseClick(0) ?
        *  !isStockPileEmpty ? flipTopCard to Talon Pile : putTalonBackToStock 
        */
        print("Hit Deck");
        // Deal cards
        print("Deal 1 or 3 more cards");
        /*
         * isEmpty ? RETURN : dragCard
         * !isMouseButton(0) ? LookForClosestElement
         * canCardBePlaced ? TRUE (SWITCH ELEMENTS) : return cards to origin
         */
        print("Hit Top");
        // cards on top can be moved back to the bottom
        print("Pick this card up to move it somewhere");
        ////////////////////////////////////
        print("Hit Bottom");
        // cards on the bottom can be picked up individually or as a stack
        print("Pick up this card (or cards if stack selected)");
        // if stack, make sure it is moveable as a stack
    }
    private void OnMouseDown()
    {
        isDragged = true;
        TheLogger.PrintLog("Got to OnMouseDown");
    }

    private void OnMouseUp()
    {
       
        TheLogger.PrintLog("Got to OnMouseUp");
        isDragged = false;
        targetObject = GetCardPlaceLocation();
        TheLogger.PrintLog(targetObject.name);
        Transform targetPosition = targetObject.transform;

        switch (targetObject.name)
        {
            case "Top0":
            case "Top1":
            case "Top2":
            case "Top3":
                //clickedObject.transform.position = targetLocation.transform.position;
                /*clickedObject.transform.position = new Vector3(targetObject.transform.position.x,
                    targetObject.transform.position.y, targetObject.transform.position.z - .03f);*/
                TheLogger.PrintLog("Got to Tops");
                /*clickedObject.transform.Translate(targetObject.transform.position.x,
                    targetObject.transform.position.y, targetObject.transform.position.z -0.03f);*/
                //Foundation();
                
                break;
            case "Bottom0":
            case "Bottom1":
            case "Bottom2":
            case "Bottom3":
            case "Bottom4":
            case "Bottom5":
            case "Bottom6":
                // clickedObject.transform.position = targetLocation.transform.position;
                /*clickedObject.transform.position = new Vector3(targetObject.transform.position.x,
                    targetObject.transform.position.y - .04f, targetObject.transform.position.z - .03f);*/
                TheLogger.PrintLog("Got to Bottoms");
                /*clickedObject.transform.Translate(targetObject.transform.position.x,
                    targetObject.transform.position.y - .04f, targetObject.transform.position.z - 0.03f);*/
                //Tableau();
                break;
            default:
                TheLogger.PrintLog("got to default");
                break;

        }

        clickedObject.transform.position = targetPosition.position;

        /*
         * check what the clickedObject's collider is colliding with and then set it there.
         * 
         */

    }

    private GameObject GetCardPlaceLocation() 
    {
        clickedObject.layer = 2;
        RaycastHit2D hit;
        Rigidbody2D targetBody = null;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.transform != null)
        {
            targetBody = hit.transform.GetComponent<Rigidbody2D>();
            TheLogger.PrintLog("we got the targetBody");
        }

        GameObject targetLocation;

        if (targetBody.transform.gameObject.transform.parent.gameObject.name.Equals("Bottom"
            ) || targetBody.transform.gameObject.transform.parent.gameObject.name.Equals("Top"))
        {
            targetLocation = targetBody.gameObject;
           
        } else
        {
            targetLocation = targetBody.gameObject.transform.parent.gameObject;
        }
        
        clickedObject.layer = 0;
        return targetLocation;
    }
}
