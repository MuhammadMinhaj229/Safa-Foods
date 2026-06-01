using SafaFoods.Core.Entities;

namespace SafaFoods.Core.Services;

public interface INotificationMessageRenderer
{
    string Render(NotificationLog notification);
}
