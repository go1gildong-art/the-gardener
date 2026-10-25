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
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using System.Security.Cryptography.X509Certificates;
using MegaCrit.Sts2.Core.Models;

[Pool(typeof(GardenerCardPool))]
public class FlowerPot() : NutrientCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self, 4)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new PowerVar<FlowerPotPower>(1)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal amount = base.DynamicVars["FlowerPotPower"].BaseValue;

        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<FlowerPotPower>(
            choiceContext,
            base.Owner.Creature,
            amount,
            base.Owner.Creature, this);

        AbstractRoom? currentRoom = base.CombatState?.RunState.CurrentRoom;
        if (currentRoom is CombatRoom combatRoom)
        {
            for (int i = 0; i < amount; i++)
            {
                combatRoom.AddExtraReward(
                    base.Owner,
                    BloomReward.Generate(RarityOptions.Bud, base.Owner)
                );
            }

        }
        else
        {
            GD.Print($"[DEBOOG] FlowerPot added Bud reward but current room is not a CombatRoom. Room: {currentRoom?.GetType().Name}");
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
