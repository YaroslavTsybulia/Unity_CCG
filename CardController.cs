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
