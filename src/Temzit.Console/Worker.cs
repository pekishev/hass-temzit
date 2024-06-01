using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Temzit.Api;
using Temzit.MQTT;

namespace Temzit.Console
{
    public class Worker : BackgroundService
    {
        private readonly IMqttSender _mqttSender;
        private readonly ITemzitReader _temzitReader;
        private Timer _timer;

        public Worker(ITemzitReader temzitReader, IMqttSender mqttSender)
        {
            _temzitReader = temzitReader;
            _mqttSender = mqttSender;
        }

        private async void Callback(object state2)
        {
            try
            {
                var state = await _temzitReader.ReadState();
                await _mqttSender.SendData(state);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _timer?.Dispose());
            //var state = await _temzitReader.ReadState();
            await _mqttSender.Init();
            _timer = new Timer(Callback, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
        }
    }
}