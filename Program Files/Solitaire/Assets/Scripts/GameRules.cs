using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        
    }

    // SOL 9 - Rank of cards must be functional as per rules
    public static bool IsRankGoood(string stackCard, string selectedCard, string pos)
    {
        int cardRank = int.Parse(selectedCard.Substring(1, selectedCard.Length - 1));
        int stackRank = int.Parse(stackCard.Substring(1, stackCard.Length - 1));

        switch (pos)
        {
            case "top":
                return (cardRank - stackRank == 1);
            case "bottom":
                return (stackRank - cardRank == 1);
        }
        // This is the bad place
        return false;
    }

    // SOL 12 - Tableau cards can only be stacked in alternating colors
    public static bool IsAlternating(string stackCard, string selectedCard)
    {
        string cardSuit = selectedCard.Substring(0, 1);
        string stackSuit = stackCard.Substring(0, 1);

        switch (stackSuit)
        {
            // Black suits need red underneath
            case "C":
            case "S":
                return (cardSuit == "D" || cardSuit == "H");
            // Red suits need black underneath
            case "D":
            case "H":
                return (cardSuit == "C" || cardSuit == "S");
        }
        // This is the bad place
        return false;
    }

    // Checks if suit is the same
    public static bool IsSameSuit(string stackCard, string selectedCard)
    {
        string cardSuit = selectedCard.Substring(0, 1);
        string stackSuit = stackCard.Substring(0, 1);

        return stackSuit == cardSuit;
    }

    // Checks if the stack is empty (can be used for top or bottom)
    public static bool IsEmpty(Transform stack){
        //print("stack child count is " + stack.transform.childCount);
        return (stack.transform.childCount == 0);
    }

    // SOL 13 - Empty Tableau spots can only be filled with a king
    // SOL 14 - Foundations can only be filled starting with an ace
    public static bool IsEmptyRank(string card, string pos){
        int cardRank = int.Parse(card.Substring(1, card.Length - 1));
        switch (pos){
            case "top":
                return (cardRank == 1);
            case "bottom":
                return (cardRank == 13);
        }

        // This is the bad place
        return false;
    }
}
