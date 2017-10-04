# Poker-Hand-Ranking

An algorithm that compare a poker hand to an other and returns "Win", "Loss" or "Tie". 

### How to use 

Create a poker hand that has a method to compare itself to another poker hand:

```
 Result PokerHand.CompareWith(PokerHand hand)
 ```
 
A poker hand has a constructor that accepts a string containing 5 cards:

```
 PokerHand hand = new PokerHand("KS 2H 5C JD TD");
 ```
 
The characteristics of the string of cards are:
 * A space is used as card seperator
 * Each card consists of two characters
* The first character is the value of the card, valid characters are: 
2, 3, 4, 5, 6, 7, 8, 9, T(en), J(ack), Q(ueen), K(ing), A(ce)
* The second character represents the suit, valid characters are: 
S(pades), H(earts), D(iamonds), C(lubs)
