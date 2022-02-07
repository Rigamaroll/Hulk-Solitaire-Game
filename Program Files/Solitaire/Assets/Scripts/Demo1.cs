using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
    }

    // Test SOL9 that you can only stack one lower on the bottom, one higher on the top
    public static void TestSol9(List<string> stackCard){
        List<string> testCards = new List<string> {"D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10",
                                                    "D11", "D12", "D13"};       
        TheLogger.PrintLog("// SOL 9 - Rank of cards must be functional as per rules");
        TheLogger.PrintLog("----- Test for rank -----");
        
        for (int i = 0; i < (testCards.Count); i++){
            // Test bottom
            TheLogger.PrintLog("Cards " + testCards[i] + " : " + 
                stackCard[stackCard.Count - 1] + 
                " - Bottom test Selected Card is one lower than stack card: " + 
                GameRules.IsRankGoood(stackCard,testCards[i], "bottom") );
            // Test top
            TheLogger.PrintLog("Cards " + testCards[i] + " : " + 
                stackCard[stackCard.Count - 1] + 
                " - Top test Selected Card is one higher than stack card: " + 
                GameRules.IsRankGoood(stackCard,testCards[i], "top") );    
        }
    }

    // Test SOL 10 A full deck of card that can be shuffled
    public static void TestSol10(List<string> deck){

        TheLogger.PrintLog("// SOL 10 - Deck consists of 52 shuffled cards");
        TheLogger.PrintLog("----- Test for full, shuffled deck -----");

        for (int i = 0; i < deck.Count; i++){
                TheLogger.PrintLog("Card: " + deck[i]);
        }
        TheLogger.PrintLog("Total Cards: " + deck.Count);
    }

    // Test SOL 11 Cards are subtracted from teh deck when dealt out
    public static void TestSol11(List<string> deck){

        TheLogger.PrintLog("// SOL 11 - Dealt cards are subtracted from deck");
        TheLogger.PrintLog("----- Test for deck after deal -----");

        for (int i = 0; i < deck.Count; i++){
                TheLogger.PrintLog("Card: " + deck[i]);
        }
        TheLogger.PrintLog("Total Cards: " + deck.Count + " = 24: " + (deck.Count == 24));
    }

    // Test SOL 12 that you can only stack alternating colours
    public static void TestSol12(List<string> stackCard){
        List<string> testCards = new List<string> {"D4", "H3", "C1", "S13"};

        TheLogger.PrintLog("// SOL 12 - Tableau cards can only be stacked in alternating colors");
        TheLogger.PrintLog("----- Test for alternating colours -----");

        for (int i = 0; i < (testCards.Count); i++){
            TheLogger.PrintLog("Cards " + testCards[i] + " : " + 
                stackCard[stackCard.Count - 1] + 
                " - Cards are alternating colours: " + 
                GameRules.IsAlternating(stackCard,
                testCards[i]) );
        }
    }

    // Test SOL 13 empty tableau spots can only be filled with King
    public static void TestSol13(){
        List<string> testCards = new List<string> {"D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10",
                                                    "D11", "D12", "D13"}; 

        TheLogger.PrintLog("// SOL 13 - Empty Tableau spots can only be filled with a king");
        TheLogger.PrintLog("----- Test for rank of card -----");

        for (int i = 0; i < (testCards.Count); i++){
            TheLogger.PrintLog("Card " + testCards[i] + " : " + 
                " - Card is a King: " + 
                GameRules.IsCardCorrect(testCards[i], "bottom" ));
        }
    } 

    // Test SOL 14 empty foundation spots can only be filled with Ace
    public static void TestSol14(){
        List<string> testCards = new List<string> {"D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10",
                                                    "D11", "D12", "D13"}; 

        TheLogger.PrintLog("// SOL 14 - Foundations can only be filled starting with an ace");
        TheLogger.PrintLog("----- Test for rank of card -----");

        for (int i = 0; i < (testCards.Count); i++){
            TheLogger.PrintLog("Card " + testCards[i] + " : " + 
                " - Card is an Ace: " + 
                GameRules.IsCardCorrect(testCards[i], "top" ));
        }
    } 
}
