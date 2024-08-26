using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections.Specialized;

namespace Battlegrounds
{
  public class PlayerInfoPanelData : UIPanelData
  {
    public IPlayerInfo PlayerInfo;
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
      playerInfoData = mData.PlayerInfo;

      // 监听数据变化 更新UI
      playerInfoData.CurrentHp.RegisterWithInitValue(UpdateCurrentHpText).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.MaxHp.RegisterWithInitValue(UpdateMaxHpText).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.CurrentMp.RegisterWithInitValue(UpdateCurrentMp).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.MaxMp.RegisterWithInitValue(UpdateMaxMp).UnRegisterWhenGameObjectDestroyed(this);
      playerInfoData.HandCards.CollectionChanged += OnHandCardsChanged;
      //监听拖拽事件
      TypeEventSystem.Global.Register<EndDragMinionEvent>(OnEndDragMinion).UnRegisterWhenGameObjectDestroyed(this);
      turnBtn.onClick.AddListener(TurnBtnClick);

      // 生成卡牌id
      // ITableLoader tableLoader = this.GetUtility<ITableLoader>();
      // cfg.Card card = tableLoader.Tables.TbCard.Get(10002);

      // 实例化卡牌预制体
      // var go = MinionSlot.InitMinionUI(new MinionData(new MinionCardData(card)));
      // go.transform.SetParent(MinionSlot.transform, false);
    }

    //手牌变更事件
    private void OnHandCardsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
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

    // 结束拖拽随从事件
    private void OnEndDragMinion(EndDragMinionEvent @event)
    {
      if (@event.MinionData == null)
      {
        "拖拽的随从数据为空".LogError();
        return;
      }
      RectTransform handArea = HandCardSlot.transform as RectTransform;
      // 判断是否在可放置区域
      bool isSuccess = RectTransformUtility.RectangleContainsScreenPoint(
          handArea,
          @event.EventData.position,
          @event.EventData.pressEventCamera
          );

      if (isSuccess)
      {
        // 获取卡牌配置表数据
        cfg.Card card = this.GetUtility<ITableLoader>().Tables.TbCard.Get(@event.MinionData.Id);
        MinionCardData minionCardData = new MinionCardData(card);
        //将随从加入手牌
        this.GetModel<IPlayerInfoModel>().AddHandCard(minionCardData, playerInfoData.Id);

        //销毁拖拽随从
        Destroy(@event.MinionUIItem.gameObject);
      }
      else
      {
        // 卡牌不在可放置区域, 回到原来的位置
        @event.MinionUIItem.gameObject.Parent(@event.ParentTf);
      };
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

    //移除卡牌显示效果
    // private void CardRemoveDisplay(Card card)
    // {
    //   // 移除卡牌
    //   cardsCache.Remove(card);
    //   _currentCardCount--;

    //   // 更新卡牌位置
    //   UpdateCardTransformDisplay();
    // }

    public void showMana()
    {
      //显示法力值显示
      manaTxt.gameObject.SetActive(true);
    }

    // //更新随从显示
    // public void UpdateMinionDisplay(MinionData minionData)
    // {
    //   Minion minion = Instantiate(minionPrefabs);
    //   minion.Init(minionData);
    //   minion.transform.SetParent(minionMgrTf);
    // }

    //回合结束按钮点击事件
    private void TurnBtnClick()
    {
      //隐藏法力值显示
      ManaSlot.gameObject.SetActive(false);
      manaTxt.gameObject.SetActive(false);
    }

    private new void OnDestroy()
    {
      playerInfoData.HandCards.CollectionChanged -= OnHandCardsChanged;
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }

}
