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

  public class PlayerInfoData : IPlayerInfo
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

    public PlayerInfoData(cfg.PlayerInfo playerInfo)
    {
      CurrentHp.RegisterWithInitValue(OnCurrentHpChanged);
      MaxHp.RegisterWithInitValue(OnMaxHpChanged);
      CurrentMp.RegisterWithInitValue(OnCurrentMpChanged);
      MaxMp.RegisterWithInitValue(OnMaxMpChanged);

      Id = playerInfo.Id;
      MaxHp.Value = playerInfo.Hp;
      MaxMp.Value = playerInfo.Mp;
      CurrentHp.Value = playerInfo.Hp;
      CurrentMp.Value = playerInfo.Mp;
      Star.Value = playerInfo.Star;
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
  }

  public interface IPlayerInfoModel : IModel
  {
    Dictionary<int, IPlayerInfo> PlayerInfos { get; }
    IPlayerInfo CreatePlayerInfo(int playerId);
    IPlayerInfo GetPlayerInfo(int playerId);
  }

  public class PlayerInfoModel : AbstractModel, IPlayerInfoModel
  {
    public Dictionary<int, IPlayerInfo> PlayerInfos { get; set; } = new Dictionary<int, IPlayerInfo>();
    public IPlayerInfo CreatePlayerInfo(int playerId)
    {
      cfg.PlayerInfo playerInfo = this.GetUtility<ITableLoader>().Tables.TbPlayerInfo.Get(playerId);
      var playerInfoData = new PlayerInfoData(playerInfo);
      PlayerInfos.Add(playerInfoData.Id, playerInfoData);
      return playerInfoData;
    }

    public IPlayerInfo GetPlayerInfo(int playerId)
    {
      if (!PlayerInfos.ContainsKey(playerId))
      {
        $"不存在的玩家id: {playerId}".LogWarning();
        return null;
      }
      return PlayerInfos[playerId];
    }

    protected override void OnInit()
    {

    }
  }
}
