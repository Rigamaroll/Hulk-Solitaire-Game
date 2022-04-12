using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Autocomplete : MonoBehaviour
{
    public static Autocomplete instance;
    public Button Solve;
    
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        print("Auto Complete here");
        Solve.gameObject.SetActive(false);
    }
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanBeAutoCompleted(){
        // Talon Pile is Empty
        if (GameObject.Find("TalonPile").transform.childCount > 0)
        {
            return;
        }

        // Stock Pile is empty
        if (GameObject.Find("DeckButton").transform.childCount > 0)
        {
            return;
        }

        // Check if All Tableau Cards are turned over
        // get an array of all the cards 
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        for (int i = 0; i < 52; i++)
        {
            if (!cards[i].GetComponent<Selectable>().IsFaceUp())
            {
                return;
            }
        }

        print("Can be autocompleted");
        SetVisible(true);
    }

    public void SetVisible(bool isVisible )
    {
        Solve.gameObject.SetActive(isVisible);
    }

    public void SolveGameButton()
    {
        StartCoroutine(SolveGame());
    }

    public IEnumerator SolveGame()
    {
        print("Button pushed");
        
        //get an array of all the cards 
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        //Dictionary AKA Map to hold the cards to be accessible by name
        Dictionary<string, GameObject> cardMap = new Dictionary<string, GameObject>();

        //load the Dictionary
        for (int i = 0; i < 52; i++)
        {
            cardMap.Add(cards[i].name, cards[i]);  
        }

        //getting the correct cards to bounce in order
        string[] suits = { "C", "D", "H", "S" };
        GameObject card;
        UserInput userInput = FindObjectOfType<UserInput>();

        for (int i = 1; i <= 13; i++)
        {
            for (int s = 0; s < suits.Length; s++)
            {
                //check if card is in tableau
                cardMap.TryGetValue(suits[s] + i, out card);
                // print(card.name);
                //if so, move to foundation
                if (card.transform.root.name.Equals("Bottom")){
                    userInput.SetClickedObject(card.transform);
                    userInput.GoodFoundationMove();
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }
    SetVisible(false);
    }
}
