using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 cardOrigin; //when card hit by mouseDown startLocation in case has to revert
    
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
       
        if (Input.GetMouseButton(0))
        {
           GetMouseClick();
        } 
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
                               new Vector3(Input.mousePosition.x,
                               Input.mousePosition.y, -10));
        // https://docs.unity3d.com/ScriptReference/RaycastHit-collider.html 
        RaycastHit2D hit;

       // if (Input.GetMouseButton(0)){
            
        

            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            // What we will do depends on what has been clicked
            if (hit){

                // The tag is associated with various game objects
                
                Rigidbody2D wasHit = hit.rigidbody;
                Selectable cardFace = wasHit.GetComponent<Selectable>();
            
                wasHit.transform.forward = Vector3.forward;
            

               /* if (!cardFace.IsFaceUp())  
                {
                    
                    return;
                    //cardFace.FlipCard();
                }*/
                wasHit.position = mousePosition;
                

                string whatHit = hit.collider.tag;
                print(whatHit);

                switch(whatHit){
                    case "Deck":
                        StockPile();
                        break;
                    case "Card":
                        TalonPile();
                        break;
                    case "Top":
                        Foundation();
                        break;
                    case "Bottom":
                        Tableau();
                        break;
                    default:
                    //This is the bad place
                        print("You missed");
                        break;
                }
            }
        //}
    }

    // This is where we will call the algorithm for if Deck is touched
    void StockPile(){

        Solitaire solitaire = FindObjectOfType<Solitaire>();
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
    void Foundation(){

        // NEED TO GET THE PILE SELECTED
        int pileSelected = 2;


        // Probably want one solitaire game object for whole class
        Solitaire solitaire = FindObjectOfType<Solitaire>();
        List<string>[] foundations = solitaire.GetFoundations();
        List<string> foundationPile = foundations[pileSelected];
        if (GameRules.IsEmpty(foundationPile)){
            // You can't remove a card from an empty pile
            TheLogger.PrintLog("No Action");
        } else {
            // Pick up the card
            TheLogger.PrintLog("Grab Card");
            GameObject topCard = foundationPile[foundationPile.Count - 1];
            TheLogger.PrintLog(topCard.name);


            //wait for mouse up
            // get card pile we are trying to add it onto
            int BottomPileSelected = 3;

            //NOTE This should be the pile we are trying to add our card to
            List<string>[] tableaus =solitaire.GetTableaus();
            List<string> tableauPile = tableaus[BottomPileSelected];

            // Check if suits are alternating and card is 1 rank lower than 
            // bottom of stack card
            if (GameRules.IsAlternating(tableauPile, topCard.name) &&
                GameRules.IsRankGoood(tableauPile, topCard.name, "bottom")){
                    
                // if the card can go in the location selected remove it from the foundation pile
                foundationPile.RemoveAt(foundationPile.Count - 1);
                foundations[pileSelected] = foundationPile;
                solitaire.setFoundations(foundations);

                //add Card to Tableau Pile
                tableauPile.Add(topCard);
                tableaus[BottomPileSelected] = tableauPile;
                solitaire.SetTableaus(tableaus);
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
    void Tableau(){
        /*
         *isCard Faceup ? getStackConnectedCardsBelow : return
         *!isMouseButton(0) ? LookForClosestElement
         *canCardBePlaced ? TRUE (SWITCH ELEMENTS) (isCardUncovered && faceDown ? flipCard : return): return cards to origin
         */
         
        print("Hit Bottom");
        // cards on the bottom can be picked up individually or as a stack
        print("Pick up this card (or cards if stack selected)");
        // if stack, make sure it is moveable as a stack
    }
}
