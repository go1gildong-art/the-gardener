using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
[Pool(typeof(GardenerCardPool))]
public class SurpriseMutation() : GardenerCard(
  0,
  CardType.Skill,
  CardRarity.Common,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("TemporaryStrengthPower_", 2m),
        new IntVar("TemporaryDexterityPower_", 2m),
        new PowerVar<TemporaryThornsPower>(2m),
        new PowerVar<SurpriseMutationPower>(1m)
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
    {
        CardKeyword.Exhaust
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<SurpriseMutationPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SurpriseMutationPower"].BaseValue, base.Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
