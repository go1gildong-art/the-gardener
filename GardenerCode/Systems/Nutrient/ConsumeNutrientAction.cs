using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Gardener.GardenerCode.Cards;
using MegaCrit.Sts2.Core.Models;


using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;

namespace Gardener.GardenerCode.Systems.Nutrient;


public class ConsumeNutrientAction : GameAction
{
    private readonly Player _player;

    private readonly int _turnNumber;
    private readonly PlayerChoiceContext ChoiceContext;

    private readonly CardModel _card;

    public override ulong OwnerId => _player.NetId;

    public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

    public ConsumeNutrientAction(Player player, CardModel card, PlayerChoiceContext choiceContext)
    {
        CardPileCmd.RemoveFromCombat(card);
        CardPileCmd.RemoveFromDeck(card);
        _player = player;
        _card = card;
        ChoiceContext = choiceContext;
    }

    protected override async Task ExecuteAction()
    {
        if (_card is not GardenerCard gardenerCard || gardenerCard.Nutrient == null)
        {
            return;
        }
        gardenerCard.Id

        gardenerCard.Nutrient.Decrease();

        if (gardenerCard.Nutrient.Current <= 0)
        {
            // THIS is where depletion gets triggered
            await new NutrientDepletedAction(_player, _card).Execute();
        }

        return;
    }

    public override INetAction ToNetAction()
    {
        return new NetNutrientDepletedAction
        {
            deplectedCard = _deplectedCard,
            turnNumber = _turnNumber
        };
    }

    public override string ToString()
    {
        return $"{"NutrientDepletedAction"} for player {_player.NetId} turn {_turnNumber} card {_deplectedCard.Id}";
    }
}
