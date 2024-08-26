/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;

namespace Battlegrounds
{
  public partial class MinionUIItem : UIElement, IDragHandler, IBeginDragHandler, IEndDragHandler
  {
    private IMinionData _minionData;
    private Transform parentTf;
    private void Awake()
    {
    }
    /// <summary>
    /// 注册更新随从的显示信息
    /// </summary>
    /// <param name="minionData">随从数据</param>
    public void InitMinionDisplay(IMinionData minionData)
    {
      _minionData = minionData;
      minionData.Name.RegisterWithInitValue(OnNameChanged);
      minionData.Atk.RegisterWithInitValue(OnAtkChanged);
      minionData.CurrentHp.RegisterWithInitValue(OnHpChanged);
      minionData.Star.RegisterWithInitValue(OnStarChanged);
    }

    private void OnStarChanged(int star)
    {
      starTxt.text = star.ToString();
    }

    private void OnNameChanged(string name)
    {
      nameTxt.text = name;
    }

    private void OnHpChanged(int hp)
    {
      hpTxt.text = hp.ToString();
    }

    private void OnAtkChanged(int atk)
    {
      atkTxt.text = atk.ToString();
    }


    protected override void OnBeforeDestroy()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      parentTf = transform.parent;
      Transform dragPanelTf = UIKit.GetPanel<DragPanel>().transform;
      transform.SetParent(dragPanelTf);
      transform.position = Input.mousePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
      //拖拽
      transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
      TypeEventSystem.Global.Send(new EndDragMinionEvent(eventData, _minionData, this, parentTf));
    }
  }
}