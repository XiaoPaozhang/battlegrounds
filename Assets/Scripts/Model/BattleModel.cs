using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public interface IBattleModel : IModel
  {
    FSM<BattleProcess.States> Fsm { get; set; }
    int TurnCount { get; set; }
    int CalculateShopDrawMinionCount();
  }
  public class BattleModel : AbstractModel, IBattleModel
  {
    public FSM<BattleProcess.States> Fsm { get; set; }
    public int TurnCount { get; set; }
    protected override void OnInit()
    {
      Fsm = new FSM<BattleProcess.States>();
    }

    /// <summary>
    /// 计算商店抽取的随从数量
    /// </summary>
    /// <param name="turnCount"></param>
    public int CalculateShopDrawMinionCount()
    {
      // 计算抽取数量,抽取数量为初始回合为3, 每个第四回合+1
      return 3 + TurnCount / 4;
    }
  }
}
