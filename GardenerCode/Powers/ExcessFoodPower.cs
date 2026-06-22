
using BaseLib.Extensions;
using BaseLib.Extensions;
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
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System.Reflection;

namespace Gardener.GardenerCode.Powers;

public class ExcessFoodPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected bool ConsumedThisTurn = false;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == base.Owner.Player
        && cardPlay.Card.EnergyCost.CostsX
        && !ConsumedThisTurn)
        {
            Flash();
            ConsumedThisTurn = true;
            var player = base.Owner.Player;
            if (player != null) await PlayerCmd.GainEnergy(base.Amount, player);
        }
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(base.Owner)) return;
        ConsumedThisTurn = false;
    }

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}