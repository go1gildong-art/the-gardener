
using MegaCrit.Sts2.Core.Entities.Powers;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Commands;

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;


using Gardener.GardenerCode.Extensions;
using Gardener.GardenerCode.Cards;

using MegaCrit.Sts2.Core.GameActions.Multiplayer;


namespace Gardener.GardenerCode.Powers;

public class TemporaryThornsPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side == base.Owner.Side)
        {
            Flash();
			await PowerCmd.Remove(this);
			await PowerCmd.Apply<ThornsPower>(null, base.Owner, base.Amount * -1, base.Owner, null);
        }
    }
}