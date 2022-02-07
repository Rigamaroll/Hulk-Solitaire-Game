using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    
    // Suits Clubs / Diamonds / Hearts / Spades
    public static string[] suits = new string[] {"C", "D", "H", "S"};
    // Values Ace through King
    public static string[] values = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13"};
   
    public List<string>[] bottoms;
    public List<string>[] tops;

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    private List<string> top0 = new List<string>();
    //private List<string> top1 = new List<string>();
    //private List<string> top2 = new List<string>();
    //private List<string> top3 = new List<string>();

    public List<string> deck;

    // Start is called before the first frame update
    void Start() {
        // Initialize the bottoms array with each of the piles
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        
        // Generate the deck 
        deck = GenerateDeck();
        // Call shuffle the deck
        Shuffle(deck);
 
        // Deal the card onto the board and display them
        DealCards();

        // TEST ZONE ///////////////////////////
        // Alternating 
        print("// SOL 12 - Tableau cards can only be stacked in alternating colors");
        print("----- Test for alternating colours -----");
        print("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "D4"));
        print("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "H3"));
        print("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "C1"));
        print("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "S13"));
        print("");
        print("// SOL 9 - Rank of cards must be functional as per rules");
        print("----- Test for rank -----");
        print("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D4", "bottom"));
        print("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D6", "bottom"));
        print("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D8", "bottom"));
        print("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D10", "bottom"));
        print("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D4", "top"));
        print("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D6", "top"));
        print("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D8", "top"));
        print("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D10", "top"));
        print("");
        print(GameRules.IsCardCorrect(bottom0[0], "bottom"));
        ////////////////////////////////////////
    }

    // Update is called once per frame
    void Update(){
        
    }

    // Owen/Jenne Refactored 31-01-22 
    // Generate the deck from the two array lists
    public static List<string> GenerateDeck() {
        // Create a list to store the card (Suit/Value) in
        List<string> deck = new List<string>();
        // for each suit
        for (int s = 0; s < suits.Length; s++){
            // for each value
             for(int v = 0; v < values.Length; v++){
                 // adding the current suit/value to the deck
                 deck.Add(suits[s] + values[v]);
             }
        }

        // Return the shuffled deck
        return deck;
    }

    // Owen/Jenne Refactored 31-01-22 
    // Shuffle the deck of cards
    public static void Shuffle<T>(List<T> deck){
        // Random so that we don't get the same shuffle pattern each time
        System.Random random = new System.Random();
        // Create a copy of the deck to put shuffled cards in
        List<T> shuffledDeck = deck;

        // Cards left in deck to shuffle
        int cardsLeft = deck.Count;

        // While there are still cards left to shuffle
        while(cardsLeft > 0){
            // Get the index of the randomly selected card to move
            int cardIndex =  random.Next(deck.Count);
            // Get the card
            T cardToAdd = deck[cardIndex];
            // Add the card to the shuffled deck
            shuffledDeck.Add(cardToAdd);
            // Remove the card from the original deck
            deck.RemoveAt(cardIndex);
            // decrement the number of cards left
            cardsLeft--;
        }
        // Assign the deck back to the newly shuffled deck
        deck = shuffledDeck;
    }

    // Owen/Jenne Refactored 31-01-22 
    // Deal the cards onto the bottom display piles
   public void DealCards(){
       // define offsets in y and z axis
        float yOffset = 0;
        float zOffset = 0.03f;

        // define Game object newCard
        GameObject newCard = null;

       // for each card position, deal a card
        for (int row = 0; row < 7; row++){
            // remove the first pile from the list until there are no more piles
            for (int pile = row; pile < 7; pile++){
                // Add card to the pile
                bottoms[pile].Add(deck.Last<string>());
                // remove card from the deck of cards remaining
                deck.RemoveAt(deck.Count - 1);
                // create a game object of the card and 
                // assign it a position relative to the pile it is in
                newCard = Instantiate(cardPrefab, 
                    new Vector3(bottomPos[pile].transform.position.x, 
                                bottomPos[pile].transform.position.y - yOffset, 
                                bottomPos[pile].transform.position.z - zOffset), 
                    Quaternion.identity, // rotation = 0
                    bottomPos[pile].transform);
                // Assign the value of the new card to the current card    
                newCard.name = bottoms[pile][row];
                // determine if card should be face up or face  
                // (last card on pile is faceup)
                if (pile == row){
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
            }

            // Increase the offset with each card added
            yOffset = yOffset + 0.4f;
            zOffset = zOffset + 0.03f;
        }
    }
}