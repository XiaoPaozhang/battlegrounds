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
  }
}
