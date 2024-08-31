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
    private Dictionary<int, IPlayerInfo> playerInfoData;
    private int playerId;
    private List<IMinionData> goodsMinionDatas;
    private IShopModel shopModel;
    private bool isLock;
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as ShopInfoPanelData ?? new ShopInfoPanelData();
      goodsMinionDatas = mData.goodsMinionDatas;
      shopModel = this.GetModel<IShopModel>();
      playerInfoData = this.GetModel<IPlayerInfoModel>().PlayerInfos;
      playerId = this.GetModel<IBattleModel>().PlayerId;

      shopModel.Star.RegisterWithInitValue(OnStarChanged);
      shopModel.Goods.CollectionChanged += OnGoodsChanged;

      upgradeBtn.onClick.AddListener(OnUpgradeBtnClick);
      refreshBtn.onClick.AddListener(OnRefreshBtnClick);
      lockBtn.onClick.AddListener(OnLockBtnClick);

      //摆放随从
      MinionSlot.UpdateMinionSlot(shopModel.Goods);
    }

    private void OnGoodsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems == null) return;
      //摆放随从
      MinionSlot.UpdateMinionSlot((ObservableCollection<IMinionData>)sender);


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
      //费用是否充足
      bool IsEnoughCost = playerInfoData[playerId].CurrentMp.Value >= 1;
      if (isLock)
      {
        "商店已锁住,无法刷新".LogInfo();
        return;
      }
      else if (!IsEnoughCost)
      {
        "费用不足,无法刷新".LogInfo();
        return;
      }
      // "刷新".LogInfo();

      //扣除费用
      playerInfoData[playerId].CurrentMp.Value -= 1;

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

      //商店清理随从商品
      shopModel.Goods.Clear();

      //商店添加刷新后的随从商品
      foreach (IMinionCardData minionCardData in minionCardDatas)
      {
        shopModel.Goods.Add(new MinionData(minionCardData, IMinionData.UiType.Shop));
      }
    }

    private void OnUpgradeBtnClick()
    {
      // "升级".LogInfo();

      //费用是否充足
      bool IsEnoughCost = playerInfoData[playerId].CurrentMp.Value >= shopModel.UpgradeCost;
      if (!IsEnoughCost)
      {
        "提升商店等级,费用不足".LogInfo();
        return;
      }
      //扣除费用
      playerInfoData[playerId].CurrentMp.Value -= shopModel.UpgradeCost;
      //升级商店等级
      shopModel.UpgradeStore();
      this.GetModel<IDeckModel>().PushCardsByMaxStar(shopModel.Star.Value);
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
