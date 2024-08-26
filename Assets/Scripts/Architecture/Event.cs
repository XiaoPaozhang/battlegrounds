using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlegrounds
{
  // ----------------整个项目所有事件

  public struct UpdateGoodsEvent
  {
    public List<IMinionData> MinionDatas { get; private set; }
    /// <summary>
    /// 更新商品事件
    /// </summary>
    /// <param name="minionDatas">更新的随从商品数据</param>
    public UpdateGoodsEvent(List<IMinionData> minionDatas)
    {
      this.MinionDatas = minionDatas;
    }
  }

  public struct EndDragMinionEvent
  {
    public IMinionData MinionData { get; private set; }
    public PointerEventData EventData { get; private set; }
    public MinionUIItem MinionUIItem { get; private set; }
    public Transform ParentTf { get; private set; }
    /// <summary>
    /// 结束拖拽随从事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="minionData"></param>
    public EndDragMinionEvent(PointerEventData eventData, IMinionData minionData, MinionUIItem minionUIItem, Transform parentTf)
    {
      this.EventData = eventData;
      this.MinionData = minionData;
      this.MinionUIItem = minionUIItem;
      this.ParentTf = parentTf;
    }
  }
}
