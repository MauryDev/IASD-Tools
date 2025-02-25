namespace VideoPlay
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //var url = args[0];
            var uri = new Uri(args.Length > 0 ? args[0] : "youtube://v/5IsHcrAznZY");
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(uri));
        }
    }
}