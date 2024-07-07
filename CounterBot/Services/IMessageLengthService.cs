using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterBot.Services
{
    public interface IMessageLengthService
    {
        int CalculateLength(string message);
    }

    public class MessageLengthService : IMessageLengthService
    {
        public int CalculateLength(string message)
        {
            // Проверка на null или пустую строку
            if (string.IsNullOrEmpty(message)) return 0;

            // Возвращение длины сообщения
            return message.Length;
        }
    }
}

