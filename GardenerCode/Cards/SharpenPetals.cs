using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Godot;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using BaseLib.Extensions;

  
[Pool(typeof(GardenerCardPool))]
public class SharpenPetals : GardenerCard
{
    protected override bool HasEnergyCostX => true;
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;
    public SharpenPetals() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 3);
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("Turns").WithMultiplier((card, _) => NutrientModifier.GetFrom(card)?.Nutrient ?? 0),
        new IntVar("ThornsMultiplier", 2)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int cost = ResolveEnergyXValue();

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        var power = await PowerCmd.Apply<SharpenPetalsPower>(
            choiceContext,
            base.Owner.Creature,
            Nutrient,
            base.Owner.Creature, this);

        power?.Init(cost * DynamicVars["ThornsMultiplier"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        NutrientModifier.GetFrom(this)?.Increase(2);
    }
}