using AllAuto.Service.Interfaces;
using Microsoft.Extensions.Logging;
using PRTelegramBot.Attributes;
using PRTelegramBot.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AllAuto.Service.Implementations
{
    public class TelegramNotificationService : INotificationService
    {
        //private readonly ILogger<TelegramNotificationService> _logger;
        private readonly ICompleteOrderService _completeOrderService;

        public TelegramNotificationService(ICompleteOrderService completeOrderService)
        {
            _completeOrderService = completeOrderService;
            //_logger = logger;
        }

        [ReplyMenuHandler("Заказы")]
        public async Task SendLastOrders(ITelegramBotClient botClient, Update update)
        {
            string message = await _completeOrderService.GetLastCompleteOrder(5);
            var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
        }
    }
}
