namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserRepository<TUser> : 
        IMantleUserService<TUser>
        where TUser : MantleUser
    {
    }
}