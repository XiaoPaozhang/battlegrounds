using UnityEngine;
using QFramework;
using System;

namespace Battlegrounds
{
  /// <summary>
  /// 战斗流程
  /// </summary>
  [MonoSingletonPath("[GameLauncher]/[BattleProcess]")]
  public class BattleProcess : MonoSingleton<BattleProcess>
  {
    public FSM<States> FSM = new FSM<States>();
    public enum States
    {
      /// <summary>
      /// 战斗初始化
      /// </summary>
      BattleInit,
      /// <summary>
      /// 招募随从
      /// </summary>
      RecruitMinion,
      /// <summary>
      /// 战斗
      /// </summary>
      Fighting,
    }

    public void Awake()
    {

      FSM.AddState(States.BattleInit, new BattleInit(FSM, this));
      FSM.AddState(States.RecruitMinion, new RecruitMinion(FSM, this));
      FSM.AddState(States.Fighting, new Fighting(FSM, this));

      FSM.StartState(States.BattleInit);

    }

    private void Update()
    {
      FSM.Update();
    }

    private void FixedUpdate()
    {
      FSM.FixedUpdate();
    }

    private void OnGUI()
    {
      FSM.OnGUI();
      GUI.Label(new Rect(100, 100, 200, 20), "Current State: " + FSM.CurrentStateId);
    }

    protected override void OnDestroy()
    {
      FSM.Clear();
    }

    public void Destroy()
    {
      Destroy(this);
      Destroy(this.gameObject);
    }
  }
}
