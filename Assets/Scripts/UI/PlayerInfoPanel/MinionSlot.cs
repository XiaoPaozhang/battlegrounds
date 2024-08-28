/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.ObjectModel;

namespace Battlegrounds
{
  public partial class MinionSlot : UIElement
  {
    private void Awake()
    {
    }
    /// <summary>
    /// 更新随从槽
    /// </summary>
    /// <param name="minionData">随从数据</param>
    public void UpdateMinionSlot(ObservableCollection<IMinionData> minionDatas)
    {
      // 先清空之前的随从UI
      foreach (Transform child in transform)
      {
        Destroy(child.gameObject);
      }

      // 再创建新的随从UI
      foreach (var minionData in minionDatas)
      {
        MinionUIItem.Instantiate()
                    .Position(transform.position)
                    .Parent(this)
                    .InitMinionDisplay(minionData);// 更新随从UI
      }
    }
    protected override void OnBeforeDestroy()
    {
    }
  }
}