using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public class Fighting : AbstractState<BattleProcess.States, BattleProcess>
  {
    public Fighting(FSM<BattleProcess.States> fsm, BattleProcess target) : base(fsm, target)
    {
    }
    protected override void OnEnter()
    {
      base.OnEnter();

      UIKit.OpenPanel<EnemyInfoPanel>();
    }

    protected override void OnExit()
    {
      base.OnExit();
      UIKit.ClosePanel<EnemyInfoPanel>();
    }
  }
}
