using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public interface IMinionData
  {
    enum UiType
    {
      None,
      Shop,
      Player
    }
    int Id { get; set; }
    UiType BelongsTo { get; set; }
    BindableProperty<string> Name { get; }
    BindableProperty<int> MaxHp { get; set; }
    BindableProperty<int> CurrentHp { get; set; }
    BindableProperty<int> Atk { get; set; }
    BindableProperty<string> Icon { get; set; }
    BindableProperty<int> Star { get; set; }
  }
  public class MinionData : IMinionData
  {
    public int Id { get; set; }
    public IMinionData.UiType BelongsTo { get; set; }
    public BindableProperty<string> Name { get; set; } = new BindableProperty<string>();
    public BindableProperty<int> MaxHp { get; set; } = new BindableProperty<int>();
    public BindableProperty<int> CurrentHp { get; set; } = new BindableProperty<int>();
    public BindableProperty<int> Atk { get; set; } = new BindableProperty<int>();
    public BindableProperty<string> Icon { get; set; } = new BindableProperty<string>();
    public BindableProperty<int> Star { get; set; } = new BindableProperty<int>();
    public MinionData(IMinionCardData minionCardData, IMinionData.UiType belongsTo)
    {
      Id = minionCardData.CardId;
      BelongsTo = belongsTo;
      Name.Value = minionCardData.Name.Value;
      MaxHp.Value = minionCardData.MaxHp.Value;
      CurrentHp.Value = minionCardData.CurrentHp.Value;
      Atk.Value = minionCardData.Atk.Value;
      Icon.Value = minionCardData.Icon.Value;
      Star.Value = minionCardData.Star.Value;
    }
  }
  public interface IMinionCardData : IBaseCardData
  {
    BindableProperty<int> MaxHp { get; }
    BindableProperty<int> CurrentHp { get; }
    BindableProperty<int> Atk { get; }
  }
  public class MinionCardData : BaseCardData, IMinionCardData
  {
    // 最大血量
    public BindableProperty<int> MaxHp { get; private set; } = new BindableProperty<int>();
    // 当前血量
    public BindableProperty<int> CurrentHp { get; private set; } = new BindableProperty<int>();
    // 攻击力
    public BindableProperty<int> Atk { get; private set; } = new BindableProperty<int>();

    public MinionCardData(cfg.Card card) : base(card)
    {
      MaxHp.Value = card.Hp;
      CurrentHp.Value = card.Hp;
      Atk.Value = card.Atk;
    }
  }
}
