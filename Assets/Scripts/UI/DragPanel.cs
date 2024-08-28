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
  public partial class DragPanel : UIPanel, IController
  {
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as DragPanelData ?? new DragPanelData();
      // please add init code here
      TypeEventSystem.Global.Register<EndDragCardEvent>(OnEndDragCard).UnRegisterWhenGameObjectDestroyed(this);
    }

    public RectTransform shopRecycleArea;
    public RectTransform playerPlacementArea;
    public Transform handTransform;

    public void OnEndDragCard(EndDragCardEvent @event)
    {
      PlayerInfoPanel playerInfoPanel = UIKit.GetPanel<PlayerInfoPanel>();

      RectTransform handArea = playerInfoPanel.HandCardSlot.transform as RectTransform;
      RectTransform minionArea = playerInfoPanel.MinionSlot.transform as RectTransform;
      // 判断是否在手牌区域
      bool AtHandArea = RectTransformUtility.RectangleContainsScreenPoint(
          handArea,
          @event.EventData.position,
          @event.EventData.pressEventCamera
          );
      // 判断是否在随从可放置区域
      bool AtMinionArea = RectTransformUtility.RectangleContainsScreenPoint(
          minionArea,
          @event.EventData.position,
          @event.EventData.pressEventCamera
          );
      // 如果拖拽的是随从卡,且拖拽至随从区
      if (@event.BaseCardData is IMinionCardData minionCardData && AtMinionArea)
      {
        // "放置随从".LogInfo();
        IPlayerInfoModel playerInfoModel = this.GetModel<IPlayerInfoModel>();
        playerInfoModel.AddMinion(new MinionData(minionCardData, IMinionData.UiType.Player), 30001);

        playerInfoPanel.PlaceMinion(playerInfoModel.PlayerInfos[30001].Minions.ToList());
        playerInfoPanel.HandCardSlot.DestroyCard(@event.CardUIItem);

        playerInfoPanel.HandCardSlot.DealCard(@event.CardUIItem.gameObject, false);
      }
      // 如果拖拽的是法术卡,且拖拽至手牌区外
      else if (@event.BaseCardData is ISpellCardData && !AtHandArea)
      {
        //拖拽到手牌区域外
        if (!AtHandArea)
        {
          "放置法术".LogInfo();
        }
      }
      else
      {
        // "卡牌归位".LogInfo();

        // 卡牌不在可放置区域, 回到原来的位置
        @event.CardUIItem.transform.Parent(@event.ParentTf);
        playerInfoPanel.HandCardSlot.DealCard(@event.CardUIItem.gameObject, false);
      }
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

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}
