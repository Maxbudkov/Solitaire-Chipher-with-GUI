using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Cryptography
{
    public class Solitaire
    {
        Int32[] _deck;
        private String _passPhrase;

        public String PassPhrase { set { _passPhrase = value; } }

        public Solitaire(String passPhrase)
        {
            _deck = new Int32[54];
            for (Int32 x = 0; x < 54; x++) _deck[x] = x + 1;
            KeyTheDeck(passPhrase);
        }

        public Solitaire()
        {
            _deck = new Int32[54];
            for (Int32 x = 0; x < 54; x++) _deck[x] = x + 1;
        }

        public String Encrypt(String msg)
        {
            CleanMessage(ref msg);
            return Convert(msg, true);
        }

        public String Decrypt(String msg)
        {
            CleanMessage(ref msg);
            return Convert(msg, false);
        }

        public String GetDeck()
        {
            StringBuilder sb = new StringBuilder(152);
            for (int x = 0; x < 54; x++)
            {
                sb.Append(_deck[x]);
                sb.Append("|");
            }
            return sb.ToString();
        }

        protected void CleanMessage(ref String msg)
        {
            msg = msg.ToUpper();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[^A-Z]", options);
            if (regex.IsMatch(msg)) msg = regex.Replace(msg, "");
        }

        protected String Convert(String message, Boolean encode)
        {
            StringBuilder sb = new StringBuilder();
            Int32 curChar, curKey;
            Int32 x = 0;
            while (x < message.Length)
            {
                PushAndCut();
                curChar = ((Int32)message[x] - 64);
                curKey = _deck[0];

                if (curKey == 54)
                    curKey = _deck[53]; // JokerB's value is 53
                else
                    curKey = _deck[curKey];

                if (encode)
                    curChar = (curChar + curKey);
                else
                { // decode
                    if (curChar < curKey) curChar += 26;
                    curChar = (curChar - curKey);
                }
                if (curKey < 53) // if the card is not a joker then get the output char
                {
                    if (curChar > 26) curChar %= 26;
                    if (curChar < 1) curChar += 26;
                    sb.Append((char)(curChar + 64));
                    x++;
                }
            }
            return sb.ToString();
        }

        protected void KeyTheDeck(String passPhrase)
        {
            // Set the starting state of the deck based on a passphrase
            CleanMessage(ref passPhrase);
            Int32 curChar;
            for (Int32 x = 0; x < passPhrase.Length; x++)
            {
                PushAndCut();
                curChar = (Int32)passPhrase[x] - 65;
                CountCut(curChar + 1);
            }
        }

        protected void PushAndCut()
        {
            MoveCardDown(53);
            MoveCardDown(54);
            MoveCardDown(54);
            TripleCut();
            CountCut();
        }

        protected void MoveCardDown(Int32 card)
        {
            Int32 pos1 = Array.IndexOf(_deck, card);
            if (pos1 == 53)
            {
                BottomToTop();
                MoveCardDown(card);
            }
            else
            {
                _deck[pos1] = _deck[pos1 + 1];
                _deck[pos1 + 1] = card;
            }
        }

        protected void BottomToTop()
        {
            Int32 card = _deck[53];
            for (int x = 53; x > 0; x--)
                _deck[x] = _deck[x - 1];
            _deck[0] = card;
        }

        protected void TripleCut()
        {
            // Swaps the section before the top joker with the section after the bottom joker
            Int32 jokerTop = Array.IndexOf(_deck, 53);
            Int32 jokerBottom = Array.IndexOf(_deck, 54);
            if (jokerTop > jokerBottom)
            {
                Int32 hold = jokerTop;
                jokerTop = jokerBottom;
                jokerBottom = hold;
            }
            Int32[] newDeck = new Int32[54];
            Int32 lengthBottom = 53 - jokerBottom;
            Int32 lengthMiddle = jokerBottom - jokerTop - 1;
            Array.Copy(_deck, jokerBottom + 1, newDeck, 0, lengthBottom);
            Array.Copy(_deck, jokerTop, newDeck, lengthBottom, lengthMiddle + 2);
            Array.Copy(_deck, 0, newDeck, lengthBottom + lengthMiddle + 2, jokerTop);
            newDeck.CopyTo(_deck, 0);
        }

        protected void CountCut(Int32 cutPos)
        {
            // Cut the deck at cutPos, leave the bottom card intact.
            Int32[] newDeck = new Int32[54];
            if (cutPos < 53)
            { // don't cut if the card is a joker
                Array.Copy(_deck, cutPos, newDeck, 0, 53 - (cutPos));
                Array.Copy(_deck, 0, newDeck, 53 - (cutPos), cutPos);
                newDeck[53] = _deck[53];
                newDeck.CopyTo(_deck, 0);
            }
        }

        protected void CountCut()
        {
            CountCut(_deck[53]);
        }
    }
}