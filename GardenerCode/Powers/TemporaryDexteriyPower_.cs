namespace Gardener.GardenerCode.Powers;

using MegaCrit.Sts2.Core.Models;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

public class TemporaryDexterityPower_ : TemporaryDexterityPower
{
    public override AbstractModel OriginModel => ModelDb.Card<SurpriseMutation>();
}