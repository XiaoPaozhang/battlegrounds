using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;
using UnityEngine.Networking;

namespace Battlegrounds
{
  public class AnimeImageBackGroundData : UIPanelData
  {
  }
  public partial class AnimeImageBackGround : UIPanel
  {
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as AnimeImageBackGroundData ?? new AnimeImageBackGroundData();
      // please add init code here
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
