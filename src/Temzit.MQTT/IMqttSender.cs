using System.Threading.Tasks;
using Temzit.Api;

namespace Temzit.MQTT
{
    public interface IMqttSender
    {
        Task Init();
        Task SendData(TemzitActualState state);
    }
}