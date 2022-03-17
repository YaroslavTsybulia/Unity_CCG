using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 8; i++)
            list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);
        return list; //Рука вмещает в себя максимум 8 карт

    }
}

public class GameManagerScr : MonoBehaviour
{
    public static GameManagerScr Instance;
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand,
                     EnemyField, PlayerField;
    public GameObject CardPref;
    int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeTxt;
    public Button EndTurnBtn, SurrenderBtn;


    public int PlayerHP, EnemyHP;
    public TextMeshProUGUI PlayerHPTxt, EnemyHPTxt;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt, ResultReason;

    public AttackedHero EnemyHero, PlayerHero;

    public List<CardController> PlayerHandCards = new List<CardController>(),
                                PlayerFieldCards = new List<CardController>(),
                                EnemyHandCards = new List<CardController>(),
                                EnemyFieldCards = new List<CardController>();

    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    void Start()
    {
        StartGame();
    }

    public void RestartGame()
    {
        StopAllCoroutines();

        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);

        PlayerHandCards.Clear();
        PlayerFieldCards.Clear();
        EnemyHandCards.Clear();
        EnemyFieldCards.Clear();

        StartGame();
    }

    void StartGame()
    {
        Turn = 0;

        EndTurnBtn.interactable = true;

        CurrentGame = new Game();

        GiveHardCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHardCards(CurrentGame.PlayerDeck, PlayerHand);

        PlayerHP = EnemyHP = 8000;
        ShowHP();


        ResultGO.SetActive(false);

        StartCoroutine(TurnFunc());
    }

    void GiveHardCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 4)
            GiveCardToHand(deck, hand);
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;

        CreateCardPref(deck[0], hand);

        deck.RemoveAt(0);
    }

    void CreateCardPref(Card card, Transform hand) 
    {
        GameObject cardGO = Instantiate(CardPref, hand, false);
        CardController cardC = cardGO.GetComponent<CardController>();

        cardC.Init(card, hand == PlayerHand);

        if (cardC.IsPlayerCard)
            PlayerHandCards.Add(cardC);
        else
            EnemyHandCards.Add(cardC);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();

        foreach (var card in PlayerFieldCards)
            card.Info.HighlightCard(false);

        if (IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {
                card.Card.CanAttack = true;
                card.Info.HighlightCard(true);
            }
            while (TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }
            ChangeTurn();
        }
        else
        {
            foreach (var card in EnemyFieldCards)
                card.Card.CanAttack = true;

            StartCoroutine(EnemyTurn(EnemyHandCards));
        }
    }

    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);

        int count = cards.Count == 1 ? 1 :
                 Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 3 || EnemyHandCards.Count == 0)
                break; //Огранчение до 4 карт на поле


            cards[0].GetComponent<CardMovementScr>().MoveToField(EnemyField);

            yield return new WaitForSeconds(.31f);

            cards[0].Info.ShowCardInfo();
            cards[0].transform.SetParent(EnemyField);

            cards[0].OnCast();
        }

        yield return new WaitForSeconds(1);

        foreach (var activeCard in EnemyFieldCards.FindAll(x => x.Card.CanAttack))
        {
            if (Random.Range(0, 2) == 0 && PlayerFieldCards.Count > 0)
            {
                if (PlayerFieldCards.Count == 0)
                    break;

                var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

                activeCard.Card.CanAttack = false;

                activeCard.GetComponent<CardMovementScr>().MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.5f);

                CardsFight(enemy, activeCard);
            }
            else
            {
                activeCard.Card.CanAttack = false;

                activeCard.GetComponent<CardMovementScr>().MoveToTarget(PlayerHero.transform);
                yield return new WaitForSeconds(.5f);

                DamageHero(activeCard, false);
            }

            yield return new WaitForSeconds(.2f); // 0.2 секунды перед новой анимацией
        }

        yield return new WaitForSeconds(1);
        ChangeTurn();

    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;

        EndTurnBtn.interactable = IsPlayerTurn;

        if (IsPlayerTurn)
            GiveNewCards();

        StartCoroutine(TurnFunc());
    }

    public void SurrenderMove()
    {
        ResultGO.SetActive(true);
        StopAllCoroutines();

        ResultTxt.text = "YOU LOSE";
        ResultReason.text = "surrended";
    }


    void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }

    public void CardsFight(CardController player, CardController enemy)
    {
        player.Card.GetDamage(enemy.Card.Attack);
        player.OnDamageDeal();
        enemy.OnTakeDamage(player);

        enemy.Card.GetDamage(player.Card.Attack);
        player.OnTakeDamage();

        player.CheckForAlive();
        enemy.CheckForAlive();
    }


    void ShowHP()
    {
        EnemyHPTxt.text = EnemyHP.ToString();
        PlayerHPTxt.text = PlayerHP.ToString();
    }

    public void DamageHero(CardController card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
            EnemyHP = Mathf.Clamp(EnemyHP - card.Card.Attack, 0, int.MaxValue);
        else
            PlayerHP = Mathf.Clamp(PlayerHP - card.Card.Attack, 0, int.MaxValue);

        ShowHP();
        card.OnDamageDeal();
        CheckForResult();

    }


    void CheckForResult()
    {
        if (EnemyHP == 0 || PlayerHP == 0)
        {
            ResultGO.SetActive(true);
            StopAllCoroutines();

            if (EnemyHP == 0)
            {
              ResultTxt.text = "YOU WIN";
              ResultReason.text = "opponent's life points became 0";
            }
            else
            {
              ResultTxt.text = "YOU LOSE";
              ResultReason.text = "opponent's life points became 0";
            }
        }
    }

    public void HighlightTargets(bool highlight)
    {
        foreach (var card in EnemyFieldCards)
            card.Info.HighlightAsTarget(highlight);

        EnemyHero.HighlightAsTarget(highlight);
    }
}