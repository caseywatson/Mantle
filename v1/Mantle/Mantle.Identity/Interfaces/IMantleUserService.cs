namespace Mantle.Identity.Interfaces
{
    public interface IMantleUserService<T> :
        IMantleUserCommandService<T>,
        IMantleUserQueryService<T>
        where T : MantleUser
    {
    }
}