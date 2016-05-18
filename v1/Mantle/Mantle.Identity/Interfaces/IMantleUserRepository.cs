namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserRepository<TUser> :
        IMantleUserCommandService<TUser>,
        IMantleUserQueryService<TUser>
        where TUser : MantleUser
    {
    }
}