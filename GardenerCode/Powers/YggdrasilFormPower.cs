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

namespace Gardener.GardenerCode.Powers;

using MegaCrit.Sts2.Core.Entities.Cards;



public class YggdrasilFormPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        List<CardModel> cards =
        CardFactory
        .GetDistinctForCombat
        (
            player,
            ModelDb.CardPool<GardenerCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint),
            base.Amount,
            player.RunState.Rng.CombatCardGeneration
        ).ToList();

        foreach (CardModel item in cards)
        {
            item.SetToFreeThisCombat();
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(
                    item,
                    PileType.Draw,
                    base.Owner.Player,
                    CardPilePosition.Random
            ));
        }


    }
}