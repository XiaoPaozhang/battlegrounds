using UnityEngine;
using QFramework;

namespace Battlegrounds
{
  /// <summary>
  /// 战斗流程
  /// </summary>
  [MonoSingletonPath("[GameLauncher]/[BattleProcess]")]
  public class BattleProcess : MonoSingleton<BattleProcess>, IController
  {
    public FSM<States> FSM;
    [SerializeField] private States CurrentState;
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
      // 初始化 FSM
      this.GetModel<IBattleModel>().Fsm = new FSM<States>();
      FSM = this.GetModel<IBattleModel>().Fsm;
      FSM.AddState(States.BattleInit, new BattleInit(FSM, this));
      FSM.AddState(States.RecruitMinion, new RecruitMinion(FSM, this));
      FSM.AddState(States.Fighting, new Fighting(FSM, this));

      FSM.StartState(States.BattleInit);

    }

    private void Update()
    {
      FSM.Update();
      CurrentState = FSM.CurrentStateId;
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

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}
