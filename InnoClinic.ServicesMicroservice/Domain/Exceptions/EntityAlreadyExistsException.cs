namespace Domain.Exceptions;

public class EntityAlreadyExistsException : Exception
{
	public EntityAlreadyExistsException()
		: base("entity already exists")
	{
	}
	
	public EntityAlreadyExistsException(string message)
		: base(message)
	{
	}
}
