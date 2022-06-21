namespace DewIt.Client.infrastructure
{
    public interface IBootstrapper
    {
        /// <summary> Load the data required to start the application. </summary>
        void Bootstrap();
    }
}
