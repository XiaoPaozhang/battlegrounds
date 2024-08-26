using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public class RecruitMinion : AbstractState<BattleProcess.States, BattleProcess>, IController
  {
    private IBattleModel battleModel;
    public RecruitMinion(FSM<BattleProcess.States> fsm, BattleProcess target) : base(fsm, target)
    {
    }


    protected override void OnEnter()
    {
      base.OnEnter();
      battleModel = this.GetModel<IBattleModel>();
      "招募阶段".LogInfo();

      //回合数加一
      battleModel.TurnCount++;

      // 计算要抽取的数量
      int drawCount = battleModel.CalculateShopDrawMinionCount();
      MinionCardData[] minionCardDatas = this.GetModel<IDeckModel>().DrawCards<MinionCardData>(drawCount);

      // 添加商品到商店中,并返回商品列表
      List<IMinionData> goods = this.GetModel<IShopModel>().UpdateGoods(minionCardDatas);

      UIKit.OpenPanel<ShopInfoPanel>(new ShopInfoPanelData()
      {
        goodsMinionDatas = goods
      });
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}
