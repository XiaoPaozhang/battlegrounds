/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Battlegrounds
{
  public partial class ManaSlot : UIElement
  {
    // 显示法力值数量
    private int mpCount = 10;
    private GameObject[] manaItems;
    private void Awake()
    {
      manaItems = new GameObject[mpCount];

      for (int i = 0; i < mpCount; i++)
      {
        manaItems[i] = transform.Find($"manaItem ({i})").gameObject;
        manaItems[i].SetActive(false);
      }
    }
    public void UpdateMpDisplay(int currentMp)
    {
      for (int i = 0; i < mpCount; i++)
      {
        manaItems[i].SetActive(i < currentMp);
      }
    }
    protected override void OnBeforeDestroy()
    {
    }
  }
}