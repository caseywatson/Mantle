namespace Mantle.Interfaces
{
    public interface ICanBeAutoSetup
    {
        bool AutoSetup { get; set; }
        void Setup();
    }
}