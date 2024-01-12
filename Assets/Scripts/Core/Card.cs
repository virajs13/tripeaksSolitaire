using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TriPeaksSolitaire.Core
{
    
    public class Card
    {
        public enum Suit
        {
            HEART,
            DIAMOND,
            SPADES,
            CLUB
        }
        private int m_value;
        private Suit m_suit;
        private bool m_isFaceUp;


        public Card(int value, Suit suit)
        {
            m_value = value;
            m_suit = suit;
        }

        public bool IsFaceUp => m_isFaceUp;

        public int Value => m_value;
        
        public void SetFaceUp(bool isFaceUp)
        {
            m_isFaceUp = isFaceUp;
        }
        
        public override string ToString()
        {
            return $"{m_value} of {m_suit}";
        }
        
        public int CompareTo(Card otherCard)
        {
            //As per game requirement, if diff between values is 1, return 1 else 0 
            //For special case of ACE, check value of other card 

            if ((m_value == 14 && otherCard.Value is 2 or 13) ||
                (m_value is 2 or 13 && otherCard.Value == 14) ||
                Mathf.Abs(m_value - otherCard.Value) == 1)
            {
                return 1;
            }

            return 0;
        }
        
    }
    
}

