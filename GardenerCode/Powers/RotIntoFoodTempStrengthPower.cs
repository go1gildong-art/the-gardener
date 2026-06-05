namespace Gardener.GardenerCode.Powers;

using BaseLib.Extensions;
using Gardener.GardenerCode.Extensions;
using MegaCrit.Sts2.Core.Models;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;

public class RotIntoFoodTempStrengthPower : TemporaryStrengthPower, ICustomModel
{
    public override AbstractModel OriginModel => ModelDb.Card<RotIntoFood>();

    // public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    // public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}