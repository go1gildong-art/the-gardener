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
public class BombBlossom() 
    : NutrientCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies, 1), IOnDepleted
{

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    new DamageVar(10m, DamageProps.card),
    new IntVar("DamageOnDepleted", 5m),
        new IntVar("Nutrient", Nutrient),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        }

    protected override void OnUpgrade()
    {
        IncreaseNutrient(7);
        DynamicVars["DamageOnDepleted"].UpgradeValueBy(5);
    }

    public async Task OnDepleted(PlayerChoiceContext choiceContext)
    {
        var dmg = new DamageVar(DynamicVars["DamageOnDepleted"].BaseValue, DamageProps.card);
        await DamageCmd.Attack(dmg.BaseValue)
            .FromCard(this, null)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}
