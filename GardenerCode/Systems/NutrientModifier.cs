using System;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Saves.Runs;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace Gardener.GardenerCode.Systems;


public class NutrientModifier(int baseNutrient) : CardModifier
{
    [SavedProperty]
    public int Nutrient = baseNutrient;

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await GardenerCmd.ConsumeNutrientNew(choiceContext, cardPlay.Card);
    }

    public void Decrease(int amount) { Nutrient -= amount; }
    public void Increase(int amount) { Nutrient += amount; }


    public static NutrientModifier? GetFrom(CardModel? card)
    {
        if (card == null)
        {
            GD.Print($"[DEBOOG] Card {card.Id} does not have NutrientModifier");
            return null;
        }

        var matches = Modifiers(card)
    .Where(m => m is NutrientModifier)
    .Take(2)
    .ToList();

        if (matches.Count == 0)
        {
            GD.Print($"[DEBOOG] Card {card.Id} does not have NutrientModifier.");
            return null;
        }

        if (matches.Count > 1)
        {
            GD.Print($"[DEBOOG] [WARNING] Multiple NutrientModifiers found for card {card.Id}");
        }

        return (NutrientModifier)matches[0];
    }

    public static void AddFor(CardModel? card, int amount)
    {
        if (card == null)
        {
            GD.Print($"[DEBOOG] Cannot attach NutrientModifier to null card {card.Id}");
            return;
        }

        if (GetFrom(card) != null)
        {
            GD.Print($"[DEBOOG] Card {card.Id} already has nutrient modifier. cancelling");
            return;
        }

        CardModifier.AddModifier(card, new NutrientModifier(amount));
    }
}