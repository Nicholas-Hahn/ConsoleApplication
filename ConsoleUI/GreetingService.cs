using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

//GOALS
// use dependency injection (DI)
// use Serilog
// use Settings (appsettings.json)

namespace ConsoleUI
{
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private readonly IConfiguration _config;

        public GreetingService(ILogger<GreetingService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                _log.LogError("Run number {runNumber}", i);
            }
        }
    }
}