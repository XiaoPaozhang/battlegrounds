using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  public interface IPlayerInfo
  {
    int Id { get; }
    BindableProperty<int> MaxHp { get; }
    BindableProperty<int> CurrentHp { get; }
    BindableProperty<int> MaxMp { get; }
    BindableProperty<int> CurrentMp { get; }
    BindableProperty<int> Star { get; }
    ObservableCollection<IBaseCardData> HandCards { get; }
    ObservableCollection<IMinionData> Minions { get; }
  }

  public class PlayerInfoData : IPlayerInfo, ICanGetUtility
  {
    // 编号
    public int Id { get; }
    // 最大血量
    public BindableProperty<int> MaxHp { get; } = new BindableProperty<int>();
    // 当前血量
    public BindableProperty<int> CurrentHp { get; } = new BindableProperty<int>();
    // 最大法力值
    public BindableProperty<int> MaxMp { get; } = new BindableProperty<int>();
    // 当前法力值
    public BindableProperty<int> CurrentMp { get; } = new BindableProperty<int>();
    // 星级
    public BindableProperty<int> Star { get; } = new BindableProperty<int>();
    // 拥有的手牌
    public ObservableCollection<IBaseCardData> HandCards { get; } = new ObservableCollection<IBaseCardData>();
    // 场地上的随从
    public ObservableCollection<IMinionData> Minions { get; } = new ObservableCollection<IMinionData>();

    public PlayerInfoData(int playerId = 30001)
    {
      Id = playerId;
      CurrentHp.Register(OnCurrentHpChanged);
      MaxHp.Register(OnMaxHpChanged);
      CurrentMp.Register(OnCurrentMpChanged);
      MaxMp.Register(OnMaxMpChanged);
    }

    private void OnCurrentHpChanged(int newValue)
    {
      CurrentHp.Value = Mathf.Clamp(newValue, 0, MaxHp.Value);
    }

    private void OnMaxHpChanged(int newValue)
    {
      if (CurrentHp.Value > newValue)
      {
        CurrentHp.Value = newValue;
      }
    }

    private void OnCurrentMpChanged(int newValue)
    {
      CurrentMp.Value = Mathf.Clamp(newValue, 0, MaxMp.Value);
    }

    private void OnMaxMpChanged(int newValue)
    {
      if (CurrentMp.Value > newValue)
      {
        CurrentMp.Value = newValue;
      }
    }

    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }

  public interface IPlayerInfoModel : IModel
  {
    Dictionary<int, IPlayerInfo> PlayerInfos { get; }
    IPlayerInfo CreatePlayerInfo(int playerId);
    void AddHandCard(IBaseCardData cardData, int playerId);
    void AddMinion(IMinionData minionData, int playerId);
  }

  public class PlayerInfoModel : AbstractModel, IPlayerInfoModel
  {
    public Dictionary<int, IPlayerInfo> PlayerInfos { get; set; }
    protected override void OnInit()
    {
      PlayerInfos = new Dictionary<int, IPlayerInfo>();
    }


    public IPlayerInfo CreatePlayerInfo(int playerId)
    {
      var playerInfo = this.GetUtility<ITableLoader>().Tables.TbPlayerInfo.Get(playerId);
      var playerInfoData = new PlayerInfoData(playerId);
      playerInfoData.MaxHp.Value = playerInfo.Hp;
      playerInfoData.MaxMp.Value = playerInfo.Mp;
      playerInfoData.CurrentHp.Value = playerInfo.Hp;
      playerInfoData.CurrentMp.Value = playerInfo.Mp;
      playerInfoData.Star.Value = playerInfo.Star;
      PlayerInfos.Add(playerInfoData.Id, playerInfoData);
      return playerInfoData;
    }

    public void AddHandCard(IBaseCardData cardData, int playerId)
    {
      PlayerInfos[playerId].HandCards.Add(cardData);
    }

    public void AddMinion(IMinionData minionData, int playerId)
    {
      PlayerInfos[playerId].Minions.Add(minionData);
    }
  }
}
