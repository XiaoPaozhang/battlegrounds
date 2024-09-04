/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using System.Linq;
using DG.Tweening;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlegrounds
{
  public partial class CardUIItem : UIElement
  , IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IController
  {
    private RectTransform rectTransform;
    private int siblingIndex;
    private IBaseCardData baseCardData;
    private Transform parentTf;
    private bool IsRecruitMinionProcess;//是否是招募随从流程

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 注册更新随从卡牌的显示信息
    /// </summary>
    /// <param name = "minionCardData" > 随从数据 </ param >
    public void InitCardDisplay(IBaseCardData baseCardData)
    {
      this.baseCardData = baseCardData;
      if (baseCardData is IMinionCardData minionCardData)
      {
        minionCardData.Star.RegisterWithInitValue(OnStarChanged);
        minionCardData.Name.RegisterWithInitValue(OnNameChanged);
        minionCardData.Des.RegisterWithInitValue(OnDesChanged);
        minionCardData.Atk.RegisterWithInitValue(OnAtkChanged);
        minionCardData.CurrentHp.RegisterWithInitValue(OnHpChanged);
      }
    }

    private void OnStarChanged(int star)
    {
      starTxt.text = star.ToString();
    }

    private void OnNameChanged(string name)
    {
      nameTxt.text = name;
    }

    private void OnDesChanged(string des)
    {
      desTxt.text = des;
    }

    private void OnAtkChanged(int atk)
    {
      attackTxt.text = atk.ToString();
    }

    private void OnHpChanged(int hp)
    {
      hpTxt.text = hp.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      IsRecruitMinionProcess = this.GetModel<IBattleModel>().Fsm.CurrentStateId == BattleProcess.States.RecruitMinion;
      // 如果已经有动画正在运行，则先清除
      // if (tweenSequence != null && tweenSequence.IsPlaying())
      // {
      //   tweenSequence.Kill();
      // }

      //记录当前的渲染顺序
      siblingIndex = transform.GetSiblingIndex();
      // 设置渲染模式为置顶
      rectTransform.SetAsLastSibling();
      // 创建一个新的动画序列
      Sequence tweenSequence = DOTween.Sequence();
      // 放大动画
      tweenSequence.Append(rectTransform.DOScale(1.5f, 0.15f));

      if (!IsRecruitMinionProcess) return;
      borderLine.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      // 如果已经有动画正在运行，则先清除
      // if (tweenSequence != null && tweenSequence.IsPlaying())
      // {
      //   tweenSequence.Kill();
      // }

      // 恢复正常的渲染顺序
      rectTransform.SetSiblingIndex(siblingIndex);
      // 创建一个新的动画序列
      Sequence tweenSequence = DOTween.Sequence();
      // 恢复原始大小
      tweenSequence.Append(rectTransform.DOScale(1f, 0.25f));

      if (!IsRecruitMinionProcess) return;
      borderLine.color = Color.white;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      if (!IsRecruitMinionProcess) return;

      parentTf = transform.parent;

      if (baseCardData is IMinionCardData)
      {
        Transform dragPanelTf = UIKit.GetPanel<DragPanel>().transform;
        transform.SetParent(dragPanelTf);
        transform.position = Input.mousePosition;

      }
    }
    public void OnDrag(PointerEventData eventData)
    {
      if (!IsRecruitMinionProcess) return;

      //拖拽
      Vector3 mousePosition = eventData.position;
      mousePosition = this.GetUtility<IScreenUtils>().ClampToScreenBounds(mousePosition);
      transform.position = mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
      if (!IsRecruitMinionProcess) return;

      CheckForDropTarget(eventData, baseCardData);
    }

    private void CheckForDropTarget(PointerEventData @event, IBaseCardData baseCardData)
    {
      PlayerInfoPanel playerInfoPanel = UIKit.GetPanel<PlayerInfoPanel>();

      RectTransform handArea = playerInfoPanel.HandCardSlot.transform as RectTransform;
      RectTransform minionArea = playerInfoPanel.MinionSlot.transform as RectTransform;
      // 判断是否在手牌区域
      bool AtHandArea = RectTransformUtility.RectangleContainsScreenPoint(
          handArea,
          @event.position,
          @event.pressEventCamera
          );
      // 判断是否在随从可放置区域
      bool AtMinionArea = RectTransformUtility.RectangleContainsScreenPoint(
          minionArea,
          @event.position,
          @event.pressEventCamera
          );
      // 如果拖拽的是随从卡,且拖拽至随从区
      if (baseCardData is IMinionCardData minionCardData && AtMinionArea)
      {
        // "放置随从".LogInfo();
        IPlayerInfoModel playerInfoModel = this.GetModel<IPlayerInfoModel>();
        int playerId = this.GetModel<IBattleModel>().PlayerId;
        MinionData minionData = new MinionData(minionCardData, IMinionData.UiType.Player);
        playerInfoModel.GetPlayerInfo(playerId).Minions.Add(minionData);
        playerInfoPanel.PlaceMinion(playerInfoModel.PlayerInfos[playerId].Minions.ToList());

        playerInfoPanel.HandCardSlot.DestroyCard(this);

        playerInfoPanel.HandCardSlot.DealCard(gameObject, false);
      }
      // 如果拖拽的是法术卡,且拖拽至手牌区外
      else if (baseCardData is ISpellCardData && !AtHandArea)
      {
        //拖拽到手牌区域外
        if (!AtHandArea)
        {
          "放置法术".LogInfo();
        }
      }
      else
      {
        // "卡牌归位".LogInfo();

        // 卡牌不在可放置区域, 回到原来的位置
        transform.Parent(parentTf);
        transform.SetSiblingIndex(siblingIndex);
        playerInfoPanel.HandCardSlot.DealCard(gameObject, false);
      }
    }

    protected override void OnBeforeDestroy()
    {
      // 清除任何可能仍在运行的动画
      // if (tweenSequence != null)
      // {
      //   tweenSequence.Kill();
      // }
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}