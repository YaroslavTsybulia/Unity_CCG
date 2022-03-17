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
            GameManagerScr.Instance.CardsFight(player, enemy);
        }
    }
}