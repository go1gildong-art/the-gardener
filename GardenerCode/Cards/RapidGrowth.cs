using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class RapidGrowth : GardenerCard, IOnConsumed, IOnFed
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public RapidGrowth() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 5);
    }

    private bool _isCostReduced = false;

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new IntVar("NutrientThreshold", 4),
        new CardsVar(4),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        await GardenerCmd.ConsumeNutrient(choiceContext, this);
        UpdateCost();
    }
    public async Task OnConsumed(PlayerChoiceContext choiceContext)
    {
        UpdateCost();
    }

    public async Task OnFed(PlayerChoiceContext choiceContext)
    {
        UpdateCost();
    }

    protected override void OnUpgrade()
    {
        NutrientModifier.GetFrom(this)?.Increase(4);
        UpdateCost();
    }

    private void UpdateCost()
    {
        if 
        (
            !_isCostReduced 
            && base.DynamicVars["Nutrient"].BaseValue <= base.DynamicVars["NutrientThreshold"].BaseValue
        )
        {
            base.EnergyCost.UpgradeBy(-1);
            base.DeckVersion?.EnergyCost.UpgradeBy(-1);
            _isCostReduced = true;
        }
        else if 
        (
            _isCostReduced
            && base.DynamicVars["Nutrient"].BaseValue > base.DynamicVars["NutrientThreshold"].BaseValue
        )
        {
            base.EnergyCost.UpgradeBy(1);
            base.DeckVersion?.EnergyCost.UpgradeBy(1);
            _isCostReduced = false;
        }
    }
}
