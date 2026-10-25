using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards.Blooms;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

  
  
[Pool(typeof(TokenCardPool))]
public class GlassBud() : NutrientCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, 6), IBloomCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(6m, DamageProps.card),
        new BlockVar(6m, ValueProp.Move),
        new IntVar("Nutrient", Nutrient)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
        .Targeting(cardPlay.Target)
        .FromCard(this, cardPlay)
        .WithHitCount((int)DynamicVars.Repeat.BaseValue)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        base.DynamicVars.Block.UpgradeValueBy(2m);
        IncreaseNutrient(3);
    }
}

