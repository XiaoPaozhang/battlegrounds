/****************************************************************************
 * 2024.8 DESKTOP-D9T87KM
 ****************************************************************************/

using System;
using DG.Tweening;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlegrounds
{
  public partial class CardUIItem : UIElement
  , IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
  {
    private RectTransform rectTransform;
    private int _index;
    private IBaseCardData baseCardData;
    private Transform parentTf;

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
      borderLine.color = Color.green;

      // 如果已经有动画正在运行，则先清除
      // if (tweenSequence != null && tweenSequence.IsPlaying())
      // {
      //   tweenSequence.Kill();
      // }

      //记录当前的渲染顺序
      _index = transform.GetSiblingIndex();
      // 设置渲染模式为置顶
      rectTransform.SetAsLastSibling();
      // 创建一个新的动画序列
      Sequence tweenSequence = DOTween.Sequence();
      // 放大动画
      tweenSequence.Append(rectTransform.DOScale(1.5f, 0.15f));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      borderLine.color = Color.white;

      // 如果已经有动画正在运行，则先清除
      // if (tweenSequence != null && tweenSequence.IsPlaying())
      // {
      //   tweenSequence.Kill();
      // }

      // 恢复正常的渲染顺序
      rectTransform.SetSiblingIndex(_index);
      // 创建一个新的动画序列
      Sequence tweenSequence = DOTween.Sequence();
      // 恢复原始大小
      tweenSequence.Append(rectTransform.DOScale(1f, 0.25f));

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
      parentTf = transform.parent;

      if (baseCardData is IMinionCardData minionCardData)
      {
        Transform dragPanelTf = UIKit.GetPanel<DragPanel>().transform;
        transform.SetParent(dragPanelTf);
        transform.position = Input.mousePosition;
      }
    }
    public void OnDrag(PointerEventData eventData)
    {
      //拖拽
      transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
      TypeEventSystem.Global.Send(new EndDragCardEvent(
        eventData,
        baseCardData,
        this,
        parentTf
        ));
    }


    protected override void OnBeforeDestroy()
    {
      // 清除任何可能仍在运行的动画
      // if (tweenSequence != null)
      // {
      //   tweenSequence.Kill();
      // }
    }
  }
}