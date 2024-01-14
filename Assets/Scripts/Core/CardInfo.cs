using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TriPeaksSolitaire.Core
{
    public enum Suit
    {
        Hearts = 1,
        Clubs = 2,
        Spades = 3,
        Diamonds = 4
    }
        
    public enum Value 
    {
        Ace   = 1,
        Two   = 2,
        Three = 3,
        Four  = 4,
        Five  = 5,
        Six   = 6,
        Seven = 7,
        Eight = 8,
        Nine  = 9,
        Ten   = 10,
        Jack  = 11,
        Queen = 12,
        King  = 13
    }
    public class CardInfo
    {
       
        private Value value;
        private Suit suit;


        public CardInfo(Value value, Suit suit)
        {
            this.value = value;
            this.suit = suit;
        }

        public Suit Suit => suit;

        public Value Value => value;
        
        public static IEnumerable<CardInfo> EnumerateCardInfo()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    yield return new CardInfo(value,suit);
                }
            }
        }

        public static CardInfo GetRandomCardInfo()
        {
            var value = (Value)Random.Range(1, 14);
            var suit = (Suit)Random.Range(1, 5);
            return new CardInfo(value, suit);
        }
        
        public override string ToString()
        {
            return $"{value} of {suit}";
        }

        public int GetCardId()
        {
            return ((int)Suit - 1) * 13 + ((int)Value - 1);
        }

        public string GetCardResourcePath()
        {
            return value switch
            {
                Value.Ace => $"card{suit}_A",
                Value.Jack => $"card{suit}_J",
                Value.Queen => $"card{suit}_Q",
                Value.King => $"card{suit}_K",
                _ => $"card{suit}_{(int)value}"
            };
        }
        
        
        
        
    }
    
}

