namespace Gardener.GardenerCode.Systems;

public interface IOnConsumed
{
    Task OnConsumed();
}

public interface IOnFed
{
    Task OnFed();
}

public interface IOnDepleted
{
    Task OnDepleted();
}