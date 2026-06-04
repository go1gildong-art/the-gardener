using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
[Pool(typeof(GardenerCardPool))]
public class SurpriseMutation() : GardenerCode.Cards.GardenerCard(
  0,
  CardType.Skill,
  CardRarity.Common,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new PowerVar<TemporaryStrengthPower_>(1m),
        new PowerVar<TemporaryDexterityPower_>(1m),
        new PowerVar<TemporaryThornsPower>(2m),
    };

    public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[]
    {
        CardKeyword.Exhaust
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        foreach (var dynamicVar in this.DynamicVars)
        {
            GD.Print($"[DEBOOG] {dynamicVar}");
        }
        await PowerCmd.Apply<TemporaryStrengthPower_>(choiceContext, base.Owner.Creature, base.DynamicVars["TemporaryStrengthPower_"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<TemporaryDexterityPower_>(choiceContext, base.Owner.Creature, base.DynamicVars["TemporaryDexterityPower_"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<TemporaryThornsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["TemporaryThornsPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
