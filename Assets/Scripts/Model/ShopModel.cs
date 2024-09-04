
using System.Collections.ObjectModel;
using cfg;
using QFramework;

namespace Battlegrounds
{
  public interface IShopModel : IModel
  {
    BindableProperty<int> Star { get; }
    int RefreshCost { get; }
    int UpgradeCost { get; }
    ShopModel InitializeShop(int star);
    ShopModel UpgradeStore();
    ShopModel RefreshStore();
    bool IsMaxStar();
    ObservableCollection<IMinionData> Goods { get; }
  }
  public class ShopModel : AbstractModel, IShopModel
  {
    private bool _isInitialized = false;
    private TbShop _tbShop;//商店数据表
    public BindableProperty<int> Star { get; private set; } = new BindableProperty<int>(); //星级
    public int RefreshCost { get; private set; } //刷新所需要的金币数量
    public int UpgradeCost { get; private set; } //升级所需要的金币数量
    public ObservableCollection<IMinionData> Goods { get; private set; } = new ObservableCollection<IMinionData>(); //商品随从列表
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
