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
        if (card.DynamicVars["Nutrient"] == null)
        {
            GD.Print($"[DEBOOG] Card {card.Id} has no nutrient to consume.");
            return;
        }
        CardModel? deckCard = card.DeckVersion;

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is consumed from {card.DynamicVars["Nutrient"].BaseValue} by {amount} to {card.DynamicVars["Nutrient"].BaseValue - amount}.");
        card.DynamicVars["Nutrient"].UpgradeValueBy(-amount);
        deckCard?.DynamicVars["Nutrient"].UpgradeValueBy(-amount);

        if (deckCard != null && deckCard.DynamicVars["Nutrient"].IntValue <= 0)
        {
            await Deplete(card);
        }
    }

    public static async Task FeedNutrient(
        CardModel card,
        int amount = 1)
    {
        if (card.DynamicVars["Nutrient"] == null)
        {
            GD.Print($"[DEBOOG] Card {card.Id} has no nutrient to feed");
            return;
        }
        CardModel? deckCard = card.DeckVersion;

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is fed from {card.DynamicVars["Nutrient"].BaseValue} by {amount} to {card.DynamicVars["Nutrient"].BaseValue - amount}.");
        card.DynamicVars["Nutrient"].UpgradeValueBy(amount);
        deckCard?.DynamicVars["Nutrient"].UpgradeValueBy(amount);
    }

    public static async Task Deplete(
        CardModel card
        )
    {
        GD.Print($"[DEBOOG] Card {card.Id} is depleted. Removing from combat and deck.");
        await CardPileCmd.RemoveFromCombat(card);

        CardModel? deckCard = card.DeckVersion;
        if (deckCard != null) await CardPileCmd.RemoveFromDeck(deckCard);
    }
}