using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gardener.GardenerCode.Systems;

public interface IOnConsumed
{
    Task OnConsumed(PlayerChoiceContext choiceContext);
}

public interface IOnFed
{
    Task OnFed(PlayerChoiceContext choiceContext);
}

public interface IOnDepleted
{
    Task OnDepleted(PlayerChoiceContext choiceContext);
}