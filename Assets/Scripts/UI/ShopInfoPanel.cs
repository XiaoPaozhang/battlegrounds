using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Battlegrounds
{
  public class ShopInfoPanelData : UIPanelData
  {
    public List<IMinionData> goodsMinionDatas;
  }




  public partial class ShopInfoPanel : UIPanel, IController
  {
    private List<IMinionData> goodsMinionDatas;
    private IShopModel shopModel;
    private bool isLock;
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as ShopInfoPanelData ?? new ShopInfoPanelData();
      goodsMinionDatas = mData.goodsMinionDatas;
      shopModel = this.GetModel<IShopModel>();

      shopModel.Star.RegisterWithInitValue(OnStarChanged);
      shopModel.Goods.CollectionChanged += OnGoodsChanged;

      upgradeBtn.onClick.AddListener(OnUpgradeBtnClick);
      refreshBtn.onClick.AddListener(OnRefreshBtnClick);
      lockBtn.onClick.AddListener(OnLockBtnClick);
      TypeEventSystem.Global.Register<EndDragMinionEvent>(OnEndDragMinion).UnRegisterWhenGameObjectDestroyed(this);

      //摆放随从
      MinionSlot.UpdateMinionSlot(shopModel.Goods);
    }

    private void OnGoodsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems == null) return;
      //遍历新值
      // for (int i = 0; i < e.NewItems.Count; i++)
      // {
      // switch (e.Action)
      // {
      //   case NotifyCollectionChangedAction.Add:
      //     LogKit.I($"Item added: {item}");
      //     break;
      //   case NotifyCollectionChangedAction.Remove:
      //     LogKit.I($"Item removed: {item}");
      //     break;
      //   case NotifyCollectionChangedAction.Replace:
      //     LogKit.I($"Item replaced.");
      //     break;
      //   case NotifyCollectionChangedAction.Move:
      //     LogKit.I($"Item moved.");
      //     break;
      //   case NotifyCollectionChangedAction.Reset:
      //     LogKit.I("The collection was reset.");
      //     break;
      // }
      // }
      //摆放随从
      MinionSlot.UpdateMinionSlot((ObservableCollection<IMinionData>)sender);


    }

    private void OnEndDragMinion(EndDragMinionEvent @event)
    {
      //判断是否为玩家的随从
      bool IsPlayerMinion = @event.MinionData.BelongsTo == IMinionData.UiType.Player;
      // 判断是否在垃圾回收区域
      bool AtRecoveryArea = RectTransformUtility.RectangleContainsScreenPoint(
          recoveryArea,
          @event.EventData.position,
          @event.EventData.pressEventCamera
          );

      if (IsPlayerMinion && AtRecoveryArea)
      {
        // "随从卖出".LogInfo();
        IPlayerInfo playerInfo = this.GetModel<IPlayerInfoModel>().PlayerInfos[30001];
        var MinionsRemove = playerInfo.Minions.First(minion => minion.Id == @event.MinionData.Id);
        if (MinionsRemove != null) playerInfo.Minions.Remove(MinionsRemove);

        @event.MinionUIItem.gameObject.DestroySelf();
      }
    }

    private void OnLockBtnClick()
    {
      "锁住".LogInfo();
      isLock = !isLock;
      Image lockBtnBorderImg = lockBtn.GetComponent<Image>();
      lockBtnBorderImg.color = isLock ? Color.red : Color.white;
    }

    private void OnRefreshBtnClick()
    {
      if (isLock)
      {
        "商店已锁住,无法刷新".LogInfo();
        return;
      }
      "刷新".LogInfo();

      // 获取抽取数量
      int drawCount =
      this.GetModel<IBattleModel>()
          .CalculateShopDrawMinionCount();

      // 刷新随从,重新抽取
      IMinionCardData[] minionCardDatas =
      this.GetModel<IDeckModel>()
          .AddCardToDeck(shopModel.Goods.Select(goods => goods.Id).ToList())
          .ShuffleDeck()
          .DrawCards<IMinionCardData>(drawCount);

      //商店添加随从
      foreach (IMinionCardData minionCardData in minionCardDatas)
      {
        shopModel.Goods.Add(new MinionData(minionCardData, IMinionData.UiType.Shop));
      }
    }

    private void OnUpgradeBtnClick()
    {
      "升级".LogInfo();
      //获取当前星级
      int star = this.GetModel<IShopModel>().UpgradeStore().Star.Value;
      this.GetModel<IDeckModel>().PushCardsByMaxStar(star);
    }

    private void OnStarChanged(int value)
    {
      starTxt.text = value.ToString();
      refreshCostTxt.text = shopModel.RefreshCost.ToString();
      upgradeCostTxt.text = shopModel.UpgradeCost.ToString();
      if (shopModel.IsMaxStar())
      {
        upgradeBtn.gameObject.SetActive(false);
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
      shopModel.Goods.CollectionChanged -= OnGoodsChanged;
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }

  }
}
