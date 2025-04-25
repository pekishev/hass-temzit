using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Temzit.Api;

namespace Temzit.MQTT
{
    public class MqttSender : IMqttSender
    {
        private readonly IMqttClient _mqttClient;
        private readonly IMqttClientOptions _options;

        public MqttSender(IOptions<TemzitOptions> temzitOptions)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(temzitOptions.Value.MqttServer, 1883)
                .WithCredentials(temzitOptions.Value.MqttUser, temzitOptions.Value.MqttPass)
                .WithClientId("temzitmqtt")
                .Build();
        }

        public async Task Init()
        {
            await _mqttClient.ConnectAsync(_options, CancellationToken.None);

            await SendSensorConfig(nameof(TemzitActualState.HotWaterT), "Температура в баке", "temperature",
                "°C");
            await SendSensorConfig(nameof(TemzitActualState.OutsideT), "Уличная температура",
                "temperature", "°C");
            
            await SendSensorConfig(nameof(TemzitActualState.InputPower), "Потребление", "power", "kW");
            await SendSensorConfig(nameof(TemzitActualState.OutputPower), "Выходная мощность", "power", "kW");
            await SendSensorConfig(nameof(TemzitActualState.InHeatT), "Обратка", "temperature", "°C");
            await SendSensorConfig(nameof(TemzitActualState.OutHeatT), "Подача", "temperature", "°C");
            await SendSensorConfig(nameof(TemzitActualState.COP), "СОР", "power_factor", "");
            await SendSensorConfig(nameof(TemzitActualState.Compressor1), "Компрессор", "frequency", "Hz");
            await SendSensorConfig(nameof(TemzitActualState.LiquidSpeed), "Поток", "water", "");
        }

        public async Task SendData(TemzitActualState state)
        {
            if (state == null)
                return;

            await _mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic("temzit")
                .WithPayload(JsonSerializer.Serialize(state))
                .Build(), CancellationToken.None);
        }

        private async Task SendSensorConfig(string objectId, string name, string deviceClass, string unit)
        {
            var device = new Device
            {
                Identifiers = new[] {"kotel_1"},
                Manufacturer = "Temzit",
                Model = "Темзит",
                Name = "Тепловой насос Темзит",
                SwVersion = "0.1"
            };
            var sensor = new TemzitSensor
            {
                Device = device,
                DeviceClass = deviceClass,
                Name = $"{device.Name} {name}",
                StateTopic = $"temzit",
                JsonAttributesTopic = $"temzit",
                UnitOfMeasurement = unit,
                UniqueId = $"temzit_{objectId}",
                ValueTemplate = $"{{{{ value_json.{objectId} }}}}"
            };

            var jsonString = JsonSerializer.Serialize(sensor,
                new JsonSerializerOptions {PropertyNamingPolicy = new SnakeCaseNamingPolicy()});

            var message = new MqttApplicationMessageBuilder()
                .WithTopic($"homeassistant/sensor/temzit/{objectId}/config")
                .WithPayload(jsonString)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(message, CancellationToken.None);
        }
    }
}