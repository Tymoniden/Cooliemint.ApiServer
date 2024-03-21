using Cooliemint.ApiServer.Mqtt;
using Cooliemint.ApiServer.Services.Messaging;

namespace Cooliemint.ApiServer.Services.Automation
{
    public class ActionManager
    {
        private readonly IMessageConverterService _messageConverterService;
        private readonly IPushOverService _pushOverService;
        private readonly ValueStore _valueStore;
        private List<Rule> _rules = new();
        private Dictionary<string, List<Rule>> _registeredRules = new();

        public ActionManager(IMessageConverterService messageConverterService,
            IPushOverService pushOverService, ValueStore valueStore)
        {
            _messageConverterService = messageConverterService ??
                                       throw new ArgumentNullException(nameof(messageConverterService));
            _pushOverService = pushOverService ?? throw new ArgumentNullException(nameof(pushOverService));
            _valueStore = valueStore ?? throw new ArgumentNullException(nameof(valueStore));

            AddRule(new Rule()
            {
                Conditions = new List<ICondition>()
                {
                    new MqttValueCondition {Value = "true"},
                    new MqttTopicCondition {Topic = "shellies/shellyflood1/sensor/flood"}
                },
                Actions = new List<IAutomationAction>()
                {
                    new PushoverNotificationAction {Title = "Keller", Message = "Wasser im Keller"}
                }
            });
        }

        public void AddRule(Rule rule)
        {
            if (rule.Conditions.FirstOrDefault(c => c is MqttTopicCondition) is MqttTopicCondition topicCondition)
            {
                if (!_registeredRules.ContainsKey(topicCondition.Topic.ToLower()))
                {
                    _registeredRules.Add(topicCondition.Topic.ToLower(), new List<Rule>());
                }

                _registeredRules[topicCondition.Topic.ToLower()].Add(rule);
            }
        }

        public async Task HandleMqttMessage(MqttMessage mqttMessage, CancellationToken cancellationToken)
        {
            if (!_registeredRules.ContainsKey(mqttMessage.Title.ToLower()))
            {
                return;
            }

            var message = _messageConverterService.ConvertPayload<string>(mqttMessage) ?? string.Empty;

            foreach (var rule in _registeredRules[mqttMessage.Title.ToLower()])
            {
                var isValid = false;
                foreach (var condition in rule.Conditions)
                {
                    switch (condition)
                    {
                        case MqttValueCondition valueCondition:
                            isValid = HandleCondition(valueCondition, message.ToLower());
                            break;
                        case CheckValueCondition checkValueCondition:
                            isValid = HandleCondition(checkValueCondition);
                            break;
                    }

                    if (!isValid)
                    {
                        break;
                    }
                }

                if (!isValid)
                {
                    continue;
                }

                await HandleAutomationAction(rule, cancellationToken);
            }
        }

        public bool HandleCondition(MqttValueCondition mqttValueCondition, string message) =>
            mqttValueCondition.Value.ToLower().Equals(message);

        public bool HandleCondition(CheckValueCondition checkValueCondition) => _valueStore.GetValue(checkValueCondition.Key).Equals(checkValueCondition.Value);

        public async Task HandleAutomationAction(Rule rule, CancellationToken cancellationToken)
        {
            foreach (var action in rule.Actions)
            {
                switch (action)
                {
                    case PushoverNotificationAction pushoverNotificationAction:
                        await HandleAction(pushoverNotificationAction, cancellationToken);
                        break;
                    case SetValueAction setValueAction:
                        HandleAction(setValueAction);
                        break;
                }
            }
        }

        public async Task HandleAction(PushoverNotificationAction action, CancellationToken cancellationToken)
        {
            await _pushOverService.SendMessage(new AppNotification { Message = action.Message, Title = action.Title },
                cancellationToken);
        }

        public void HandleAction(SetValueAction setValueAction)
        {
            _valueStore.SetValue(setValueAction.Key, setValueAction.Value);
        }
    }
}
