using Godot;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

public partial class CardScene : Node2D
{
    private CardObj[] InitialDeckOfCards {get; set;} 
    private List<CardObj> RecordDeckOfCards = new List<CardObj>();
    private Stack<CardObj> DeckOfCards {get; set;} = new Stack<CardObj>();
    private int InitialDeckLength {get; set;}
    private Random rand = new Random();

    public override void _Ready() {
        base._Ready();
        StreamReader sr = new StreamReader("./Card/basicCard.json");
        string jsonObj = sr.ReadToEnd();
        var cardsObj = JsonConvert.DeserializeObject<List<CardObj>>(jsonObj);
        InitialDeckOfCards = new CardObj[cardsObj.Count * 2];
        int index = 0;
        foreach (var card in cardsObj) {
                int _id = index;
                card.id = _id;
                InitialDeckOfCards[index] = card;
                index++;
            if (card.copies > 0) {
                for (int i = 0; i < card.copies; i++) {
                    int _id1 = index;
                    CardObj newCard = new CardObj() {
                        id = _id1,
                        cardId = card.cardId,
                        color = card.color,
                        xCoord = card.xCoord,
                        copies = card.copies,
                        topFunction = card.topFunction,
                        bottomFunction = card.bottomFunction
                    };
                    InitialDeckOfCards[index] = card;
                    index++;
                }
            }
        };
        InitialDeckLength = DeckOfCards.Count;
        MapGen.shuffleArray(rand, InitialDeckOfCards);
        foreach (var card in InitialDeckOfCards) {
            if (card != null) {
                DeckOfCards.Push(card);
                RecordDeckOfCards.Add(card);
            }
        }
        InitialDeckLength = RecordDeckOfCards.Count;
    }


    public void DrawCard()
    {
        CardObj card = DeckOfCards.Pop();
        int posMultiplier = InitialDeckLength - DeckOfCards.Count;
        int xPos = -300 - (100 * posMultiplier);
        card.Position = new Vector2I(xPos, -500);
        AddChild(card);
    }

}
