using SafaFoods.Core.Entities;
using SafaFoods.Core.Enums;
using SafaFoods.Infrastructure.Services;

namespace SafaFoods.Api.Tests;

public sealed class NotificationMessageRendererTests
{
    private readonly NotificationMessageRenderer _renderer = new(new NotificationTemplateRegistry());

    [Fact]
    public void Render_ReturnsExpectedMessage_ForOrderPaymentConfirmed()
    {
        var notification = new NotificationLog
        {
            Channel = NotificationChannel.WhatsApp,
            EventType = "order_payment_confirmed",
            Recipient = "9908597337",
            MessageStatus = "queued",
            PayloadJson = """{"paymentReference":"UPI12345"}"""
        };

        var message = _renderer.Render(notification);

        Assert.Equal("Safa Foods payment confirmed. Reference: UPI12345. Your order is now cleared for processing.", message);
    }

    [Fact]
    public void Render_UsesPayloadMessage_WhenProvided()
    {
        var notification = new NotificationLog
        {
            Channel = NotificationChannel.WhatsApp,
            EventType = "custom_event",
            Recipient = "9908597337",
            MessageStatus = "queued",
            PayloadJson = """{"message":"Custom Safa Foods update."}"""
        };

        var message = _renderer.Render(notification);

        Assert.Equal("Custom Safa Foods update.", message);
    }

    [Fact]
    public void Render_FallsBackToEventDescription_WhenNoTemplateOrPayloadExists()
    {
        var notification = new NotificationLog
        {
            Channel = NotificationChannel.WhatsApp,
            EventType = "delivery_slot_reserved",
            Recipient = "9908597337",
            MessageStatus = "queued"
        };

        var message = _renderer.Render(notification);

        Assert.Equal("Safa Foods update: delivery slot reserved.", message);
    }
}
