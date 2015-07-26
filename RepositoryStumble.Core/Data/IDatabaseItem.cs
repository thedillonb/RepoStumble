namespace RepositoryStumble.Core.Data
{
    public interface IDatabaseItem<out T>
    {
        T Id { get; }
    }
}

