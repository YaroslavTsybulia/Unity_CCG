using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD_1,
    SELF_FIELD_2,
    SELF_FIELD_SPELL,
    ENEMY_HAND,
    ENEMY_FIELD_1,
    ENEMY_FIELD_2,
    ENEMY_FIELD_SPELL,
}

public class DropPlaceScr : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;

    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD_1)
            return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();

        if (card && GameManagerScr.Instance.IsPlayerTurn &&
            !card.Card.IsPlaced)
        {
                card.Movement.DefaultParent = transform;
                card.OnCast();       
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD_1 ||
            Type == FieldType.ENEMY_HAND || Type == FieldType.SELF_HAND)
            return;

        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();

        if (card)
            card.DefaultTempCardParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardMovementScr card = eventData.pointerDrag.GetComponent<CardMovementScr>();

        if (card && card.DefaultTempCardParent == transform)
            card.DefaultTempCardParent = card.DefaultParent;
    }
}