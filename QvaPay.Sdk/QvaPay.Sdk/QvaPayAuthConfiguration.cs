using Microsoft.Extensions.Configuration;

namespace QvaPay.Sdk
{
    public class QvaPayAuthConfiguration
    {
        private const string _defaultConfigPrefix = "QvaPayConfiguration";

        public string AppId { get; set; }
        public string AppSecret { get; set; }

        public QvaPayAuthConfiguration(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }

        public QvaPayAuthConfiguration(IConfiguration configuration, string configurationPrefix = _defaultConfigPrefix)
        {
            AppId = configuration.GetValue<string>($"{configurationPrefix}:AppId");
            AppSecret = configuration.GetValue<string>($"{configurationPrefix}:AppSecret");
        }

        public QvaPayAuthConfiguration(IConfigurationRoot configuration, string configurationPrefix = _defaultConfigPrefix) : this((IConfiguration)configuration, configurationPrefix)
        {
        }
    }
}
