using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.CardSelection;
namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(GardenerCardPool))]
public class Fertilizer() : NutrientCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self, 8)
{

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new IntVar("NutrientFeed", 3),
        new CardsVar(1),
        new EnergyVar(1)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.BaseValue, base.Owner.Creature, this);

        var list = this.IsUpgraded
        ? PileType.Hand.GetPile(base.Owner).Cards
        : (
            await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 0, (int)base.DynamicVars.Cards.BaseValue),
                context: choiceContext,
                player: base.Owner,
                filter: card => card.DynamicVars.ContainsKey("Nutrient"),
                source: this
        )).ToList();

        int nut = (int)base.DynamicVars["NutrientFeed"].BaseValue;
        foreach (CardModel item in list)
        {
            await GardenerCmd.FeedNutrient(choiceContext, item, nut);
        }
    }

    protected override void OnUpgrade()
    {
        IncreaseNutrient(3);
    }
}
