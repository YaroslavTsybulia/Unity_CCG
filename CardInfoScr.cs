using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public CardController CC;

    public Image Logo, Element, CardType;
    public TextMeshProUGUI Name, CardTypeText, Attack, Defense;
    public GameObject HideObj, HighlightedObj;
    public Color NormalCol, TargetCol, SpellTargetCol;


    public void HideCardInfo()
    {
        HideObj.SetActive(true);
    }

    public void ShowCardInfo()
    {
        HideObj.SetActive(false);

        Logo.sprite = CC.Card.Logo;
        Element.sprite = CC.Card.Element;
        CardType.sprite = CC.Card.CardType;
        Logo.preserveAspect = true;
        Name.text = CC.Card.Name;
        CardTypeText.text = CC.Card.CardTypeText;

        RefreshData();
    }

    public void RefreshData()
    {
        Attack.text = CC.Card.Attack.ToString();
        Defense.text = CC.Card.Defense.ToString();
    }

    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ?
                                      TargetCol :
                                      NormalCol;
    }

    public void HighlightCard(bool highlight)
    {
        HighlightedObj.SetActive(highlight);
    }

    public void HighlightAsSpellTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ?
                                      SpellTargetCol :
                                      NormalCol;
    }

}
