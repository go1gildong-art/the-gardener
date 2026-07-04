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
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Saves.Runs;

  
[Pool(typeof(GardenerCardPool))]
public class Harvest() : NutrientCard(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy, 3)
{
    protected override bool HasEnergyCostX => true;
    private const int _baseDamage = 8;
    private int _currentDamage = _baseDamage;
    private int _increasedDamage;

    [SavedProperty]
    public int CurrentDamage
    {
        get { return _currentDamage; }
        set
        {
            AssertMutable();
            _currentDamage = value;
            base.DynamicVars.Damage.BaseValue = _currentDamage;
        }
    }

    [SavedProperty]
    public int IncreasedDamage
    {
        get { return _increasedDamage; }
        set
        {
            AssertMutable();
            _increasedDamage = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
        new DamageVar(CurrentDamage, DamageProps.card),
        new IntVar("NutrientFeed", 1),
        new IntVar("IncreaseDamage", 2m),
        new IntVar("RepeatBonus", 0),
        new IntVar("Nutrient", Nutrient)
        };
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        int cost = ResolveEnergyXValue();
        bool shouldTriggerFatal = cardPlay.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
        int totalRepat = cost + (int)DynamicVars["RepeatBonus"].BaseValue;

        AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target)
            .WithHitCount(totalRepat)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if
        (
            shouldTriggerFatal
            && attackCommand.Results
            .SelectMany((List<DamageResult> r) => r)
            .Any(r => r.WasTargetKilled)
        )
        {
            int increaseDamage = base.DynamicVars["IncreaseDamage"].IntValue;
            BuffFromPlay(increaseDamage);
            (base.DeckVersion as Harvest)?.BuffFromPlay(increaseDamage);

            await GardenerCmd.FeedNutrient(choiceContext, this, (int)DynamicVars["NutrientFeed"].BaseValue);
            await CardCmd.Exhaust(choiceContext, this);
        }
    }

    protected override void OnUpgrade() { DynamicVars["RepeatBonus"].UpgradeValueBy(1); }
    protected override void AfterDowngraded() { UpdateDamage(); }
    private void BuffFromPlay(int extraDamage) { IncreasedDamage += extraDamage; UpdateDamage(); }
    private void UpdateDamage() { CurrentDamage = _baseDamage + IncreasedDamage; }
}
