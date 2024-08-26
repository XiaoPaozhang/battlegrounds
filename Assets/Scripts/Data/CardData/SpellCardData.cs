
namespace Battlegrounds
{
  public interface ISpellCardData : IBaseCardData
  {

  }
  public class SpellCardData : BaseCardData, ISpellCardData
  {
    public SpellCardData(cfg.Card card)
    {

    }
  }
}