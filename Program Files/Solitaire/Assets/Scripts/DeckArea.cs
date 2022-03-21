using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class which looks after Deck related functions
public class DeckArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        Scoring.instance.ReduceScore(20);  
    }
}
