using System.Collections;
using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
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
      IMinionCardData[] minionCardDatas = this.GetModel<IDeckModel>().DrawCards<IMinionCardData>(drawCount);
      foreach (var minionCardData in minionCardDatas)
      {
        // 添加商品到商店中,并返回商品列表
        this.GetModel<IShopModel>().Goods.Add(new MinionData(minionCardData, IMinionData.UiType.Shop));
      }

      UIKit.OpenPanel<ShopInfoPanel>(new ShopInfoPanelData());
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}
