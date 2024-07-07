using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using CounterBot.Configuration;
using CounterBot.Services;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CounterBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IMessageLengthService _messageLengthService;
        private readonly ISumNumberService _sumNumberService;
        private readonly IStorage _memoryStorage;


        public TextMessageController(ITelegramBotClient telegramBotClient, IMessageLengthService messageLengthService, ISumNumberService sumNumberService, MemoryStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _messageLengthService = messageLengthService;
            _sumNumberService = sumNumberService;
            _memoryStorage = memoryStorage;

        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            var userSession = _memoryStorage.GetSession(message.From.Id);
            var modeCount = userSession.ModeCount; // Получаем режим из сессии

            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Длинна сообщения" , $"long"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sum")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>Бот может считать количество символов и сумму чисел в строке.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}В ответ на условное сообщение «сова летит» он должен прислать что-то вроде «в вашем сообщении 10 символов». А в ответ на сообщение «2 3 15» должен прислать «сумма чисел: 20»." +
                        $"{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    // Обработка сообщений в зависимости от выбранного режима
                    if (modeCount == "long")
                    {
                        // Подсчет длины сообщения
                        int messageLength = _messageLengthService.CalculateLength(message.Text);
                        await _telegramClient.SendTextMessageAsync(
                            message.Chat.Id,
                            $"Длина сообщения: {messageLength} символов",
                            cancellationToken: ct);
                    }
                    else if (modeCount == "sum")
                    {
                        // Подсчет суммы чисел
                        int sum = _sumNumberService.SumNumber(message.Text);
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id,$"Сумма чисел: {sum}.",cancellationToken: ct);
                    }
                    break;
            }
        }
    }

            }
       
