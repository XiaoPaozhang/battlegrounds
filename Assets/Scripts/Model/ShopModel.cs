using System;
using System.Collections;
using System.Collections.Generic;
using cfg;
using QFramework;

namespace Battlegrounds
{
  public struct EventA
  {

  }
  public interface IShopModel : IModel
  {
    BindableProperty<int> Star { get; }
    int RefreshCost { get; }
    int UpgradeCost { get; }
    ShopModel InitializeShop(int star);
    ShopModel UpgradeStore();
    ShopModel RefreshStore();
    bool IsMaxStar();
    List<IMinionData> Goods { get; }
    List<IMinionData> UpdateGoods(IMinionCardData[] cards);
  }
  public class ShopModel : AbstractModel, IShopModel
  {
    private bool _isInitialized = false;
    private cfg.shop.TbShop _tbShop;//商店数据表
    public BindableProperty<int> Star { get; private set; } = new BindableProperty<int>(); //星级
    public int RefreshCost { get; private set; } //刷新所需要的金币数量
    public int UpgradeCost { get; private set; } //升级所需要的金币数量
    public List<IMinionData> Goods { get; private set; } = new List<IMinionData>(); //商品随从列表
    public ShopModel InitializeShop(int star)
    {
      _tbShop = this.GetUtility<ITableLoader>().Tables.TbShop;
      if (_isInitialized)
      {
        "shop model 已经初始化过".LogWarning();
      }
      else
      {
        _isInitialized = true;
        Star.Value = star;
      }

      Star.RegisterWithInitValue(OnStarChaneged);
      return this;
    }
    public ShopModel UpgradeStore()
    {
      //升级费用为负数，表示已经是最大星级
      if (IsMaxStar())
      {
        "商店已经是最大星级".LogWarning();
      }
      else
      {
        Star.Value++;
      }
      return this;
    }

    public ShopModel RefreshStore()
    {
      return this;
    }
    private void OnStarChaneged(int Value)
    {
      cfg.Shop _shop = _tbShop.Get(Value);
      RefreshCost = _shop.RefreshCost;
      UpgradeCost = _shop.UpgradeCost;
    }

    /// <summary>
    /// 商店是否已经是最大星级
    /// </summary>
    /// <returns></returns>
    public bool IsMaxStar() => _tbShop.Get(Star.Value).UpgradeCost < 0;
    /// <summary>
    /// 更新商品列表
    /// </summary>
    /// <param name="cards"></param>
    /// <returns>商品列表</returns>
    public List<IMinionData> UpdateGoods(IMinionCardData[] cards)
    {
      ClearGoods();

      foreach (var minionCardData in cards)
      {
        Goods.Add(new MinionData(minionCardData)
        {
          BelongsTo = IMinionData.UiType.Shop
        });
      }
      TypeEventSystem.Global.Send(new UpdateGoodsEvent(Goods));
      return Goods;
    }
    /// <summary>
    /// 清空商品列表
    /// </summary>
    /// <returns></returns>
    public void ClearGoods()
    {
      Goods.Clear();
    }
    protected override void OnInit()
    {
    }
  }
}
