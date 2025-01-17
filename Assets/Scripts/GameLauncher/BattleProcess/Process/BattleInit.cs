using MoonSharp.VsCodeDebugger.SDK;
using QFramework;

namespace Battlegrounds
{
  public class BattleInit : AbstractState<BattleProcess.States, BattleProcess>, IController
  {
    private FSM<BattleProcess.States> mFsm;
    public BattleInit(FSM<BattleProcess.States> fsm, BattleProcess target) : base(fsm, target)
    {
      mFsm = fsm;
    }

    protected override void OnEnter()
    {
      base.OnEnter();

      "战斗初始化".LogInfo();

      //初始化战斗数据
      this.GetModel<IBattleModel>().PlayerId = 30001;
      this.GetModel<IBattleModel>().EnemyId = 30002;
      //创建玩家信息
      this.GetModel<IPlayerInfoModel>()
          .CreatePlayerInfo(this.GetModel<IBattleModel>().PlayerId);
      this.GetModel<IPlayerInfoModel>()
          .CreatePlayerInfo(this.GetModel<IBattleModel>().EnemyId);

      //初始化牌库,塞入1星牌,洗牌
      this.GetModel<IDeckModel>()
          .InitializeDeck(1)
          .ShuffleDeck();

      // 初始化商店数据
      this.GetModel<IShopModel>()
          .InitializeShop(1);

      //打开ui面板
      UIKit.OpenPanel<AnimeImageBackGround>();
      UIKit.OpenPanel<DragPanel>(UILevel.PopUI);
      UIKit.OpenPanel<PlayerInfoPanel>(new PlayerInfoPanelData());

      //切换到招募阶段
      mFsm.ChangeState(BattleProcess.States.RecruitMinion);
    }


    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}
