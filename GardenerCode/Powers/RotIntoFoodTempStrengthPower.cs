namespace Gardener.GardenerCode.Powers;

using MegaCrit.Sts2.Core.Models;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;

public class RotIntoFoodTempStrengthPower : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<RotIntoFood>();
}