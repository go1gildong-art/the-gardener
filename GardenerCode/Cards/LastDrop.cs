using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models;

using MegaCrit.Sts2.Core.CardSelection;


[Pool(typeof(GardenerCardPool))]
public class LastDrop() : GardenerCode.Cards.GardenerCard(
  2,
  CardType.Skill,
  CardRarity.Uncommon,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", 8),
        new RepeatVar(3)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel? cardModel = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 1),
            context: choiceContext,
            player: base.Owner,
            filter: (CardModel c) => c.DynamicVars["Nutrient"] != null,
            source: this
            )
            ).FirstOrDefault();

        for (int i = 0; i < base.DynamicVars.Repeat.BaseValue; i++)
        {
            if (
                cardModel == null
                || !cardModel.IsInCombat
            ) break;

            await CardCmd.AutoPlay(choiceContext, cardModel, null);
        }

        while (cardModel?.IsInCombat ?? false)
        {
            await GardenerCmd.ConsumeNutrient(cardModel);
        }

        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Nutrient"].UpgradeValueBy(1);
    }
}
