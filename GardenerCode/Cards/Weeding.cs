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
using MegaCrit.Sts2.Core.Models;
using BaseLib.Abstracts;

[Pool(typeof(GardenerCardPool))]
public class Weeding : GardenerCard, ITranscendenceCard
{
    protected override bool HasEnergyCostX => true;
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public Weeding() : base(
  0,
  CardType.Attack,
  CardRarity.Basic,
  TargetType.AnyEnemy)
    {
        NutrientModifier.AddTo(this, 7);
    }

    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<Harvest>();
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
        new DamageVar(7m, DamageProps.card),
        new IntVar("Nutrient", Nutrient)
        };

    // public override IEnumerable<CardKeyword> CanonicalKeywords => new CardKeyword[] { CardKeyword.Exhaust };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        int cost = ResolveEnergyXValue();

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitCount(cost)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
