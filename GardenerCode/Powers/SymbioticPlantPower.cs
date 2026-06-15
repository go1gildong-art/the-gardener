
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Gardener.GardenerCode.Systems;
using BaseLib.Abstracts;
using Gardener.GardenerCode.Extensions;
namespace Gardener.GardenerCode.Powers;

public sealed class SymbioticPlantPower : CustomPowerModel, IShouldConsumeNutrient
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	// protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));

	public bool ShouldConsumeNutrient(Creature creature)
	{
		if (base.Owner != creature)
		{
			return true;
		}
		return false;
	}

	public async Task OnNutrientConsumeBlocked()
    {
        Flash();
        await PowerCmd.Decrement(this);
    }

	public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
	public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}