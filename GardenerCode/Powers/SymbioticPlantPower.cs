
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

public sealed class SymbioticPlantPower : PowerModel
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
}