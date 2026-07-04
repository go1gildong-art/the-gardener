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
using MegaCrit.Sts2.Core.Models.Powers;

  
[Pool(typeof(GardenerCardPool))]
public class WinterBreeze() : NutrientCard(0, CardType.Attack, CardRarity.Common, TargetType.AllEnemies, 10), IOnDepleted
{
    public override TargetType TargetType => IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(2m, DamageProps.card),
        new PowerVar<WeakPower>(2),
        new IntVar("WeakPowerOnDepleted", 1),
        new IntVar("Nutrient", Nutrient),
    };

    public async Task OnDepleted(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<WeakPower>(
            choiceContext,
            base.Owner.Creature,
            DynamicVars["WeakPowerOnDepleted"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .TargetingAllOpponents(base.CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await PowerCmd.Apply<WeakPower>(
                choiceContext,
                base.CombatState.HittableEnemies,
                DynamicVars["WeakPower"].BaseValue,
                base.Owner.Creature, this);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await PowerCmd.Apply<WeakPower>(
                choiceContext,
                cardPlay.Target,
                DynamicVars["WeakPower"].BaseValue,
                base.Owner.Creature, this);
        }

        }

    protected override void OnUpgrade()
    {
    }
}
