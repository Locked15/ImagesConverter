using System;
using System.Drawing;
using static System.Math;
using Float = System.Single;
using Colour = System.Drawing.Color;
using ConsoleColour = System.ConsoleColor;

namespace Converter
{
    /// <summary>
    /// Прочие методы, вынесенные в отдельный класс.
    /// </summary>
    public static class Other
    {
        /// <summary>
        /// Метод для получения Индекса ASCii-массива по цвету.
        /// </summary>
        /// <param name="value">Ключевое значение.</param>
        /// <param name="begin1">Значение начала расчета.</param>
        /// <param name="end1">Значение окончания первого расчета.</param>
        /// <param name="begin2">Значение начала вторичного расчета.</param>
        /// <param name="end2">Значение окончания всего расчета.</param>
        /// <returns>Индекс для ASCii-массива.</returns>
        public static Float GetIndex(this Colour colour, Float value, Float begin1, Float end1, Float begin2, Float end2)
        {
            return (value - begin1) / (end1 - begin1) * (end2 - begin2) + begin2;
        }

        /// <summary>
        /// Метод для скалирования размера изображения. Нужно для корректного вывода ASCii-представления в Консоль и Файл.
        /// </summary>
        /// <param name="image">Экземпляр класса "Bitmap", от которого вызывается метод.</param>
        /// <param name="scaleLevel">Уровень скалирования изображения. Чем больше, тем шире будет изображение.</param>>
        /// <returns>Изображение с измененным размером.</returns>
        public static Bitmap Resize(this Bitmap image, Double scaleLevel)
        {
            Double width = 400;
            Double height = Round(image.Height / scaleLevel * (width / image.Width), 3);

            if (image.Width > width || image.Height > height)
            {
                image = new Bitmap(image, new Size((Int32)width, (Int32)height));
            }

            return image;
        }

        /// <summary>
        /// Метод для получения предмета из кортежа по указанному индексу.
        /// </summary>
        /// <param name="tuple">Кортеж, из которого необходимо получить элемент.</param>
        /// <param name="index">Индекс, по которому необходимо получить элемент.</param>
        /// <returns>Элемент кортежа с указанным индексом.</returns>
        public static Double GetItemOnIndex(this (Double, Double, Double) tuple, Int32 index)
        {
            switch (index)
            {
                case 0:
                    return tuple.Item1;

                case 1:
                    return tuple.Item2;

                case 2:
                    return tuple.Item3;

                default:
                    Console.ForegroundColor = ConsoleColour.DarkRed;
                    Console.WriteLine("\nОБНАРУЖЕНА ОШИБКА В РАСШИРЕНИИ.\n");
                    Console.ResetColor();

                    return 0;
            }
        }
    }
}
