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

    public enum SpellType 
    {
      NO_SPELL,
      HEAL_PLAYER_FIELD_CARDS,
      DAMAGE_ENEMY_FIELD_CARDS,
      HEAL_PALYER_HERO,
      DAMAGE_ENEMY_HERO,
      HEAL_PLAYER_CARD,
      DAMAGE_ENEMY_CARD,
      SHIELD,
      PROVOCATION,  
      BUFF_CARD_DAMAGE,
      DEBUFF_CARD_DAMAGE,
    }

    public enum TargetType
    {
      NO_TARGET,
      PLAYER_CARD_TARGET,
      ENEMY_CARD_TARGET
    }

    public string Name, CardTypeText;
    public Sprite Logo, Element, CardType;
    public int Attack, Defense;
    public bool CanAttack;
    public bool IsPlaced;

    public List<AbilityType> Abilities;
    public SpellType Spell;
    public TargetType SpellTarget;
    public int SpellValue; 


    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public bool IsSpell
    {
        get
        {
            return Spell != SpellType.NO_SPELL;
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

    public Card(string name, string type, string logoPath, string typePath, string elementPath, int attack, int defense, AbilityType abilityType = 0, SpellType spellType = 0, int spellVal = 0, TargetType targetType = 0)
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

        Spell = spellType;
        SpellTarget = targetType;
        SpellValue = spellVal;

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

    public Card GetCopy()
    {
        Card card = this;
        card.Abilities = new List<AbilityType>(Abilities);
        return card;
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
        CardManager.AllCards.Add(new Card("L Luchador - Rookie", "[Link/Warrior]", "Sprite/Archtypes/Rookie", "Sprite/Card types/Link Card", "Sprite/Elements/Earth", 1500, 1200, Card.AbilityType.NO_ABILTY));
        CardManager.AllCards.Add(new Card("L Luchador - Counter Master", "[Warrior]", "Sprite/Archtypes/CounterMaster", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 1900, 2300, Card.AbilityType.COUNTER_ATTACK));
        CardManager.AllCards.Add(new Card("L Luchador - Dirty Heel", "[Magnet/Warrior]", "Sprite/Archtypes/DirtyHeel", "Sprite/Card types/Magnet Card", "Sprite/Elements/Earth", 2100, 1500, Card.AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("L Luchador - Bully", "[Warrior]", "Sprite/Archtypes/Bully", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 2100, 1800, Card.AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(new Card("L Luchador - Champion", "[Link/Warrior]", "Sprite/Archtypes/MainSuperstar", "Sprite/Card types/Link Card", "Sprite/Elements/Earth", 3000, 2500, Card.AbilityType.PROVOCATION));
        CardManager.AllCards.Add(new Card("L Luchador - Partner", "[Warrior]", "Sprite/Archtypes/Partner", "Sprite/Card types/Normal Card", "Sprite/Elements/Earth", 2300, 2100, Card.AbilityType.REGENERATION));
        CardManager.AllCards.Add(new Card("L Luchador - Powerhouse", "[Magnet/Warrior]", "Sprite/Archtypes/Powerhouse", "Sprite/Card types/Magnet Card", "Sprite/Elements/Earth", 2000, 3000, Card.AbilityType.SHIELD));

        CardManager.AllCards.Add(new Card("Emergency Case", "[Link/Warrior]", "Sprite/Spells/EmergencyCase", "Sprite/Card types/Spell Card", "Sprite/Elements/Spell", 0, 0, 0, 
            Card.SpellType.HEAL_PALYER_HERO, 1000, Card.TargetType.NO_TARGET));
        CardManager.AllCards.Add(new Card("Straight Punch", "[Link/Warrior]", "Sprite/Spells/StraightPunch", "Sprite/Card types/Spell Card", "Sprite/Elements/Spell", 0, 0, 0, 
            Card.SpellType.DAMAGE_ENEMY_HERO, 1000, Card.TargetType.NO_TARGET));
    }
}
