/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;

namespace Battlegrounds
{
  public partial class HandCardSlot : UIElement
  {
    [SerializeField, Header("移动到目标位置延迟时间")] private float moveDuration;
    [SerializeField, Header("卡牌旋转动画持续时间")] private float dealDuration;
    [SerializeField, Header("每张卡牌动画间隔时间")] private float delayBetweenCards;
    [SerializeField, Header("卡牌宽度")] private float cardWidth;
    [SerializeField, Header("卡牌起始生成位置y轴偏移")] private float cardStartingYOffset;
    private static int _currentCardCount = 0;
    private void Awake()
    {
    }


    //获得卡牌显示效果 获得
    public void CardGettingDisplay(IBaseCardData cardData)
    {
      // CardUIItem.Instantiate()
      //           .Parent(this)
      //           .LocalPosition(Vector3.zero)
      //           .LocalScale(Vector3.one)
      //           .LocalRotation(Quaternion.identity)
      //           .InitCardDisplay(cardData);

      CardUIItem cardMono = Instantiate(CardUIItem, transform.position + new Vector3(0, cardStartingYOffset, 0), transform.rotation, transform);
      //默认显示背面(配合动画)
      cardMono.transform.Find("CardBack").gameObject.SetActive(true);

      cardMono.InitCardDisplay(cardData);

      _currentCardCount++;

      //默认显示背面(配合动画)
      cardMono.transform.Find("CardBack").gameObject.SetActive(true);

      DealCard(cardMono.gameObject);
    }

    /// <summary>
    /// 这是游戏刚开始发牌的动画,需要一个
    /// </summary>
    /// <param name="card">卡牌游戏对象</param>
    /// <param name="isFill">是否需要翻转</param>
    public void DealCard(GameObject card, bool isFill = true)
    {
      Quaternion initialRotation = card.transform.rotation;

      Sequence sequence = DOTween.Sequence();
      //延时
      sequence.AppendInterval(delayBetweenCards);
      //更新位置 从手牌区中央开始移动到计算位置
      for (int i = 0; i < transform.childCount; i++)
      {
        sequence.Join(transform.GetChild(i).transform.DOMove(GetCardPosition(i), moveDuration));
      }

      if (isFill)
      {
        //旋转一半,看不见
        sequence.Append(card.transform.DORotate(new Vector3(0, 90, 0), dealDuration / 2));
        //显示正面
        sequence.AppendCallback(() => card.transform.Find("CardBack").gameObject.SetActive(false));
        //旋转回来 看的见
        sequence.Append(card.transform.DORotate(initialRotation.eulerAngles, dealDuration / 2));
      }

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

    //清除指定卡牌缓存
    public void DestroyCard(CardUIItem cardUIItem)
    {
      Destroy(cardUIItem.gameObject);
      _currentCardCount--;
    }


    protected override void OnBeforeDestroy()
    {
    }
  }
}