using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(GardenerCardPool))]
public class FungiRemover : GardenerCard, IOnDepleted
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public FungiRemover() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        NutrientModifier.AddTo(this, 8);
    }

    public override TargetType TargetType => IsUpgraded ? TargetType.AllEnemies : TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, DamageProps.card),
        new PowerVar<VulnerablePower>(1),
        new IntVar("VulnerablePowerOnDepleted", 3),
        new IntVar("Nutrient", Nutrient),
    };

    public async Task OnDepleted(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<VulnerablePower>(
            choiceContext,
            base.CombatState.HittableEnemies,
            DynamicVars["VulnerablePowerOnDepleted"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this)
                .TargetingAllOpponents(base.CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await PowerCmd.Apply<VulnerablePower>(
                choiceContext,
                base.CombatState.HittableEnemies,
                DynamicVars["VulnerablePower"].BaseValue,
                base.Owner.Creature, this);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await PowerCmd.Apply<VulnerablePower>(
                choiceContext,
                cardPlay.Target,
                DynamicVars["VulnerablePower"].BaseValue,
                base.Owner.Creature, this);
        }

        }

    protected override void OnUpgrade()
    {
    }
}
