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
using MegaCrit.Sts2.Core.Rooms;

namespace Gardener.GardenerCode.Powers;




  
public class PhotosynthesisPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        if (player != base.Owner.Player) return amount;
        return amount + (decimal)base.Amount;
    }

    public override async Task AfterCombatEnd(CombatRoom _)
	{
		if (!base.Owner.IsDead)
		{
			Flash();
			await CreatureCmd.GainMaxHp(base.Owner, base.Amount * 3);
            await PowerCmd.Remove(this);
		}
	}

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}