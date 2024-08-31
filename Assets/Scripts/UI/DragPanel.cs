using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;
using System;
using System.Linq;

namespace Battlegrounds
{
  public class DragPanelData : UIPanelData
  {
  }
  public partial class DragPanel : UIPanel
  {
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as DragPanelData ?? new DragPanelData();
    }

    protected override void OnOpen(IUIData uiData = null)
    {
    }

    protected override void OnShow()
    {
    }

    protected override void OnHide()
    {
    }

    protected override void OnClose()
    {
    }
  }
}
