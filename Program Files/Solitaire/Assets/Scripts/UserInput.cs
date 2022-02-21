using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour{
    Vector2 mousePosition;
    Vector3 cardOrigin; //when card hit by mouseDown startLocation in case has to 
    // Selectable cardFace = null;
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
        string parentStack = clickedObject.transform.parent.gameObject.name;    
        // Get the pile number to use as an index
        int parentStackNo = int.Parse(parentStack.Substring(parentStack.Length-1));
        string parentStackType = clickedObject.transform.root.name;

        List<string>[] foundations = solitaire.GetFoundations();
        List<string> foundationPile = foundations[parentStackNo];
        List<string> cardsSelected = new List<string>();
        cardsSelected.Add(foundationPile[foundationPile.Count - 1]); // Game object??

        //get card pile we are trying to add it onto
        string targetStack = targetObject.transform.name;
        int targetStackNo = int.Parse(targetStack.Substring(targetStack.Length - 1));
        string targetStackType = targetObject.transform.parent.gameObject.name;  

        //NOTE This should be the pile we are trying to add our card to
        List<string>[] tableaus =solitaire.GetTableaus();
        List<string> tableauPile = tableaus[targetStackNo];

        // Check if suits are alternating and card is 1 rank lower than 
        // bottom of stack card
        if (GameRules.IsAlternating(tableauPile, cardsSelected[0]) && GameRules.IsRankGoood(tableauPile, cardsSelected[0], "bottom")){
                
            // if the card can go in the location selected remove it from the foundation pile
            foundationPile.RemoveAt(foundationPile.Count - 1);
            foundations[parentStackNo] = foundationPile;
            solitaire.setFoundations(foundations);

            //add Card to Tableau Pile
            tableauPile.Add(cardsSelected[0]);
            tableaus[targetStackNo] = tableauPile;
            solitaire.setTableaus(tableaus);

            // Update Locations
            UpdateLocation(true, false);
            solitaire.UpdatePositions(cardsSelected, parentStackType, parentStackNo, targetStackType, targetStackNo);
            return;
        }
        UpdateLocation(false, false);
    }

    // Call algorithm for if bottom spot is selected
    void Tableau()
    {
        // Get the pile that card(s) was selected from
        string parentStack = clickedObject.transform.parent.gameObject.name;    
        // Get the pile number to use as an index
        int parentStackNo = int.Parse(parentStack.Substring(parentStack.Length-1));
        string parentStackType = clickedObject.transform.root.name;
        // Get the tableau piles and the list the card was selected from
        List<string>[] tableaus = solitaire.GetTableaus();
        List<string> tableauPile = tableaus[parentStackNo];                
       
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
        string targetStack = targetObject.transform.name;
        int targetStackNo = int.Parse(targetStack.Substring(targetStack.Length - 1));
        string targetStackType;
        // Get the target pile type      
        if ((gameObject.transform.parent.gameObject.name.Equals("Top"))||(gameObject.transform.parent.gameObject.name.Equals("Bottom"))){
            // Case where we have selected an empty pile
            targetStackType = targetObject.transform.name;
        } else {
            // Case where pile is not empty
            targetStackType = targetObject.transform.parent.gameObject.name;
        }

        if (targetStackType == "Bottom"){
            // Pile we are adding card(s) to
            List<string> newPile = tableaus[targetStackNo];

            // if empty || cards go on top of stack (alternating, rank one less)
            if (GameRules.IsEmpty(newPile)){
                if (!GameRules.IsCardCorrect(cardsSelected[0], "bottom")){
                    // clickedObject.transform.position = cardOrigin;
                    UpdateLocation(false, false);
                    return;
                }
            }else if(!(GameRules.IsAlternating(newPile, cardsSelected[0])&& GameRules.IsRankGoood(newPile, cardsSelected[0], "bottom"))){
                    // clickedObject.transform.position = cardOrigin;
                    UpdateLocation(false, false);
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
            tableaus[parentStackNo] = tableauPile;
            tableaus[targetStackNo] = newPile;
            
            // send updates to game
            solitaire.setTableaus(tableaus);

            // update locations
            // clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
            // dropLocation.transform.position.y - .40f, dropLocation.transform.position.z - .03f);
            UpdateLocation(true, false);
            // Call Solitaire method to update the drawing of the cards
            solitaire.UpdatePositions(cardsSelected, parentStackType, parentStackNo, targetStackType, targetStackNo);
            return;
        } else if (targetStackType == "Top"){
            print("Card is going to top");
            // Get pile we are adding cards to
            List<string>[] foundations = solitaire.GetFoundations();
            List<string> newFoundationPile = foundations[targetStackNo];

            // make sure only one card selected
            if (cardsSelected.Count == 1){
                // check if it can be placed

                if (GameRules.IsEmpty(newFoundationPile)) {
                    if(!GameRules.IsCardCorrect(cardsSelected[0], "top")){
                        // clickedObject.transform.position = cardOrigin;
                        UpdateLocation(false, false);
                        return;
                    }
                }else if(!(GameRules.IsSameSuit(newFoundationPile, cardsSelected[0]) && GameRules.IsRankGoood(newFoundationPile, cardsSelected[0], "top"))){
                    // clickedObject.transform.position = cardOrigin;
                    UpdateLocation(false, false);
                    return;
                }

                print("Card is gooood for top");
                // remove card
                tableauPile.RemoveAt(cardIndex);
                // place card
                newFoundationPile.Add(cardsSelected[0]);

                // update the lists in the game
                tableaus[parentStackNo] = tableauPile;
                foundations[targetStackNo] = newFoundationPile;
                
                // send updates to game
                solitaire.setTableaus(tableaus);
                solitaire.setFoundations(foundations);

                // update location
                // clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                // dropLocation.transform.position.y, dropLocation.transform.position.z - .03f);
                UpdateLocation(true, true);
                solitaire.UpdatePositions(cardsSelected, parentStackType, parentStackNo, targetStackType, targetStackNo);
                return;
            }
        }
        // return cards to pile (do nothing) let go of cards
        clickedObject.transform.position = cardOrigin;
        print("Returning Card to Origin");
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

    private void UpdateLocation(bool isMoving, bool isTop){
        if (isMoving){
            if (isTop){
                clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                dropLocation.transform.position.y, dropLocation.transform.position.z - 0.03f);
            }else{
                clickedObject.transform.position = new Vector3(dropLocation.transform.position.x,
                dropLocation.transform.position.y - 0.40f, dropLocation.transform.position.z - 0.03f);
            }

        }else{
            clickedObject.transform.position = cardOrigin;
        }
    }
}
