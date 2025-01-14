
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
public partial class Tables
{
    public TbCard TbCard {get; }
    public TbPlayerInfo TbPlayerInfo {get; }
    public TbShop TbShop {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        TbCard = new TbCard(loader("tbcard"));
        TbPlayerInfo = new TbPlayerInfo(loader("tbplayerinfo"));
        TbShop = new TbShop(loader("tbshop"));
        ResolveRef();
    }
    
    private void ResolveRef()
    {
        TbCard.ResolveRef(this);
        TbPlayerInfo.ResolveRef(this);
        TbShop.ResolveRef(this);
    }
}

}
