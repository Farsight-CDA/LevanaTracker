using LevanaTracker.Api.Models.Ids;

namespace LevanaTracker.Api.Models;

public class User
{
    public UserId Id { get; private set; }

    public static User Create()
        => new User(
            new UserId(Guid.NewGuid())
        );

    private User(UserId id)
    {
        Id = id;
    }

    public User()
    {

    }
}
