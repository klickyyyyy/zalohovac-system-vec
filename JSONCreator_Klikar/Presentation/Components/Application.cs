namespace JSONCreator.Presentation.Components
{
    public class Application
    {
        public bool Running { get; private set; } = true;

        public void Stop()
        {
            Running = false;
        }
    }
}
