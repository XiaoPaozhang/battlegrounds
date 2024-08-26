/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Battlegrounds
{
  public partial class HandCardSlot : UIElement
  {
    [SerializeField, Header("移动到目标位置延迟时间")] private float moveDuration;
    [SerializeField, Header("卡牌旋转动画持续时间")] private float dealDuration;
    [SerializeField, Header("每张卡牌动画间隔时间")] private float delayBetweenCards;
    [SerializeField, Header("卡牌宽度")] private float cardWidth;
    [SerializeField, Header("卡牌起始生成位置y轴偏移")] private float cardStartingYOffset;
    private List<CardUIItem> cardUIItemCache = new List<CardUIItem>();
    private static int _currentCardCount = 0;
    private void Awake()
    {
    }


    //获得卡牌显示效果 获得
    public void CardGettingDisplay(IBaseCardData cardData)
    {
      // CardUIItem.Instantiate()
      //           .Parent(this)
      //           .LocalPosition(Vector3.zero + new Vector3(0, cardStartingYOffset, 0))
      //           .LocalScale(Vector3.one)
      //           .LocalRotation(Quaternion.identity)
      //           .InitCardDisplay(cardData); 

      CardUIItem cardMono = Instantiate(CardUIItem);

      GameObject cardgo = cardMono.gameObject;
      cardgo.transform.SetParent(transform, false);
      cardgo.transform.localPosition = Vector3.zero + new Vector3(0, cardStartingYOffset, 0);
      cardgo.transform.localScale = Vector3.one;
      cardgo.transform.localRotation = Quaternion.identity;
      //默认显示背面(配合动画)
      cardgo.transform.Find("CardBack").gameObject.SetActive(true);

      CardUIItem.InitCardDisplay(cardData);

      //添加缓存
      cardUIItemCache.Add(CardUIItem);
      _currentCardCount++;

      DealCard(cardgo, delayBetweenCards, GetCardPosition(_currentCardCount - 1));
    }

    /// <summary>
    /// 这是游戏刚开始发牌的动画,需要一个
    /// </summary>
    /// <param name="card">卡牌游戏对象</param>
    /// <param name="delay">延迟时间</param>
    /// <param name="targetPos">目标位置</param>
    private void DealCard(GameObject card, float delay, Vector3 targetPos)
    {
      Quaternion initialRotation = card.transform.rotation;

      Sequence sequence = DOTween.Sequence();

      sequence.AppendInterval(delay);

      sequence.Append(card.transform.DOMove(targetPos, moveDuration));
      sequence.Append(card.transform.DORotate(new Vector3(0, 90, 0), dealDuration / 2));
      // 重新排序
      for (int i = 0; i < cardUIItemCache.Count; i++)
      {
        sequence.Join(cardUIItemCache[i].transform.DOMove(GetCardPosition(i), moveDuration));
      }
      sequence.AppendCallback(() => card.transform.Find("CardBack").gameObject.SetActive(false));
      sequence.Append(card.transform.DORotate(initialRotation.eulerAngles, dealDuration / 2));

    }

    // 计算卡牌位置的辅助方法
    private Vector3 GetCardPosition(int index)
    {
      // 计算总宽度
      float totalWidth = _currentCardCount * cardWidth;
      // 计算第一张卡牌的起始位置
      float startingX = transform.position.x - totalWidth / 2 + cardWidth / 2;
      float cardX = startingX + index * cardWidth;
      return new Vector3(cardX, transform.position.y, transform.position.z);
    }
    protected override void OnBeforeDestroy()
    {
    }
  }
}