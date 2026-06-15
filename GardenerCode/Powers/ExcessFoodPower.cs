
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

    // could not find "AfterCardPlay()" or similar method. 
    // using some hacks here
    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        PileType pileType,
        CardPilePosition position)
    {
        return (pileType, position);
    }

    public override async Task AfterModifyingCardPlayResultPileOrPosition(
        CardModel card,
        PileType pileType,
        CardPilePosition position)
    {
        if (ConsumedThisTurn) return;

        var field = card.GetType().GetField(
                    "HasEnergyCostX",
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
        var isXCost = field?.GetValue(card) is bool b && b;

        if (isXCost) return;


    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == base.Owner.Player && cardPlay.Card.EnergyCost.CostsX)
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