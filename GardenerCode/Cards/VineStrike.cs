using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.CardPools;
[Pool(typeof(SilentCardPool))]
public class VineStrike() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Attack,
  CardRarity.Basic,
  TargetType.Self)
{
    protected override IEnumerable<MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
      ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
      await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
      .WithHitFx("vfx/vfx_attack_slash")
      .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
      base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}