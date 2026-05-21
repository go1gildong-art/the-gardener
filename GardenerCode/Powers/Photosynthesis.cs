using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Commands;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Combat;

using Gardener.GardenerCode.Extensions;

namespace Gardener.GardenerCode.Powers;
  

  
public class Photosynthesis : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "photosynthesispower.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "photosynthesispower.png".BigPowerImagePath();
        }
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner != null && Owner.IsPlayer)
        {
            await CreatureCmd.Heal(base.Owner, 3);
        }
    }

    public override decimal ModifyEnergyGain(Player player, decimal amount)
    {
        if (player != base.Owner.Player)
		{
			return amount;
		}
		return amount + (decimal)base.Amount;
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        await CreatureCmd.Heal(base.Owner, (decimal)base.Amount * 3);
        await base.BeforeHandDraw(player, choiceContext, combatState);
    }
}