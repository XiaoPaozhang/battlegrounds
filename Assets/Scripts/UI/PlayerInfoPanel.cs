using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using static Battlegrounds.BattleProcess;

namespace Battlegrounds
{
  public class PlayerInfoPanelData : UIPanelData
  {
  }
  public partial class PlayerInfoPanel : UIPanel, IController
  {
    // private int _currentCardCount;//当前卡牌数量
    public float cardWidth;//卡牌宽度
    private IPlayerInfo playerInfoData;//玩家数据
    [SerializeField, Header("当前手持卡牌数量")] private float currentCardCount;
    protected override void OnInit(IUIData uiData = null)
    {
      mData = uiData as PlayerInfoPanelData ?? new PlayerInfoPanelData();
      playerInfoData = this.GetModel<IPlayerInfoModel>().PlayerInfos[this.GetModel<IBattleModel>().PlayerId];

      // 监听数据变化 更新UI
      playerInfoData.CurrentHp.RegisterWithInitValue(UpdateCurrentHpText).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.MaxHp.RegisterWithInitValue(UpdateMaxHpText).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.CurrentMp.RegisterWithInitValue(UpdateCurrentMp).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.MaxMp.RegisterWithInitValue(UpdateMaxMp).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.HandCards.CollectionChanged += OnHandCardsChanged;

      //监听ui交互
      turnBtn.onClick.AddListener(TurnBtnClick);
    }

    //手牌变更事件
    private void OnHandCardsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      "玩家卡牌数据变更".LogInfo();
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          foreach (IBaseCardData item in e.NewItems)
          {
            LogKit.I($"Item added: {item}");
            HandCardSlot.CardGettingDisplay(item);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (var item in e.OldItems)
            LogKit.I($"Item removed: {item}");
          break;
        case NotifyCollectionChangedAction.Replace:
          LogKit.I($"Item replaced.");
          break;
        case NotifyCollectionChangedAction.Move:
          LogKit.I($"Item moved.");
          break;
        case NotifyCollectionChangedAction.Reset:
          LogKit.I("The collection was reset.");
          break;
      }
    }
    //放置随从
    public void PlaceMinion(List<IMinionData> minionCardData)
    {
      ObservableCollection<IMinionData> minionDatas = new ObservableCollection<IMinionData>();
      for (int i = 0; i < minionCardData.Count; i++)
      {
        minionDatas.Add(minionCardData[i]);
      }
      MinionSlot.UpdateMinionSlot(minionDatas);
    }
    protected override void OnOpen(IUIData uiData = null)
    {
    }

    protected override void OnShow()
    {
    }

    protected override void OnHide()
    {
    }

    protected override void OnClose()
    {
      playerInfoData.HandCards.CollectionChanged -= OnHandCardsChanged;
    }
    private void UpdateCurrentHpText(int value)
    {
      hp.text = value.ToString();
    }
    private void UpdateMaxHpText(int value)
    {
      //Todo
    }

    private void UpdateCurrentMp(int currentMp)
    {
      manaTxt.text = $"{currentMp}/{playerInfoData.MaxMp.Value}";
      ManaSlot.UpdateMpDisplay(currentMp);
    }

    private void UpdateMaxMp(int maxMp)
    {
      manaTxt.text = $"{playerInfoData.CurrentMp.Value}/{maxMp}";
      ManaSlot.UpdateMpDisplay(playerInfoData.CurrentMp.Value);
    }

    public void showMana()
    {
      //显示法力值显示
      manaTxt.gameObject.SetActive(true);
    }

    //回合结束按钮点击事件
    private void TurnBtnClick()
    {
      var fsm = this.GetModel<IBattleModel>().Fsm;
      fsm.ChangeState(States.Fighting);
    }


    /// <summary>
    /// 法力值显示
    /// </summary>
    /// <param name="isDisable">是否显示</param>
    public void SetManaDisplayDisable(bool isDisable)
    {
      ManaSlot.gameObject.SetActive(isDisable);
      manaTxt.gameObject.SetActive(isDisable);
      turnBtn.gameObject.SetActive(isDisable);
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }

}
