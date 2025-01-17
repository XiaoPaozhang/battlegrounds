using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlegrounds
{
  // ----------------整个项目所有事件

  // public struct EndDragMinionEvent
  // {
  //   public PointerEventData EventData { get; private set; }
  //   public IMinionData MinionData { get; private set; }
  //   public MinionUIItem MinionUIItem { get; private set; }
  //   public Transform ParentTf { get; private set; }
  //   public int OriginalIndex { get; private set; }
  //   /// <summary>
  //   /// 结束拖拽随从事件
  //   /// </summary>
  //   /// <param name="eventData">拖拽事件数据</param>
  //   /// <param name="minionData">随从数据</param>
  //   /// <param name="minionUIItem">随从对象</param>
  //   /// <param name="parentTf">源父对象位置</param>
  //   public EndDragMinionEvent(PointerEventData eventData, IMinionData minionData, MinionUIItem minionUIItem, Transform parentTf, int index)
  //   {
  //     this.EventData = eventData;
  //     this.MinionData = minionData;
  //     this.MinionUIItem = minionUIItem;
  //     this.ParentTf = parentTf;
  //     this.OriginalIndex = index;
  //   }

  // }

  // public struct EndDragCardEvent
  // {
  //   public PointerEventData EventData { get; private set; }
  //   public IBaseCardData BaseCardData { get; private set; }
  //   public CardUIItem CardUIItem { get; private set; }
  //   public Transform ParentTf { get; private set; }
  //   /// <summary>
  //   /// 结束拖拽随从事件
  //   /// </summary>
  //   /// <param name="eventData">拖拽事件数据</param>
  //   /// <param name="baseCardData">卡牌数据</param>
  //   /// <param name="cardUIItem">卡牌对象</param>
  //   /// <param name="parentTf">源父对象位置</param>
  //   public EndDragCardEvent(PointerEventData eventData, IBaseCardData baseCardData, CardUIItem cardUIItem, Transform parentTf, Action cancelAction = null)
  //   {
  //     this.EventData = eventData;
  //     this.BaseCardData = baseCardData;
  //     this.CardUIItem = cardUIItem;
  //     this.ParentTf = parentTf;
  //   }
  // }
}
