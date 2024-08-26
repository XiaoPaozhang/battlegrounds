using QAssetBundle;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public class GameMainProcess : MonoSingleton<GameMainProcess>, IController
  {
    private ITableLoader mTableLoader;
    public static readonly string[] LubanTableNames = new string[]
    {
      Lubantables.CARD_TBCARD,
      Lubantables.PLAYER_INFO_TBPLAYERINFO,
      Lubantables.SHOP_TBSHOP,
    };
    [SerializeField] private States CurrentState;
    private BattleProcess battleProcess;
    public FSM<States> FSM = new FSM<States>();
    public enum States
    {
      /// <summary>
      /// 预加载
      /// </summary>
      Preload,
      /// <summary>
      /// 游戏开始
      /// </summary>
      GameStart,
      /// <summary>
      /// 游戏战斗
      /// </summary>
      GameFight,
    }
    private void Awake()
    {
      ResKit.Init();
    }

    private void Start()
    {
      FSM.State(States.Preload)
      .OnEnter(() =>
      {
        mTableLoader = this.GetUtility<ITableLoader>();
        mTableLoader.InitializeTables(LubanTableNames, () =>
        {
          Debug.Log("Tables loaded");
          FSM.ChangeState(States.GameFight);
        });
      });

      FSM.State(States.GameFight)
      .OnEnter(() =>
      {
        battleProcess = BattleProcess.Instance;
      })
      .OnExit(() => { battleProcess.Destroy(); })
      .OnGUI(() =>
      {
        GUILayout.Label("State GameFight");
        // if (GUILayout.Button("To State GameStart"))
        // {
        //   FSM.ChangeState(States.GameStart);
        // }
      });

      // FSM.State(States.GameStart)
      //   //  .OnCondition(() => FSM.CurrentStateId == States.B)
      //   .OnGUI(() =>
      //   {
      //     GUILayout.Label("State GameStart");
      //     if (GUILayout.Button("To State GameFight"))
      //     {
      //       FSM.ChangeState(States.GameFight);
      //     }
      //   });

      FSM.StartState(States.Preload);
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
    }

    protected override void OnDestroy()
    {
      FSM.Clear();
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }

}
