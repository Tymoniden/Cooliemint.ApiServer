using System.Text;
using Cooliemint.ApiServer.Mqtt;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cooliemint.ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqttMessageController : ControllerBase
    {
        private readonly IMessageStore _messageStore;

        public MqttMessageController(IMessageStore messageStore)
        {
            _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        }
        
        // POST api/<MqttMessageController>
        [HttpPost]
        public void Post([FromBody] MqttTextMessage value)
        {
            _messageStore.AddMessage(new MqttMessage()
            {
                Title = value.Topic,
                Body = Encoding.UTF8.GetBytes(value.Message)
            });
        }
    }

    public class MqttTextMessage
    {
        public string Topic { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
