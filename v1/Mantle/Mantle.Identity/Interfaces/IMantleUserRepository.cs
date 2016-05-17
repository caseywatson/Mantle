namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserRepository<T> :
        IMantleUserCommandService<T>,
        IMantleUserQueryService<T>
        where T : MantleUser
    {
    }
}