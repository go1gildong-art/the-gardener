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

using System.Reflection;

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

        foreach (var power in card.Owner.Creature.Powers)
        {
            if (power is IShouldConsumeNutrient shouldConsumePower)
            {
                bool shouldConsume = shouldConsumePower.ShouldConsumeNutrient(card.Owner.Creature);
                if (!shouldConsume)
                {
                    await shouldConsumePower.OnNutrientConsumeBlocked();
                    return;
                }
            }
        }


        CardModel? deckCard = card.DeckVersion;

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is consumed from {card.DynamicVars["Nutrient"].BaseValue} by {amount} to {card.DynamicVars["Nutrient"].BaseValue - amount}.");
        card.DynamicVars["Nutrient"].UpgradeValueBy(-amount);
        deckCard?.DynamicVars["Nutrient"].UpgradeValueBy(-amount);

        NutrientCombatState.Get(card.CombatState).NutrientConsumedThisCombat += 1;

        if (card is IOnConsumed consumableCard)
        {
            await consumableCard.OnConsumed();
        }

        foreach (var power in card.Owner.Creature.Powers)
        {
            if (power is IOnNutrientConsume nutrientConsumePower)
            {
                await nutrientConsumePower.OnNutrientConsume();
            }
        }

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

        if (card is IOnFed fedCard)
        {
            await fedCard.OnFed();
        }
    }

    public static async Task Deplete(
        CardModel card
        )
    {
        GD.Print($"[DEBOOG] Card {card.Id} is depleted. Removing from combat and deck.");
        await CardPileCmd.RemoveFromCombat(card);

        if (card is IOnDepleted depletableCard)
        {
            await depletableCard.OnDepleted();
        }

        CardModel? deckCard = card.DeckVersion;
        if (deckCard != null) await CardPileCmd.RemoveFromDeck(deckCard);
    }
}