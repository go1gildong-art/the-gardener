using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;

using BaseLib.Abstracts;
using BaseLib.Extensions;
using Gardener.GardenerCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;

using Godot;





namespace Gardener.GardenerCode.Powers;

public class NatureRetreatPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != base.Owner.Player) return;

        await PlayerCmd.GainEnergy(base.Amount, player);
        await CardPileCmd.Draw(choiceContext, base.Amount, base.Owner.Player);
        await PowerCmd.Remove(this);
    }


    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}
