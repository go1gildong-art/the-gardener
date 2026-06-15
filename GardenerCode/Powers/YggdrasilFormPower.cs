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
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Factories;

using System.Reflection;

namespace Gardener.GardenerCode.Powers;

using MegaCrit.Sts2.Core.Entities.Cards;



public class YggdrasilFormPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != base.Owner.Player) return;

        List<CardModel> cards =
        CardFactory
        .GetDistinctForCombat
        (
            player,
            ModelDb.CardPool<GardenerCardPool>()
                .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
                .Where(card =>
                {
                    var field = card.GetType().GetField("CannotBeGeneratedFromYggdrasilForm");
                    if (field == null) return true;
                    if (field.GetValue(card) is bool b && !b) return true;
                    return false;
                }),
            base.Amount,
            player.RunState.Rng.CombatCardGeneration
        ).ToList();

        foreach (CardModel item in cards)
        {
            item.EnergyCost.SetUntilPlayed(0);
            item.SetStarCostUntilPlayed(0);
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(
                    item,
                    PileType.Draw,
                    base.Owner.Player,
                    CardPilePosition.Random
            ));
        }


    }

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}