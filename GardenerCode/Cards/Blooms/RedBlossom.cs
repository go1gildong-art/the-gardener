using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards.Blooms;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;

  
[Pool(typeof(TokenCardPool))]
public class RedBlossom() : NutrientCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, 6), IBloomCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(10m, DamageProps.card),
        new PowerVar<VulnerablePower>(1),
        new IntVar("Nutrient", Nutrient),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
        .Targeting(cardPlay.Target)
        .FromCard(this, cardPlay)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);

        await PowerCmd.Apply<VulnerablePower>(
                choiceContext,
                cardPlay.Target,
                DynamicVars["VulnerablePower"].BaseValue,
                base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        base.DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
        IncreaseNutrient(3);
    }
}
