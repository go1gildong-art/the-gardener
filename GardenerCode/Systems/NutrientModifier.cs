using System;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Saves.Runs;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using BaseLib.Extensions;
using BaseLib.Config;
using Gardener.GardenerCode.Cards;

namespace Gardener.GardenerCode.Systems;


public class NutrientModifier() : CardModifier
{
    public int Nutrient;

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
            GD.Print($"[DEBOOG] Invalid card {card.Id} ");
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

    public static void AddTo(CardModel? card, int amount)
    {
        if (card == null)
        {
            GD.Print($"[DEBOOG] Cannot attach NutrientModifier to null card {card?.Id}");
            return;
        }

        if (GetFrom(card) != null)
        {
            GD.Print($"[DEBOOG] Card {card.Id} already has nutrient modifier. cancelling");
            return;
        }

        var modif = ModelDb.CardModifier<NutrientModifier>().MutableClone();
        GD.Print($"[DEBOOG] {System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(modif)}");
        if (modif is NutrientModifier nutrientModif)
        {
            nutrientModif.Increase(amount);
            CardModifier.AddModifier(card, nutrientModif);
        }

    }

    public override void ModifyDescription(Creature? target, ref string description)
    {
        description += $"\n[gold]양분[/gold] {Nutrient}.";
        base.ModifyDescription(target, ref description);
    }

    public override void StoreSaveData(ModifierSave save)
    {
        save.IntProperties.Add("Nutrient", Nutrient);
        base.StoreSaveData(save);
    }

    public override void LoadSaveData(ModifierSave save)
    {
        Nutrient = save.IntProperties.TryGetValue("Nutrient", out var nut) ? nut : 1;
        base.LoadSaveData(save);
    }


}