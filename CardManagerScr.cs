using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name, CardTypeText;
    public Sprite Logo, Element, CardType;
    public int Attack, Defense;
    public bool CanAttack;
    public bool IsPlaced;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }

    public Card(string name, string type, string logoPath, string typePath, string elementPath, int attack, int defense)
    {
        Name = name;
        CardTypeText = type;
        Logo = Resources.Load<Sprite>(logoPath);
        Element = Resources.Load<Sprite>(elementPath);
        CardType = Resources.Load<Sprite>(typePath);
        Attack = attack;
        Defense = defense;
        CanAttack = false;
        IsPlaced = false;
    }

    public void GetDamage(int damage)
    {
        Defense -= damage;
    }
}

public static class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerScr : MonoBehaviour
{
    public void Awake()
    {
        CardManager.AllCards.Add(new Card("Luchador1", "[Link/Warrior]", "Sprite/Archtypes/Luchador1", "Sprite/Card types/Link Card", "Sprite/Elements/Earth", 3000, 2500));
        CardManager.AllCards.Add(new Card("Luchador2", "[Warrior]", "Sprite/Archtypes/Luchador2", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 2500, 1500));
        CardManager.AllCards.Add(new Card("Luchador3", "[Magnet/Warrior]", "Sprite/Archtypes/Luchador3", "Sprite/Card types/Magnet Card", "Sprite/Elements/Earth", 1800, 1800));
        CardManager.AllCards.Add(new Card("Luchador4", "[Warrior]", "Sprite/Archtypes/Luchador4", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 2500, 1000));
        CardManager.AllCards.Add(new Card("Luchador5", "[Link/Warrior]", "Sprite/Archtypes/Luchador5", "Sprite/Card types/Link Card", "Sprite/Elements/Earth", 2300, 3000));
        CardManager.AllCards.Add(new Card("Luchador6", "[Warrior]", "Sprite/Archtypes/Luchador6", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 1500, 1200));
        CardManager.AllCards.Add(new Card("Luchador7", "[Magnet/Warrior]", "Sprite/Archtypes/Luchador7", "Sprite/Card types/Magnet Card", "Sprite/Elements/Earth", 2000, 2500));
    }
}
