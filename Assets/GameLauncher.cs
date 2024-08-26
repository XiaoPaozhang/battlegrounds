using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Battlegrounds
{
  /// <summary>
  /// 游戏入口
  /// </summary>
  public class GameLauncher : MonoBehaviour, IController
  {
    private void Awake()
    {
      UIKit.Root.SetResolution(2560, 1440, 1);
      GameMainProcess instance = GameMainProcess.Instance;
    }
    public IArchitecture GetArchitecture()
    {
      return Battlegrounds.Interface;
    }
  }
}
