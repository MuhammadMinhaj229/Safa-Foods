namespace SafaFoods.Core.Services.Models;

public sealed record AdminDashboardResult(
    int TotalCustomers,
    int ActiveSubscriptions,
    int PendingSubscriptions,
    int OpenOrders,
    int OutForDeliveryOrders,
    int PendingPaymentOrders,
    decimal TodayRevenue,
    decimal OutstandingPrepaidRevenue);
