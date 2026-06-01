using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class RapidGrowth() : GardenerCode.Cards.GardenerCard(
  2,
  CardType.Skill,
  CardRarity.Uncommon,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", 5),
        new CardsVar(4)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        await GardenerCmd.ConsumeNutrient(this);
        UpdateCost();
    }

    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || card.IsClone) return;
        UpdateCost();
    }
    public void OnConsumed() {
        UpdateCost();
    }

    public void OnFed()
    {
        UpdateCost();
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Nutrient"].UpgradeValueBy(4);
        UpdateCost();
    }

    private void UpdateCost()
	{
        if (base.DynamicVars["Nutrient"].BaseValue <= 4)
        {
            base.EnergyCost.AddThisTurn(-1);    
        }
	}
}
