using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public enum AbilityType
    {
        NO_ABILTY,
        INSTANT_ACTIVE,
        DOUBLE_ATTACK,
        SHIELD,
        PROVOCATION,
        REGENERATION,
        COUNTER_ATTACK 
    }

    public string Name, CardTypeText;
    public Sprite Logo, Element, CardType;
    public int Attack, Defense;
    public bool CanAttack;
    public bool IsPlaced;

    public List<AbilityType> Abilities;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public bool HasAbility
    {
        get
        {
            return Abilities.Count > 0;
        }
    }
    public bool IsProvocation
    {
        get
        {
            return Abilities.Exists(x => x == AbilityType.PROVOCATION);
        }
    }

    public int TimesDealedDamage;

    public Card(string name, string type, string logoPath, string typePath, string elementPath, int attack, int defense, AbilityType abilityType = 0)
    {
        Name = name;
        CardTypeText = type;
        Logo = Resources.Load<Sprite>(logoPath);
        Element = Resources.Load<Sprite>(elementPath);
        CardType = Resources.Load<Sprite>(typePath);
        Attack = attack;
        Defense = defense;
        CanAttack = false; //возможность аттаковать
        IsPlaced = false;

        Abilities = new List<AbilityType>();

        if (abilityType != 0)
            Abilities.Add(abilityType);

        TimesDealedDamage = 0;
    }

    public void GetDamage(int damage)
    {
        if (damage > 0)
        {
            if (Abilities.Exists(x => x == AbilityType.SHIELD))
                Abilities.Remove(AbilityType.SHIELD);
            else
                Defense -= damage;
        }

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
        CardManager.AllCards.Add(new Card("L Luchador - Rookie", "[Link/Warrior]", "Sprite/Archtypes/Rookie", "Sprite/Card types/Link Card", "Sprite/Elements/Earth", 1500, 1200,
            Card.AbilityType.NO_ABILTY));
        CardManager.AllCards.Add(new Card("L Luchador - Counter Master", "[Warrior]", "Sprite/Archtypes/CounterMaster", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 1900, 2300,
            Card.AbilityType.COUNTER_ATTACK));
        CardManager.AllCards.Add(new Card("L Luchador - Dirty Heel", "[Magnet/Warrior]", "Sprite/Archtypes/DirtyHeel", "Sprite/Card types/Magnet Card", "Sprite/Elements/Earth", 2100, 1500,
            Card.AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("L Luchador - Bully", "[Warrior]", "Sprite/Archtypes/Bully", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 2100, 1800,
            Card.AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(new Card("L Luchador - Champion", "[Link/Warrior]", "Sprite/Archtypes/MainSuperstar", "Sprite/Card types/Link Card", "Sprite/Elements/Earth", 3000, 2500, 
            Card.AbilityType.PROVOCATION));
        CardManager.AllCards.Add(new Card("L Luchador - Partner", "[Warrior]", "Sprite/Archtypes/Partner", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 2300, 2100,
            Card.AbilityType.REGENERATION));
        CardManager.AllCards.Add(new Card("L Luchador - Powerhouse", "[Magnet/Warrior]", "Sprite/Archtypes/Powerhouse", "Sprite/Card types/Magnet Card", "Sprite/Elements/Earth", 2000, 3000,
            Card.AbilityType.SHIELD));
    }
}
