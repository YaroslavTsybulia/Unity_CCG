using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card Card;
    public bool IsPlayerCard;
    public CardInfoScr Info;
    public CardMovementScr Movement;
    public CardAbility Ability;

    GameManagerScr gameManager;

    public void Init(Card card,bool isPlayerCard )
    {
        Card = card;
        gameManager = GameManagerScr.Instance;
        IsPlayerCard = isPlayerCard;

        if (isPlayerCard)
        {
            Info.ShowCardInfo();
            GetComponent<AttackedCard>().enabled = false;
        }
        else
            Info.HideCardInfo();
    }

    public void OnCast()
    {
        if (Card.IsSpell && Card.SpellTarget != Card.TargetType.NO_TARGET)
            return;

        if (IsPlayerCard)
        {
            gameManager.PlayerHandCards.Remove(this);
            gameManager.PlayerFieldCards.Add(this);
        }
        else
        {
            gameManager.EnemyHandCards.Remove(this);
            gameManager.EnemyFieldCards.Add(this);
            Info.ShowCardInfo();
        }
        Card.IsPlaced = true;

        if (Card.HasAbility)
            Ability.OnCast(); //Если у карты есть спопособности, вызываем это

        if(Card.IsSpell)
            UseSpell(null);
    }

    public void OnTakeDamage(CardController player = null)
    {
        CheckForAlive();
        Ability.OnDamageTake(player);
    }

    public void OnDamageDeal()
    {
        Card.TimesDealedDamage++;
        Card.CanAttack = false;
        Info.HighlightCard(false);

        if (Card.HasAbility)
            Ability.OnDamageDeal();
    }
    
    public void UseSpell(CardController target)
    {
        switch (Card.Spell)
        {
         /*   case Card.SpellType.HEAL_PLAYER_FIELD_CARDS:

                var playerCards = IsPlayerCard ?
                                  gameManager.PlayerFieldCards :
                                  gameManager.EnemyFieldCards;
                foreach (var card in playerCards)
                {
                    card.Card.Defense += Card.SpellVallue;
                    card.Info.RefreshData();
                }
                break;
            
            case Card.SpellType.DAMAGE_ENEMY_FIELD_CARDS:
                var enemyCards = IsPlayerCard ?
                                 new List<CardController>(gameManager.PlayerFieldCards):
                                 new List<CardController>(gameManager.EnemyFieldCards);
                foreach (var card in enemyCards)
                    GiveDamageTo(card, Card.SpellVallue);
                break;*/
            
            case Card.SpellType.HEAL_PALYER_HERO:
                if (IsPlayerCard)
                    gameManager.PlayerHP += Card.SpellValue;
                else
                    gameManager.EnemyHP += Card.SpellValue;

                gameManager.ShowHP();
                break;
            
            case Card.SpellType.DAMAGE_ENEMY_HERO:
                if (IsPlayerCard)
                    gameManager.EnemyHP -= Card.SpellValue;
                else
                    gameManager.PlayerHP -= Card.SpellValue;

                gameManager.ShowHP();
                gameManager.CheckForResult();
                break;
            
            case Card.SpellType.HEAL_PLAYER_CARD:
                target.Card.Defense += Card.SpellValue;
                GiveDamageTo(target, Card.SpellValue);
                break;
            
            case Card.SpellType.SHIELD:
                if (!target.Card.Abilities.Exists(x => x == Card.AbilityType.SHIELD))
                    target.Card.Abilities.Add(Card.AbilityType.SHIELD);
                break;
            
            case Card.SpellType.PROVOCATION:
                if (!target.Card.Abilities.Exists(x => x == Card.AbilityType.PROVOCATION))
                    target.Card.Abilities.Add(Card.AbilityType.PROVOCATION);
                break;

            case Card.SpellType.BUFF_CARD_DAMAGE:
                target.Card.Attack += Card.SpellValue;
                break;

            case Card.SpellType.DEBUFF_CARD_DAMAGE:
                target.Card.Attack = Mathf.Clamp(target.Card.Attack - Card.SpellValue, 0, int.MaxValue);
                break;

        }

        if (target != null)
        {
            target.Ability.OnCast();
            target.CheckForAlive();
        }
        DestroyCard();     
    }

    void GiveDamageTo(CardController card, int damage)
    {
        card.Card.GetDamage(damage);
        card.CheckForAlive();
        card.OnTakeDamage();
    }

    public void CheckForAlive()
    {
        if (Card.IsAlive)
            Info.RefreshData();
        else
            DestroyCard();
    }

    void DestroyCard()
    {
        Movement.OnEndDrag(null);

        RemoveCardFromList(gameManager.EnemyFieldCards);
        RemoveCardFromList(gameManager.EnemyHandCards);
        RemoveCardFromList(gameManager.PlayerFieldCards);
        RemoveCardFromList(gameManager.PlayerHandCards);

        Destroy(gameObject);

    }

    void RemoveCardFromList(List<CardController> list)
    {
        if (list.Exists(x => x == this))
            list.Remove(this);
    }
}
