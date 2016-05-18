namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserService<TUser> :
        IMantleUserCommandService<TUser>,
        IMantleUserQueryService<TUser>
        where TUser : MantleUser
    {
    }
}