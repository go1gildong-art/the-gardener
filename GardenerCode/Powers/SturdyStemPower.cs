
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

public sealed class SturdyStemPower : PowerModel
{
	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	// protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));

	public override bool ShouldClearBlock(Creature creature)
	{
		if (base.Owner != creature)
		{
			return true;
		}
		return false;
	}

	public override Task AfterPreventingBlockClear(AbstractModel preventer, Creature creature)
	{
		if (this != preventer)
		{
			return Task.CompletedTask;
		}
		Flash();
		return Task.CompletedTask;
	}

	public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner))
		{
			await PowerCmd.Decrement(this);
		}
	}
}