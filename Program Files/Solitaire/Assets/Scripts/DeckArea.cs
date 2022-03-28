using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class which looks after Deck related functions
public class DeckArea : MonoBehaviour
{
    int vegasCounter = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("o").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("x").GetComponent<SpriteRenderer>().enabled = false;
    }

    //This is where we will call the algorithm for if Deck is touched
    public void StockPile(Transform clickedObject)
    {
        //this is 'deck' object which contains both 'deckButton' and 'talonPile'
        Transform deckRoot = clickedObject.root;
        //this is 'talonPile' GameObject
        Transform talonPile = deckRoot.GetChild(1);
        float zOffSet;

        //checks to see what the zOffSet will be depending on how big the talonPile is
        if (talonPile.transform.childCount > 0)
        {
            zOffSet = talonPile.GetChild(talonPile.childCount - 1).position.z - 0.03f;
        }
        else
        {
            zOffSet = -0.03f;
        }

        //checks to see if it restocks the Stockpile or flips onto the talonpile
        if (!clickedObject.name.Equals("DeckButton"))
        {
            //move the card to the talonpile
            if (MainMenu.GetDealThree())
            {
                DealThree(clickedObject, talonPile, zOffSet);
            }
            else
            {
                DealOne(clickedObject, talonPile, zOffSet);
            }
        }
        else
        {
            // refresh from the talonpile
            // flips cards over if not in Vegas mode
            if (!MainMenu.GetOnVegas() || (MainMenu.GetOnVegas() && MainMenu.GetDealThree() && vegasCounter < 3))
            {
                RestockDeck(deckRoot, talonPile);
                vegasCounter += 1;
            }
            // When it gets to 3 change o to x
            if (vegasCounter == 3){
                GameObject.Find("o").GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("x").GetComponent<SpriteRenderer>().enabled = true;m
            }
        }
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
        Scoring.instance.ReduceScore(20);  
    }

    //deal three cards if playing deal three option
    void DealThree(Transform clickedObject, Transform talonPile, float zOffSet)
    {

        float thisZOffSet = zOffSet;
        float xOffSet = 0;
        
        //the card being moved
        Transform currentCard;
        Transform deck = talonPile.parent.GetChild(0);
        
        for (int i = 0; i < talonPile.childCount; i++)
        {
            talonPile.GetChild(i).position = new Vector3(talonPile.position.x, talonPile.GetChild(i).position.y, talonPile.GetChild(i).position.z);
        }

        //cards to deal
        int cards = 3;

        //checks how many cards are left in stockpile
        if (clickedObject.parent.childCount < 3)
        {          
            cards = clickedObject.parent.childCount;
        }

        //goes through each card and moves it to talonpile and flips it
        for (int card = 0; card < cards; card++)
        {
            currentCard = deck.GetChild(deck.childCount - 1);
            currentCard.SetParent(talonPile);
            currentCard.position = new Vector3(talonPile.position.x + xOffSet, talonPile.position.y, talonPile.position.z + thisZOffSet);

            currentCard.GetComponent<Selectable>().FlipCard();
              
            thisZOffSet -= 0.03f;
            xOffSet += 0.4f;
        }
    }

    //deal one card for one card play from stockpile
    void DealOne(Transform clickedObject, Transform talonPile, float zOffSet)
    {

        clickedObject.SetParent(talonPile);
        clickedObject.position = new Vector3(talonPile.position.x, talonPile.position.y, talonPile.position.z + zOffSet);
        clickedObject.GetComponent<Selectable>().FlipCard();

        if (MainMenu.GetOnVegas()){
            GameObject.Find("o").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("x").GetComponent<SpriteRenderer>().enabled = true;
        }

    }
}

