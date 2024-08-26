using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  /// <summary>
  /// 项目架构, 用于管理各个模块
  /// </summary>
  public class Battlegrounds : Architecture<Battlegrounds>
  {
    protected override void Init()
    {
      this.RegisterModel<IBattleModel>(new BattleModel());
      this.RegisterModel<IDeckModel>(new DeckModel());
      this.RegisterModel<IPlayerInfoModel>(new PlayerInfoModel());
      this.RegisterModel<IShopModel>(new ShopModel());

      this.RegisterUtility<ITableLoader>(new LubanTableLoader());
    }
  }
}
