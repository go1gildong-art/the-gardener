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

[Pool(typeof(GardenerCardPool))]
public class VineStrike : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public VineStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        NutrientModifier.AddTo(this, 4);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    new DamageVar(6m, DamageProps.card),
        new IntVar("Nutrient", Nutrient),
        new RepeatVar(2)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {   

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
            .WithHitCount((int)DynamicVars.Repeat.BaseValue)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        }

    protected override void OnUpgrade()
    {
        NutrientModifier.GetFrom(this)?.Increase(7);
    }
}

