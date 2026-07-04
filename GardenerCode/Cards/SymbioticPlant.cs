using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Extensions;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using Gardener.GardenerCode.Powers;

[Pool(typeof(GardenerCardPool))]
public class SymbioticPlant() : NutrientCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, 4)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(7, DamageProps.card),
        new PowerVar<SymbioticPlantPower>(2),
        new IntVar("Nutrient", Nutrient)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

        await PowerCmd.Apply<SymbioticPlantPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SymbioticPlantPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        IncreaseNutrient(3);
    }
}
