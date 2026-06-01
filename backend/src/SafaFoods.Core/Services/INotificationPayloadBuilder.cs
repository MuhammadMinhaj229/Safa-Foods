using System.Collections.Generic;

namespace SafaFoods.Core.Services;

public interface INotificationPayloadBuilder
{
    string Build(IDictionary<string, string?> values);
}
