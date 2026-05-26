using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models;


using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using Godot;

namespace Gardener.GardenerCode.Systems;

public static class GardenerCmd
{
    public static async Task ConsumeNutrient(
        CardModel card,
        int amount = 1)
    {
        if (card.DynamicVars["NutrientVar"] == null)
        {
            GD.Print($"[DEBOOG] Card {card.Id} has no nutrient to consume.");
            return;
        }

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is consumed from {card.DynamicVars["NutrientVar"].BaseValue} by {amount} to {card.DynamicVars["NutrientVar"].BaseValue - amount}.");
        card.DynamicVars["NutrientVar"].UpgradeValueBy(-amount);
        
        if (card.DynamicVars["NutrientVar"].BaseValue <= 0)
        {
            await Deplete(card);
        }
    }

    public static async Task Deplete(CardModel card)
    {
        GD.Print($"[DEBOOG] Card {card.Id} is depleted. Removing from combat and deck.");
        await CardPileCmd.RemoveFromCombat(card);
        await CardPileCmd.RemoveFromDeck(card);
    }
}