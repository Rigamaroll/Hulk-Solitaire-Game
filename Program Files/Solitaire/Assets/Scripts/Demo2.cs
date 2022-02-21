using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
    }

    // Test SOL 6 The stockpile must be displayed and functional
    public static void TestSol6(List<string> stockPile, List<string> talonPile){
        Demo2.TestSol57(stockPile);
        Demo2.TestSol58(stockPile, talonPile);
    }

    // Test SOL 57 If Stockpile not empty, deal card
    public static void TestSol57(List<string> stockPile){
        TheLogger.PrintLog("Test SOL57 Deal a card if the stockpile is not empty");
        if (!GameRules.IsEmpty(stockPile)){
            TheLogger.PrintLog("Stockpile is Not Empty. A card should be dealt.");
        }
        else {
            TheLogger.PrintLog("Stockpile is Empty. The talon cards should be " +
            "returned to the stockpile.");
        }
    }

    // Test SOL 58 If Stockpile empty, return talon pile to stockpile in same order
    public static void TestSol58(List<string> stockPile, List<string> talonPile){
        // TO BE TESTED ON THE TALON PILE FLIPPING ALGORITHM
    }

    // Test SOL15 foundations can only be stacked by matching suits
    public static void TestSol15(List<string> stackCard){
        List<string> testCards = new List<string> {"D1", "H1", "S1", "C1"};       
        TheLogger.PrintLog("// Test SOL15 foundations can only be stacked by matching suits");
        
        for (int i = 0; i < (testCards.Count); i++){
            // Test foundations
            TheLogger.PrintLog("Cards " + testCards[i] + " : " + 
                stackCard[stackCard.Count - 1] + 
                " - Top test Selected Card is the same suit as stack card: " + 
                GameRules.IsSameSuit(stackCard,testCards[i]) );    
        }
    } 

    // Test SOL16 foundations can only be stacked in ascending order
    public static void TestSol16(List<string> stackCard){
        List<string> testCards = new List<string> {"D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10",
                                                    "D11", "D12", "D13"};       
        TheLogger.PrintLog("// Test SOL16 foundations can only be stacked in ascending order");
        
        for (int i = 0; i < (testCards.Count); i++){
            // Test foundations
            TheLogger.PrintLog("Cards " + testCards[i] + " : " + 
                stackCard[stackCard.Count - 1] + 
                " - Top test Selected Card is one higher than stack card: " + 
                GameRules.IsRankGoood(stackCard,testCards[i], "top") );    
        }
    }



}
