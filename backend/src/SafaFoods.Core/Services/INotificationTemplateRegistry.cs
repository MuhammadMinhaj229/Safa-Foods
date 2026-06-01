namespace SafaFoods.Core.Services;

public interface INotificationTemplateRegistry
{
    string? GetTemplate(string eventType);
}
