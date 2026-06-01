namespace SafaFoods.Api.Contracts.Admin;

public sealed record AdminDashboardResponse(
    int TotalCustomers,
    int ActiveSubscriptions,
    int PendingSubscriptions,
    int OpenOrders,
    int OutForDeliveryOrders,
    int PendingPaymentOrders,
    decimal TodayRevenue,
    decimal OutstandingPrepaidRevenue);
