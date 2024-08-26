using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using System.Collections.Generic;

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
      upgradeBtn.onClick.AddListener(OnUpgradeBtnClick);
      refreshBtn.onClick.AddListener(OnRefreshBtnClick);
      lockBtn.onClick.AddListener(OnLockBtnClick);
      TypeEventSystem.Global.Register<UpdateGoodsEvent>(OnAddGoods).UnRegisterWhenGameObjectDestroyed(this);

      //摆放随从
      MinionSlot.UpdateMinionSlot(goodsMinionDatas);
    }

    private void OnAddGoods(UpdateGoodsEvent e)
    {
      if (e.MinionDatas == null)
      {
        "更新商品事件没有接收到数据".LogWarning();
        return;
      }
      //摆放随从
      MinionSlot.UpdateMinionSlot(e.MinionDatas);
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
          .RefreshDeck(goodsMinionDatas)
          .DrawCards<IMinionCardData>(drawCount);

      //商店添加随从
      this.GetModel<IShopModel>().UpdateGoods(minionCardDatas);
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
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }

  }
}
