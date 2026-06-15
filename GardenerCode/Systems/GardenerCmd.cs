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
using BaseLib.Abstracts;
using Godot;

using System.Reflection;

namespace Gardener.GardenerCode.Systems;

public static class GardenerCmd
{

    public static async Task ConsumeNutrient(
        PlayerChoiceContext choiceContext,
        CardModel card,
        int amount = 1)
    {
        if (!card.DynamicVars.ContainsKey("Nutrient"))
        {
            GD.Print($"[DEBOOG] Card {card.Id} has no nutrient to consume.");
            return;
        }


        foreach (var power in card.Owner.Creature.Powers)
        {
            if (power is not IShouldConsumeNutrient shouldConsumePower) continue;
            if (shouldConsumePower.ShouldConsumeNutrient(card.Owner.Creature)) continue;

            await shouldConsumePower.OnNutrientConsumeBlocked();
            return;
        }

        CardModel? deckCard = card.DeckVersion;

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is consumed from {card.DynamicVars["Nutrient"].BaseValue} by {amount} to {card.DynamicVars["Nutrient"].BaseValue - amount}.");
        card.DynamicVars["Nutrient"].UpgradeValueBy(-amount);
        deckCard?.DynamicVars["Nutrient"].UpgradeValueBy(-amount);

        NutrientCombatState.Get(card.CombatState).NutrientConsumedThisCombat += amount;

        if (card is IOnConsumed consumableCard) await consumableCard.OnConsumed(choiceContext);

        foreach (var power in card.Owner.Creature.Powers)
        {
            GD.Print($"[DEBOOG] Checking power {power.GetType().Name} for nutrient consume trigger.");
            if (power is not IOnNutrientConsume nutrientConsumePower) continue;

            await nutrientConsumePower.OnNutrientConsume();
        }

        if (card.DynamicVars["Nutrient"].IntValue <= 0) await Deplete(choiceContext, card);
    }

    

    public static async Task ConsumeNutrientNew(
        PlayerChoiceContext choiceContext,
        CardModel card,
        int amount = 1
    )
    {
        var nutrientModif = NutrientModifier.GetFrom(card);
        if (nutrientModif == null) return;

        CardModel? deckCard = card.DeckVersion;
        NutrientModifier? deckNutrientModif = null;
        if (deckCard != null) deckNutrientModif = NutrientModifier.GetFrom(deckCard);

        foreach (var power in card.Owner.Creature.Powers)
        {
            if (power is not IShouldConsumeNutrient shouldConsumePower) continue;
            if (shouldConsumePower.ShouldConsumeNutrient(card.Owner.Creature)) continue;

            await shouldConsumePower.OnNutrientConsumeBlocked();
            return;
        }

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is consumed from {card.DynamicVars["Nutrient"].BaseValue} by {amount} to {card.DynamicVars["Nutrient"].BaseValue - amount}.");
        
        nutrientModif.Decrease(amount);

        if (deckNutrientModif == null) GD.Print($"[DEBOOG] Deck Card {card.Id} does not have NutrientModifier.");
        deckNutrientModif?.Decrease(amount);

        NutrientCombatState.Get(card.CombatState).NutrientConsumedThisCombat += amount;

        if (card is IOnConsumed consumableCard) await consumableCard.OnConsumed(choiceContext);

        foreach (var power in card.Owner.Creature.Powers)
        {
            GD.Print($"[DEBOOG] Checking power {power.GetType().Name} for nutrient consume trigger.");
            if (power is not IOnNutrientConsume nutrientConsumePower) continue;

            await nutrientConsumePower.OnNutrientConsume();
        }

        if (nutrientModif.Nutrient <= 0) await Deplete(choiceContext, card);
    }


    public static async Task FeedNutrient(
        PlayerChoiceContext choiceContext,
        CardModel card,
        int amount = 1)
    {
        if (!card.DynamicVars.ContainsKey("Nutrient"))
        {
            GD.Print($"[DEBOOG] Card {card.Id} has no nutrient to feed");
            return;
        }

        CardModel? deckCard = card.DeckVersion;

        GD.Print($"[DEBOOG] Card {card.Id} nutrient is fed from {card.DynamicVars["Nutrient"].BaseValue} by {amount} to {card.DynamicVars["Nutrient"].BaseValue - amount}.");
        card.DynamicVars["Nutrient"].UpgradeValueBy(amount);
        deckCard?.DynamicVars["Nutrient"].UpgradeValueBy(amount);

        if (card is IOnFed fedCard) await fedCard.OnFed(choiceContext);
    }

    public static async Task Deplete(
        PlayerChoiceContext choiceContext,
        CardModel card
        )
    {
        if (card is IOnDepleted depletableCard) await depletableCard.OnDepleted(choiceContext);

        GD.Print($"[DEBOOG] Card {card.Id} is depleted. Removing from combat and deck.");
        await CardPileCmd.RemoveFromCombat(card);

        CardModel? deckCard = card.DeckVersion;
        if (deckCard != null) await CardPileCmd.RemoveFromDeck(deckCard);
    }
}