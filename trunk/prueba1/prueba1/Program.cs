using System;

namespace prueba1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MainClass game = new MainClass())
            {
                game.Run();
            }
        }
    }
#endif
}

