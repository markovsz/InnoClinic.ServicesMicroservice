namespace Domain.Exceptions;

public class EntityAlreadyExists : Exception
{
	public EntityAlreadyExists()
		: base("entity already exists")
	{
	}
	
	public EntityAlreadyExists(string message)
		: base(message)
	{
	}
}
