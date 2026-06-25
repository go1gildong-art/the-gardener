using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models;

using MegaCrit.Sts2.Core.CardSelection;

[Pool(typeof(GardenerCardPool))]
public class LastDrop : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public LastDrop() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 8);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new RepeatVar(3)
    };
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? cardModel = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 1),
            context: choiceContext,
            player: base.Owner,
            filter: c => NutrientModifier.GetFrom(c) != null,
            source: this
            )).FirstOrDefault();

        
        for (int i = 0; i < base.DynamicVars.Repeat.BaseValue; i++)
        {
            if (cardModel == null || !cardModel.IsInCombat) break;
            await CardCmd.AutoPlay(choiceContext, cardModel, null);
        }

        while
        (
            cardModel != null
            && NutrientModifier.GetFrom(cardModel)?.Nutrient > 0
        )
        {
            await GardenerCmd.ConsumeNutrient(choiceContext, cardModel);
        }

    }

    protected override void OnUpgrade()
    {
        CardCmd.ApplyKeyword(this, CardKeyword.Retain);
    }
}
