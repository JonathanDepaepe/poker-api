namespace DataAccessLayer.Exceptions;

public class InvalidInvitationException:Exception
{
    public InvalidInvitationException()
    {
    }

    public InvalidInvitationException(string message)
        : base(message)
    {
    }

    public InvalidInvitationException(string message, Exception inner)
        : base(message, inner)
    {
    }
    
}