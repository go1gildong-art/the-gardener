using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.CardSelection;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models;

[Pool(typeof(GardenerCardPool))]
public class Pruning() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Attack,
  CardRarity.Common,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(8m, DamageProps.card),
        new IntVar("NutrientFeed", 1),
        new CardsVar(1)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var list = this.IsUpgraded 
        ? PileType.Hand.GetPile(base.Owner).Cards
        : (
            await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 0, (int) base.DynamicVars.Cards.BaseValue),
                context: choiceContext,
                player: base.Owner,
                filter: card => card.DynamicVars.ContainsKey("Nutrient"),
                source: this
        )).ToList();

        foreach (CardModel item in list)
        {
            await GardenerCmd.FeedNutrient(item, (int) base.DynamicVars["NutrientFeed"].BaseValue);
        }

        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
