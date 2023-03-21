namespace Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
        : base("entity not found")
    {
    }

    public EntityNotFoundException(string message)
		: base(message)
	{
	}
}
