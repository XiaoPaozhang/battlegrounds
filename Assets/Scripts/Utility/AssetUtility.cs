
using QFramework;

namespace Battlegrounds
{
  public interface IAssetUtility : IUtility
  {
  }
  /// <summary>
  /// 资源加载工具方法类
  /// </summary>
  public class AssetUtility : IAssetUtility
  {
    // 提取公共路径作为常量
    private const string GAME_ASSET_PATH = "Assets";

    // public static string GetLubanTableAsset(string assetName)
    // {
    //   return $"{GAME_ASSET_PATH}/LubanTables/{assetName}.json";
    // }
  }
}