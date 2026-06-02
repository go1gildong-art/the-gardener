using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class WaterAndFood() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Skill,
  CardRarity.Uncommon,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new CardsVar(1),
        new IntVar("NutrientFeed", 2)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        List<CardModel> list = (
            await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 0, (int) base.DynamicVars.Cards.BaseValue),
                context: choiceContext,
                player: base.Owner,
                filter: card => true,
                source: this
        )).ToList();

        if (list.Count == 0)
        {
            await GardenerCmd.ConsumeNutrient(this);
            return;
        }

        foreach (CardModel item in list)
        {
            CardCmd.ApplyKeyword(item, CardKeyword.Retain);
            item.EnergyCost.AddThisCombat(-1);
            await GardenerCmd.FeedNutrient(item, (int) base.DynamicVars["NutrientFeed"].BaseValue);
        }

        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        CardCmd.ApplyKeyword(this, CardKeyword.Retain);
    }
}
