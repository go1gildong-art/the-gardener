using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Godot;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;
using Gardener.GardenerCode.Systems;


[Pool(typeof(GardenerCardPool))]
public class Photosynthesis() : NutrientCard(2, CardType.Power, CardRarity.Ancient, TargetType.Self, 8)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
{ new PowerVar<PhotosynthesisPower>(1m),
  new EnergyVar(1),
  new MaxHpVar(3)
  };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<PhotosynthesisPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["PhotosynthesisPower"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        CardCmd.ApplyKeyword(this, CardKeyword.Innate);
    }
}