using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

namespace Battlegrounds
{
  public class EnemyInfoPanelData : UIPanelData
  {
    public IPlayerInfo PlayerInfo { get; set; }
  }
  public partial class EnemyInfoPanel : UIPanel
  {
    private IPlayerInfo enemyInfo;
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as EnemyInfoPanelData ?? new EnemyInfoPanelData();
      enemyInfo = mData.PlayerInfo;

      enemyInfo.CurrentHp.RegisterWithInitValue(OnCurrentHpChanged).UnRegisterWhenGameObjectDestroyed(this);
    }

    private void OnCurrentHpChanged(int value)
    {
      hp.text = value.ToString();
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
