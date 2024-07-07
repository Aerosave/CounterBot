using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterBot.Services
{
    public interface ISumNumberService
    {
        int SumNumber(string message);
    }

    public class SumNumberService : ISumNumberService
    {
        public  int SumNumber(string message)
        {
            // Проверка на null или пустую строку
            if (string.IsNullOrEmpty(message)) return 0;

            // Разделение строки по пробелам и преобразование каждого сегмента в число
            var numbers = message.Split(' ').Select
                (n => {
                                                           if (int.TryParse(n, out int result))
                                                           return result;
                                                           else
                                                           return 0; // Если преобразование не удалось, используем 0
                                                          }
                 );
            // Возвращение суммы всех чисел
            return numbers.Sum();
        }


    }
}
