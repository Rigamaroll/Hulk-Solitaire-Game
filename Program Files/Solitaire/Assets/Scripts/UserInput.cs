using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour{
    Vector2 mousePosition;
    Vector3 cardOrigin; //when card hit by mouseDown startLocation in case has to 
    // Selectable cardFace = null;
    GameObject clickedObject;
    GameObject targetObject;
    bool isDragged;
    Solitaire solitaire;
    GameObject dropLocation;
    
    // Start is called before the first frame update
    void Start(){
        solitaire = FindObjectOfType<Solitaire>();
    }

    // Update is called once per frame
    void Update(){
        
        if (Input.GetMouseButtonDown(0)){                
            GetMouseClick();
        }

        if (isDragged){
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

            //testing for card click
            TheLogger.PrintLog("card clicked");

            //check if card is face up 
 
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
        // Get the pile that card(s) was selected from
        string parentPile = clickedObject.transform.parent.gameObject.name;    
        // Get the pile number to use as an index
        int pileSelected = int.Parse(parentPile.Substring(parentPile.Length-1));
        // Get the tableau piles and the list the card was selected from
        List<string>[] tableaus = solitaire.GetTableaus();
        List<string> tableauPile = tableaus[pileSelected];                
       
        // This will tell us where in our list the card is
        int cardIndex = -1;
        List<string> cardsSelected = new List<string>();
        string cardSelected = clickedObject.transform.name;

        // Get the cards in a tableau pile under a card selected
        for (int i = 0; i < tableauPile.Count; i++){
            if (tableauPile[i] == cardSelected){
                cardIndex = i;
            }
            // add the cards to the new list starting at the selected card
            if (cardIndex != -1){
                cardsSelected.Add(tableauPile[i]);
                print(tableauPile[i]);
            }
        }

        //get card pile we are trying to add it onto
        string targetPile = targetObject.transform.name;
        int newPileSelected = int.Parse(targetPile.Substring(targetPile.Length - 1));
        string whatSelected;
        // Get the target pile type      
        if ((gameObject.transform.parent.gameObject.name.Equals("Top"))||(gameObject.transform.parent.gameObject.name.Equals("Bottom"))){
            // Case where we have selected an empty pile
            whatSelected = targetObject.transform.name;
        } else {
            // Case where pile is not empty
            whatSelected = targetObject.transform.parent.gameObject.name;
        }

        if (whatSelected == "Bottom"){
            // Pile we are adding card(s) to
            List<string> newPile = tableaus[newPileSelected];

            // if empty || cards go on top of stack (alternating, rank one less)
            if (GameRules.IsEmpty(newPile)){
                if (!GameRules.IsCardCorrect(cardsSelected[0], "bottom")){
                    clickedObject.transform.position = cardOrigin;
                    return;
                }
            }else if(!(GameRules.IsAlternating(newPile, cardsSelected[0])&& GameRules.IsRankGoood(newPile, cardsSelected[0], "bottom"))){
                    clickedObject.transform.position = cardOrigin;
                    return;
            }
  
            // if the card can go in the location selected remove it from the foundation pile
            for (int i = cardsSelected.Count - 1; i >= cardIndex; i--){
                tableauPile.RemoveAt(i);
            }
            // add the cards to the new pile
            for (int i = 0; i < cardsSelected.Count; i++){
                newPile.Add(cardsSelected[i]);
            }

            // update the lists in the game
            tableaus[pileSelected] = tableauPile;
            tableaus[newPileSelected] = newPile;
            
            // send updates to game
            solitaire.setTableaus(tableaus);

            // update locations
            clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
            dropLocation.transform.position.y - .40f, dropLocation.transform.position.z - .03f);
            return;
        } else if (whatSelected == "Top"){
            print("Card is going to top");
            // Get pile we are adding cards to
            List<string>[] foundations = solitaire.GetFoundations();
            List<string> newFoundationPile = foundations[newPileSelected];

            // make sure only one card selected
            if (cardsSelected.Count == 1){
                // check if it can be placed

                if (GameRules.IsEmpty(newFoundationPile)) {
                    if(!GameRules.IsCardCorrect(cardsSelected[0], "top")){
                        clickedObject.transform.position = cardOrigin;
                        return;
                    }
                }else if(!(GameRules.IsSameSuit(newFoundationPile, cardsSelected[0]) && GameRules.IsRankGoood(newFoundationPile, cardsSelected[0], "top"))){
                        clickedObject.transform.position = cardOrigin;
                        return;
                }

                print("Card is gooood for top");
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

                // update location
                clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                dropLocation.transform.position.y, dropLocation.transform.position.z - .03f);
                return;
            }
        }
        // return cards to pile (do nothing) let go of cards
        clickedObject.transform.position = cardOrigin;
        print("Returning Card to Origin");
    }

    private void OnMouseDown(){
        isDragged = true;
    }

    private void OnMouseUp(){
        isDragged = false;
        
        if (GetCardPlaceLocation() == null){
            clickedObject.transform.position = cardOrigin;
            return;
        }
        else{
            targetObject = GetCardPlaceLocation();
        }
        //TheLogger.PrintLog(targetObject.name);
        /*Transform targetPosition = targetObject.transform;
        int lastChild = targetPosition.childCount - 1 < 0 ? 0 : targetPosition.childCount - 1;
        GameObject dropLocation = targetPosition.childCount == 0 ? targetPosition.gameObject : targetPosition.GetChild(lastChild).transform.gameObject;*/
        //check to see if the card is faceup 
        //if (clickedObject.GetComponent<Selectable>().IsFaceUp())
        dropLocation = DropLocation();
        
        switch (clickedObject.transform.parent.gameObject.name){
            case "Top0":
            case "Top1":
            case "Top2":
            case "Top3":
               
                // clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                //     dropLocation.transform.position.y, dropLocation.transform.position.z - .03f);
                TheLogger.PrintLog("Got to Tops");
               
                //Foundation();
                
                break;
            case "Bottom0":
            case "Bottom1":
            case "Bottom2":
            case "Bottom3":
            case "Bottom4":
            case "Bottom5":
            case "Bottom6":
                
                // clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                //     dropLocation.transform.position.y - .40f, dropLocation.transform.position.z - .03f);
                TheLogger.PrintLog("Got to Bottoms");
               
                Tableau();
                break;
            default:
                TheLogger.PrintLog("got to default");
                break;

        }

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
        //this grabs the last child index numnber
        int lastChild;

        if (targetPosition.childCount -1 < 0){
            lastChild = 0;
        } else {
            lastChild = targetPosition.childCount - 1;
        }

        //this grabs the GameObject which is either the last child or the parent if there is no children
        GameObject dropLocation;
        if (targetPosition.childCount == 0){
            dropLocation = targetPosition.gameObject;
        }else{
            dropLocation = targetPosition.GetChild(lastChild).transform.gameObject;
        }
       
        return dropLocation;
    } 
}
