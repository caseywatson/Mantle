namespace Mantle.Interfaces
{
    public interface IOrderable<T>
    {
        int Order { get; set; }
        T Service { get; set; }
    }
}