using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.CardPools;
  
[Pool(typeof(GardenerCardPool))]
public class Photosynthesis() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Attack,
  CardRarity.Basic,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] 
{ new DamageVar(4m, DamageProps.card) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
      ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
      await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
      .WithHitFx("vfx/vfx_attack_slash")
      .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
      DynamicVars.Damage.UpgradeValueBy(4m);
    }
}