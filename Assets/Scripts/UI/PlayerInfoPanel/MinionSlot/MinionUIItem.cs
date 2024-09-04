/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.EventSystems;
using System.Collections.ObjectModel;
using System.Linq;

namespace Battlegrounds
{
  public partial class MinionUIItem : UIElement, IDragHandler, IBeginDragHandler, IEndDragHandler, IController
  {
    private IMinionData _minionData;
    private Transform parentTf;
    private int siblingIndex;
    private Dictionary<int, IPlayerInfo> playerInfoData;
    private int playerId;
    private bool IsEnoughCost;
    private bool IsPlayerMinion;
    private bool IsBelongsToShop;
    private bool IsRecruitMinionProcess;//是否是招募随从流程
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
      IsRecruitMinionProcess = this.GetModel<IBattleModel>().Fsm.CurrentStateId == BattleProcess.States.RecruitMinion;
      if (!IsRecruitMinionProcess) return;

      // 是否为玩家的随从
      IsPlayerMinion = _minionData.BelongsTo == IMinionData.UiType.Player;
      //是否是商店拿出的随从
      IsBelongsToShop = _minionData.BelongsTo == IMinionData.UiType.Shop;
      playerInfoData = this.GetModel<IPlayerInfoModel>().PlayerInfos;
      playerId = this.GetModel<IBattleModel>().PlayerId;
      //费用是否充足
      IsEnoughCost = playerInfoData[playerId].CurrentMp.Value >= 3;
      if (IsBelongsToShop && !IsEnoughCost)
      {
        "商店随从购买,费用不足".LogInfo();
        return;
      }
      parentTf = transform.parent;
      siblingIndex = transform.GetSiblingIndex();
      Transform dragPanelTf = UIKit.GetPanel<DragPanel>().transform;
      transform.SetParent(dragPanelTf);
      transform.position = Input.mousePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
      if (IsBelongsToShop && !IsEnoughCost) return;
      if (!IsRecruitMinionProcess) return;

      //拖拽
      Vector3 mousePosition = eventData.position;
      mousePosition = this.GetUtility<IScreenUtils>().ClampToScreenBounds(mousePosition);
      transform.position = mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
      if (IsBelongsToShop && !IsEnoughCost) return;
      if (!IsRecruitMinionProcess) return;

      // 检查是否拖拽到了目标区域
      CheckForDropTarget(eventData, _minionData, siblingIndex);
    }

    private void CheckForDropTarget(PointerEventData @event, IMinionData minionData, int siblingIndex)
    {
      ShopInfoPanel ShopInfoPanel = UIKit.GetPanel<ShopInfoPanel>();
      PlayerInfoPanel PlayerInfoPanel = UIKit.GetPanel<PlayerInfoPanel>();
      // 判断是否在可放置区域
      bool AthandArea = RectTransformUtility.RectangleContainsScreenPoint(
          PlayerInfoPanel.HandCardSlot.transform as RectTransform,
          @event.position,
          @event.pressEventCamera
          );
      // 判断是否在垃圾回收区域
      bool AtRecoveryArea = RectTransformUtility.RectangleContainsScreenPoint(
         ShopInfoPanel.recoveryArea,
          @event.position,
          @event.pressEventCamera
          );

      // 如果拖拽到了玩家信息UI，执行生成卡牌操作
      if (IsBelongsToShop && AthandArea)
      {
        // 获取卡牌配置表数据
        cfg.Card card = this.GetUtility<ITableLoader>().Tables.TbCard.Get(minionData.Id);
        MinionCardData minionCardData = new MinionCardData(card);

        //将随从加入手牌
        this.GetModel<IPlayerInfoModel>().GetPlayerInfo(playerId).HandCards.Add(minionCardData);
        //减少玩家MP
        playerInfoData[playerId].CurrentMp.Value -= 3;
        //删除商店商品随从
        ObservableCollection<IMinionData> Goods = this.GetModel<IShopModel>().Goods;
        var goodToRemove = Goods.First(good => good.Id == minionData.Id);
        if (goodToRemove != null) Goods.Remove(goodToRemove);

        //销毁拖拽随从
        gameObject.DestroySelf();
      }
      // 如果拖拽到了商店垃圾回收的UI，执行购买操作
      else if (IsPlayerMinion && AtRecoveryArea)
      {
        // "随从卖出".LogInfo();
        IPlayerInfo playerInfo = this.GetModel<IPlayerInfoModel>().PlayerInfos[30001];
        var MinionsRemove = playerInfo.Minions.First(minion => minion.Id == minionData.Id);
        if (MinionsRemove != null) playerInfo.Minions.Remove(MinionsRemove);

        gameObject.DestroySelf();
        //增加玩家MP
        playerInfoData[playerId].CurrentMp.Value += 1;
      }
      else
      {
        // 如果没有目标UI，执行取消操作
        // 卡牌不在可放置区域, 回到原来的位置
        gameObject.Parent(parentTf);//设置原父级
        transform.SetSiblingIndex(siblingIndex);//设置原索引
      };
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }

}