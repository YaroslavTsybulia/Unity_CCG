using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackedCard : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManagerScr.Instance.IsPlayerTurn)
            return;

        CardController player = eventData.pointerDrag.GetComponent<CardController>(),
                       enemy = GetComponent<CardController>();

        if (player && player.Card.CanAttack && enemy.Card.IsPlaced)
        {
            if (GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation) &&
                !enemy.Card.IsProvocation)
                return;

            GameManagerScr.Instance.CardsFight(player, enemy);
        }
    }
}
