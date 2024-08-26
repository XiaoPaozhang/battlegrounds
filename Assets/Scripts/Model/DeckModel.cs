using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public interface IDeckModel : IModel
  {
    DeckModel InitializeDeck(int maxStar);
    DeckModel PushCardsByMaxStar(int maxStar);
    DeckModel ShuffleDeck();
    IBaseCardData[] DrawCards(int count);
    T[] DrawCards<T>(int count) where T : class, IBaseCardData;
    IBaseCardData[] DrawAllCards();
    T[] DrawAllCards<T>() where T : class, IBaseCardData;
    DeckModel RefreshDeck(List<IMinionData> minionDatas);
    DeckModel ClearDeck();
  }

  public class DeckModel : AbstractModel, IDeckModel
  {
    private bool _initialized;
    // 卡牌数据
    private List<IBaseCardData> _baseCardDatas = new List<IBaseCardData>();
    private Dictionary<int, cfg.Card> _cardsDict = new Dictionary<int, cfg.Card>();
    private Dictionary<CardType, Func<cfg.Card, IBaseCardData>> _cardFactory = new Dictionary<CardType, Func<cfg.Card, IBaseCardData>>();

    protected override void OnInit()
    {
      // 初始化工厂字典
      _cardFactory.Add(CardType.Minion, card => new MinionCardData(card));
      _cardFactory.Add(CardType.Spell, card => new SpellCardData(card));
      _cardFactory.Add(CardType.Weapon, card => new WeaponCardData(card));
    }

    public DeckModel InitializeDeck(int maxStar)
    {
      if (_initialized)
      {
        "已经初始化过了，不需要再初始化".LogWarning();
      }
      else
      {
        _cardsDict = this.GetUtility<ITableLoader>().Tables.TbCard.DataMap;
        _initialized = true;
      }

      PushCardsByMaxStar(maxStar);
      return this;
    }

    /// <summary>
    /// 塞入牌库指定星级的卡牌。
    /// </summary>
    /// <param name="maxStar">最大星级数</param>
    /// <returns></returns>
    public DeckModel PushCardsByMaxStar(int maxStar)
    {
      if (!_initialized)
      {
        throw new InvalidOperationException("牌库尚未初始化，请先调用 InitializeDeck 方法");
      }
      ClearDeck();
      List<int> cardIds = new List<int>();
      foreach (var card in _cardsDict)
      {
        if (card.Value.Star > maxStar) continue;
        cardIds.Add(card.Value.Id);
      }

      AddCardToDeck(cardIds, 3);
      return this;
    }
    private void AddCardToDeck(List<int> cardIds, int count = 1)
    {
      foreach (int cardId in cardIds)
      {
        cfg.Card card = _cardsDict[cardId];
        if (!_cardFactory.ContainsKey((CardType)card.CardType))
        {
          throw new NotSupportedException($"不支持的卡牌类型 {card.CardType}");
        }

        Func<cfg.Card, IBaseCardData> factoryMethod = _cardFactory[(CardType)card.CardType];

        for (int i = 0; i < count; i++)
        {
          _baseCardDatas.Add(factoryMethod(card));
        }
      }
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    /// <returns></returns>
    public DeckModel ShuffleDeck()
    {
      var count = _baseCardDatas.Count;
      for (int i = count - 1; i > 0; i--)
      {
        int j = UnityEngine.Random.Range(0, i + 1);
        IBaseCardData temp = _baseCardDatas[i];
        _baseCardDatas[i] = _baseCardDatas[j];
        _baseCardDatas[j] = temp;
      }
      return this;
    }

    /// <summary>
    /// 抽取卡牌。
    /// </summary>
    /// <param name="count">抽取数量</param>
    /// <returns>抽取到的卡牌数据数组。</returns>
    public IBaseCardData[] DrawCards(int count) => DrawCards<IBaseCardData>(count);
    /// <summary>
    /// 抽取卡牌。
    /// </summary>
    /// <param name="count">抽取数量</param>
    /// <typeparam name="T">卡牌类型</typeparam>
    public T[] DrawCards<T>(int count) where T : class, IBaseCardData
    {
      if (_baseCardDatas.Count == 0)
      {
        LogKit.W("牌库已空，无法抽卡");
        return new T[0];
      }

      T[] resultCardDatas = new T[Mathf.Min(count, _baseCardDatas.Count)];
      int drawCount = resultCardDatas.Length;

      for (int i = 0; i < drawCount; i++)
      {
        IBaseCardData drawnCard = _baseCardDatas[0];
        if (drawnCard is T)
        {
          // 移除这张卡牌
          _baseCardDatas.RemoveAt(0);
          resultCardDatas[i] = drawnCard as T;
        }
      }
      return resultCardDatas;
    }

    /// <summary>
    /// 抽取所有卡牌
    /// </summary>
    /// <returns></returns>
    public IBaseCardData[] DrawAllCards() => DrawAllCards<IBaseCardData>();

    /// <summary>
    /// 抽取所有卡牌。
    /// </summary>
    /// <typeparam name="T">卡牌类型</typeparam>
    /// <returns>抽取到的所有卡牌数据数组。</returns>
    public T[] DrawAllCards<T>() where T : class, IBaseCardData
    {
      if (_baseCardDatas.Count == 0)
      {
        LogKit.W("牌库已空，无法抽卡");
        return new T[0];
      }

      List<T> resultCardDatas = new List<T>();

      while (_baseCardDatas.Count > 0)
      {
        IBaseCardData drawnCard = _baseCardDatas[0];
        if (drawnCard is T)
        {
          // 移除这张卡牌
          _baseCardDatas.RemoveAt(0);
          resultCardDatas.Add(drawnCard as T);
        }
      }

      return resultCardDatas.ToArray();
    }

    /// <summary>
    /// 刷新牌库
    /// </summary>
    /// <param name="minionDatas">回收的随从数据</param>
    /// <returns></returns>
    public DeckModel RefreshDeck(List<IMinionData> minionDatas)
    {
      List<int> cardIds = new List<int>();
      foreach (var minionData in minionDatas)
      {
        cardIds.Add(minionData.Id);
      }
      AddCardToDeck(cardIds);
      ShuffleDeck();
      return this;
    }

    /// <summary>
    /// 清空牌库
    /// </summary>
    /// <returns></returns>
    public DeckModel ClearDeck()
    {
      _baseCardDatas.Clear();
      return this;
    }
  }
}
