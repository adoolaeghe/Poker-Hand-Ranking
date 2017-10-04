using System;
using System.Collections.Generic;
using System.Linq;

public enum Result
{
	Win,
	Loss,
	Tie
}

public class PokerHand
{
	public string[] cards = new string[5];

	public PokerHand(string hand)
	{
		cards = hand.Split(' ');
	}

	public Result CompareWith(PokerHand hand)
	{
		//SAME HANDS
		if (this.cards == hand.cards) { return Result.Tie; }

		int[] myHand = HandValue(this);
		int[] adversaryHand = HandValue(hand);

		for (int i = 0, count = 0; i < myHand.Length; i++)
		{
			if (myHand[i] == adversaryHand[i])
			{
				count++;
				if (count == myHand.Length) { return Result.Tie; }
				continue;
			}

			if (myHand[i] > adversaryHand[i])
			{ return Result.Win; }
			else
			{ return Result.Loss; }
		}

		return Result.Tie;
	}

	public int[] HandValue(PokerHand hand)
	{
		int[] result = new int[3] { 0, 0, 0 };
		List<int> resList = result.ToList();
		List<int> higher = HigherCards(hand);  //Sorted hand (ascending).
		int[] poker = SameValue(higher, 4);
		int[] tris = SameValue(higher, 3);

		//STRAIGHT FLUSH
		if (IsStraight(higher) && IsFlush(hand))
		{
			result[0] = 9;
			result[1] = higher[higher.Count - 1];
			return result;
		}

		//POKER
		if (poker[0] != 0)  //Poker was found
		{
			result[0] = 8;
			result[1] = poker[0];
			result[2] = poker[2];
			return result;
		}

		//FULL
		if (tris[0] != 0)  //tris was found
		{

			for (int i = 0; i < 3; i++)
			{
				if (higher[i] == tris[0])
				{
					higher.RemoveRange(i, 3);
					break;
				}
			}
			if (higher[0] == higher[1])  //pair found
			{
				result[0] = 7;
				result[1] = tris[0];
				result[2] = higher[0];  //pair value
				return result;
			}
		}

	    //FLUSH
		if (IsFlush(hand))
		{
			resList[0] = 6;
			resList[1] = higher[higher.Count - 1];
			resList[2] = higher[higher.Count - 2];
			resList.Add(higher[higher.Count - 3]);
			resList.Add(higher[higher.Count - 4]);
			resList.Add(higher[higher.Count - 5]);
			result = resList.ToArray();
			return result;
		}

		//STRAIGHT
		if (tris[0] == 0 && IsStraight(higher))
		{
			result[0] = 5;
			result[1] = higher[higher.Count - 1];
			return result;
		}

		//TRIS
		if (tris[0] != 0)
		{
			result[0] = 4;
			result[1] = tris[0];
			result[2] = tris[2];
			return result;
		}

		//DOUBLE PAIR
		int[] pair = SameValue(higher, 2);
		List<int> pairHighs = HighestCards(pair, higher);
		if (pair[1] != 0)
		{
			resList[0] = 3;
			resList[1] = pair[1];
			resList[2] = pair[0];
			resList.Add(pairHighs[0]);
			result = resList.ToArray();
			return result;
		}

		//PAIR
		if (pair[0] != 0)
		{
			resList[0] = 2;
			resList[1] = pair[0];
			resList[2] = pair[2];
			resList.Add(pairHighs[1]);
			resList.Add(pairHighs[0]);
			result = resList.ToArray();
			return result;
		}

		//HIGH CARDS
		resList[0] = 1;
		for (int i = 4; i >= 0; i--)
		{
			resList.Add(higher[i]);
		}
		result = resList.ToArray();

		return result;
	}

	public bool IsFlush(PokerHand hand)
	{
		char suit = hand.cards[0][1];
		int count = 1;
		for (int i = 1; i < 5; i++, count++)  //Checks for same suits.
		{
			if (suit != hand.cards[i][1])
			{ break; }
		}
		return count == 5 ? true : false;
	}

	//Returns true if the hand is a straight
	public bool IsStraight(List<int> higher)
	{
		int lower = higher[0];
		bool isStraight = true;

		for (int i = 1; i < 5; i++)
		{
			if (higher[i] != lower + i)
			{
				isStraight = false;
				break;
			}
		}

		return isStraight;
	}

	//Returns the value of cards with same value(n of a kind).
	//Returns '0' if no cards with same value have been found.
	public int[] SameValue(List<int> higher, int n)
	{
		int[] res = new int[3] { 0, 0, 0 };
		int count = 1;

		for (int i = 0, j = 0; i < 4; i++)
		{
			if (higher[i] == higher[i + 1]) { count++; }
			else { count = 1; }

			if (count == n)
			{
				res[j] = higher[i];
				j++;
			}
		}
		List<int> highest = HighestCards(res, higher);
		res[2] = highest[highest.Count - 1];
		return res;
	}

	//Takes the value of "n of a kind" and returns a list of the high cards sorted.
	public List<int> HighestCards(int[] res, List<int> higher)
	{
		List<int> highest = new List<int>();

		for (int i = 0; i < 5; i++)
		{
			if (higher[i] != res[0] && higher[i] != res[1])
			{ highest.Add(higher[i]); }
		}
		highest.Sort();
		return highest;
	}

	//Returns the sorted hand starting from the lower card.
	public List<int> HigherCards(PokerHand hand)
	{
		List<int> list = new List<int>();

		for (int i = 0; i < 5; i++)
		{
			list.Add(GetValue(hand.cards[i][0]));
		}
		list.Sort();
		return list;
	}

	//Returns a value between 2 and 14(ace).
	public int GetValue(char c)
	{
		int res;

		switch (c)
		{
			case '2': res = 2; break;
			case '3': res = 3; break;
			case '4': res = 4; break;
			case '5': res = 5; break;
			case '6': res = 6; break;
			case '7': res = 7; break;
			case '8': res = 8; break;
			case '9': res = 9; break;
			case 'T': res = 10; break;
			case 'J': res = 11; break;
			case 'Q': res = 12; break;
			case 'K': res = 13; break;
			case 'A': res = 14; break;
			default: res = 0; break;
		}
		return res;
	}
}
