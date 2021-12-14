using Tizen.Applications;

using Uno.UI.Runtime.Skia;

namespace WinDevUtilityUno.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new WinDevUtilityUno.App(), args);
            host.Run();
        }
    }
}
