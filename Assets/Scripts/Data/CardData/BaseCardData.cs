using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public enum CardType
  {
    Minion = 1,
    Spell,
    Weapon
  }
  public interface IBaseCardData
  {
    static int SerializationId { get; }
    int CardId { get; }
    CardType CardType { get; }
    BindableProperty<string> Name { get; }
    BindableProperty<string> Icon { get; }
    BindableProperty<int> Star { get; }
    BindableProperty<int> Cost { get; }
    BindableProperty<string> Des { get; }
    // int AssociatedMinoinId { get; }
  }
  public class BaseCardData : IBaseCardData
  {
    // 序列化编号
    public static int SerializationId { get; private set; }
    // 编号
    public int CardId { get; private set; }
    // 卡牌类型
    public CardType CardType { get; private set; }
    // 名称
    public BindableProperty<string> Name { get; private set; } = new BindableProperty<string>();
    // 图标
    public BindableProperty<string> Icon { get; private set; } = new BindableProperty<string>();
    // 星级
    public BindableProperty<int> Star { get; private set; } = new BindableProperty<int>();
    // 描述
    public BindableProperty<string> Des { get; private set; } = new BindableProperty<string>();
    //费用
    public BindableProperty<int> Cost { get; private set; } = new BindableProperty<int>();

    //关联随从编号
    // public int AssociatedMinoinId { get; private set; }

    public BaseCardData(cfg.Card card = null)
    {
      if (card == null)
      {
        LogKit.E("Cfg not found: TbCard", card);
        return;
      }
      CardId = card.Id;
      CardType = (CardType)card.CardType;
      Name.Value = card.Name;
      Icon.Value = card.Icon;
      Star.Value = card.Star;
      Des.Value = card.Des;
      Cost.Value = card.Cost;

    }
  }
}