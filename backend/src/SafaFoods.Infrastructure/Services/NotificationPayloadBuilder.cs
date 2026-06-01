using System.Text.Json;

using SafaFoods.Core.Services;

namespace SafaFoods.Infrastructure.Services;

public sealed class NotificationPayloadBuilder : INotificationPayloadBuilder
{
    public string Build(IDictionary<string, string?> values)
    {
        var normalized = values
            .Where(x => !string.IsNullOrWhiteSpace(x.Key) && !string.IsNullOrWhiteSpace(x.Value))
            .ToDictionary(x => x.Key, x => x.Value!, StringComparer.OrdinalIgnoreCase);

        return JsonSerializer.Serialize(normalized);
    }
}
