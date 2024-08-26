using System;
using System.Collections.Generic;
using cfg;
using QFramework;
using SimpleJSON;
using UnityEngine;
namespace Battlegrounds
{
  public interface ITableLoader : IUtility
  {
    void InitializeTables(string[] tableNames, Action action = null);
    bool IsTableLoaded { get; }
    Tables Tables { get; }
  }

  public class LubanTableLoader : ITableLoader
  {
    //qframework资源加载器
    private ResLoader mResLoader = ResLoader.Allocate();
    //临时存放luban加载的资源
    private Dictionary<string, TextAsset> m_LubanTextAssets = new Dictionary<string, TextAsset>();
    //判断异步加载配置表是否完成
    private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();
    //存放配置表
    public Tables Tables { get; private set; }

    public bool IsTableLoaded { get; private set; }

    /// <summary>
    /// 创建数据表
    /// </summary>
    /// <param name="tableNames"></param>
    /// <param name="action"></param>
    public void InitializeTables(string[] tableNames, Action action = null)
    {
      m_LoadedFlag.Clear();
      m_LubanTextAssets.Clear();

      foreach (var tableName in tableNames)
      {
        AddTable(tableName);
      }

      mResLoader.LoadAsync(() =>
      {
        //修改加载状态
        IsTableLoaded = true;
        //存入配置表
        Tables = new Tables((file) =>
        {
          TextAsset textAsset = m_LubanTextAssets[file];
          var tab = JSON.Parse(textAsset.text);
          return tab;
        });
        //清理资源
        Clear();
        //执行回调
        action?.Invoke();
      });
    }

    private void AddTable(string lubanTableName)
    {
      m_LoadedFlag.Add(lubanTableName, false);

      mResLoader.Add2Load(lubanTableName, (success, res) =>
      {
        if (success)
        {
          m_LoadedFlag[lubanTableName] = true;
          m_LubanTextAssets.Add(lubanTableName, (TextAsset)res.Asset);
          LogKit.I($"Luban表资源加载完成, 表名: {lubanTableName}");
        }
        else
        {
          LogKit.E($"找不到Luban表资源, 错误信息来自资源名: {lubanTableName}, 错误信息: {res}");
        }
      });
    }

    /// <summary>
    /// 清理数据表
    /// </summary>
    public void Clear()
    {
      mResLoader.Recycle2Cache();
      mResLoader = null;
      m_LoadedFlag.Clear();
      m_LubanTextAssets.Clear();
    }
  }

}
