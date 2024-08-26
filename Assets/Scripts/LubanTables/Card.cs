
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class Card : Luban.BeanBase
{
    public Card(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["name"].IsString) { throw new SerializationException(); }  Name = _buf["name"]; }
        { if(!_buf["cost"].IsNumber) { throw new SerializationException(); }  Cost = _buf["cost"]; }
        { if(!_buf["atk"].IsNumber) { throw new SerializationException(); }  Atk = _buf["atk"]; }
        { if(!_buf["hp"].IsNumber) { throw new SerializationException(); }  Hp = _buf["hp"]; }
        { if(!_buf["star"].IsNumber) { throw new SerializationException(); }  Star = _buf["star"]; }
        { if(!_buf["des"].IsString) { throw new SerializationException(); }  Des = _buf["des"]; }
        { if(!_buf["card_type"].IsNumber) { throw new SerializationException(); }  CardType = _buf["card_type"]; }
        { if(!_buf["asset_name"].IsString) { throw new SerializationException(); }  AssetName = _buf["asset_name"]; }
        { if(!_buf["icon"].IsString) { throw new SerializationException(); }  Icon = _buf["icon"]; }
    }

    public static Card DeserializeCard(JSONNode _buf)
    {
        return new Card(_buf);
    }

    /// <summary>
    /// 卡牌编号规则开头为1代表随从2代表法术
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 费用
    /// </summary>
    public readonly int Cost;
    /// <summary>
    /// 攻击力
    /// </summary>
    public readonly int Atk;
    /// <summary>
    /// 血量
    /// </summary>
    public readonly int Hp;
    /// <summary>
    /// 星级
    /// </summary>
    public readonly int Star;
    /// <summary>
    /// 描述
    /// </summary>
    public readonly string Des;
    /// <summary>
    /// 卡牌类型<br/>前缀为<br/>1代表随从卡<br/>2代表法术卡
    /// </summary>
    public readonly int CardType;
    /// <summary>
    /// 资源名
    /// </summary>
    public readonly string AssetName;
    /// <summary>
    /// 图标
    /// </summary>
    public readonly string Icon;
   
    public const int __ID__ = 2092848;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
        
        
        
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "cost:" + Cost + ","
        + "atk:" + Atk + ","
        + "hp:" + Hp + ","
        + "star:" + Star + ","
        + "des:" + Des + ","
        + "cardType:" + CardType + ","
        + "assetName:" + AssetName + ","
        + "icon:" + Icon + ","
        + "}";
    }
}

}