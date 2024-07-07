using System;
using System.Text;
using System.Threading.Tasks;
using CounterBot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using CounterBot.Controllers;
using CounterBot.Services;
using CounterBot.Configuration;

namespace CounterBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }
        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "7405319048:AAGnFMJy1muJvaTU_lxzOae5epJ4FWMmIWY"
            };
        }

        static void ConfigureServices(IServiceCollection services)
        {
            //Создаем экземпляр класса AppSettings
            AppSettings appSettings = BuildAppSettings();
            //Регистрируем экземпляр AppSettings, возвращаемый методом BuildAppSettings()
            services.AddSingleton(BuildAppSettings());

            //Контролеры-обработчики
            services.AddTransient<DefaultMessageController>();
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
            //Регистрируем хранилище сессий
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<MemoryStorage>();
            //Регистрируем сервис подсчета
            services.AddSingleton<IMessageLengthService, MessageLengthService>();
            //Регистрируем сервис суммы чисел в строке
            services.AddSingleton<ISumNumberService, SumNumberService>();
        }
    }
}