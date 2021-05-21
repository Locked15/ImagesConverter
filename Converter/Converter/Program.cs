using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Bool = System.Boolean;
using Colour = System.Drawing.Color;
using ConsoleColour = System.ConsoleColor;

namespace Converter
{
    /// <summary>
    /// Основной класс программы.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Поле, содержащее экземпляр класса "Converter".
        /// </summary>
        static Converter image;

        /// <summary>
        /// Словарь, содержащий все возможные действия для вывода информации в консоль.
        /// </summary>
        static Dictionary<String, String> actions = new Dictionary<String, String>(1);

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args">Аргументы сценария.</param>
        static void Main (String[] args)
        {
            Console.Title = "Конвертер Изображений.";
            String choise = "";

            //Добавление значений в словарь с возможными действиями.
            Task.Run(() =>
            {
                actions.Add("?", "Вывод справки.");
                actions.Add("Ч", "Очистка консоли.");
                actions.Add("О", "Возвращение к оригинальному изображению.");
                actions.Add("С", "Сохранение измененного изображения.");
                actions.Add("А", "Вывод информации о текущем алгоритме конвертации.");
                actions.Add("С/А", "Смена алгоритма преобразования изображения.");
                actions.Add("П/Д", "Смена активного изображения на другое.");
                actions.Add("И/Р", "Изменение размеров активного изображения.");
                actions.Add("Ч/Б", "Перевод в Черно-Белый формат без сохранения изображения.");
                actions.Add("Ч/Б/С", "Перевод в Черно-Белый формат с сохранением изображения.");
                actions.Add("Н/Г", "Перевод в Негатив без сохранения изображения.");
                actions.Add("Н/Г/С", "Перевод в Негатив с сохранением изображения.");
                actions.Add("С/П", "Перевод в Сепию без сохранения изображения.");
                actions.Add("С/П/С", "Перевод в Сепию с сохранением изображения.");
                actions.Add("A/I", "Создание ASCii-варианта текущего изображения без сохранения в файл.");
                actions.Add("A/I/С", "Создание ASCii-варианта текущего изображения с сохранением в файл.");
                actions.Add("A/I/П", "Вывод ASCii-варианта текущего изображения в Консоль.");
                actions.Add("A/С", "Сохранение текущего ASCii-изображения в файл.");
            });

            Console.ForegroundColor = ConsoleColour.Green;
            Console.WriteLine("Добро пожаловать в Конвертер Изображений!");
            Console.ResetColor();

            Converter convert = InitializeNewConverter();

            while (choise.ToUpper() != "Y")
            {
                MakeAction();

                Console.Write("\nВы желаете завершить сеанс работы с программой (Y/N)? ");
                choise = Console.ReadKey().KeyChar.ToString();
            }

            if (convert.Changed)
            {
                convert.SaveImage("");
            }

            Console.ForegroundColor = ConsoleColour.Green;
            Console.WriteLine("\n\nРабота завершена. Спасибо за использование!");
            Console.ResetColor();

            Console.ReadLine();
        }

        /// <summary>
        /// Метод для создания нового экземпляра класса "Converter".
        /// </summary>
        /// <returns>Экземпляр класса "Converter", готовый к работе.</returns>
        static Converter InitializeNewConverter ()
        {
            Console.Write("\nВведите путь к изображению, которое Вы хотите изменить: ");
            String path = Console.ReadLine();

            Console.Write("\nИспользовать Быстрый Алгоритм при работе с изображением (Y/N)? ");
            image = new Converter(path, Console.ReadKey().KeyChar == 'Y');

            while (!image.Ready)
            {
                Console.ForegroundColor = ConsoleColour.DarkYellow;

                Console.Write("\nИз-за возникшей ошибки введите путь к изображению еще раз: ");
                image.ChangeImage(Console.ReadLine());

                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColour.Green;
            Console.WriteLine("\n\n\nИзображение готово к работе!");
            Console.ResetColor();

            return image;
        }

        /// <summary>
        /// Метод для выбора действия с изображением.
        /// </summary>
        static void MakeAction ()
        {
            String currentAction;

            Console.Write("\n\nВыберите действие, которое необходимо провести с изображением или программой: ");
            currentAction = Console.ReadLine().ToUpper();

            ShowInfo(currentAction);

            if (currentAction == "Ч/Б")
            {
                image.GrayGamma();
            }

            else if (currentAction == "Ч/Б/С")
            {
                Console.Write("Введите путь, по которому будет сохранено изображение (пустой ввод сохранит файл в папку с оригиналом): ");
                image.GrayGamma(Console.ReadLine());
            }

            else if (currentAction == "Н/Г")
            {
                image.NegativeGamma();
            }

            else if (currentAction == "Н/Г/С")
            {
                Console.Write("Введите путь, по которому будет сохранено изображение (пустой ввод сохранит файл в папку с оригиналом): ");
                image.NegativeGamma(Console.ReadLine());
            }

            else if (currentAction == "С/П")
            {
                image.SepiaGamma();
            }

            else if (currentAction == "С/П/С")
            {
                Console.Write("Введите путь, по которому будет сохранено изображение (пустой ввод сохранит файл в папку с оригиналом): ");
                image.SepiaGamma(Console.ReadLine());
            }

            else if (currentAction == "П/Д")
            {
                if (image.Changed)
                {
                    Console.ForegroundColor = ConsoleColour.Red;
                    Console.Write("\nВНИМАНИЕ: Измененное изображение не сохранено. Уничтожить изменения (Y/N)? ");

                    if (Console.ReadKey().KeyChar != 'Y')
                    {
                        Console.ResetColor();

                        image.SaveImage("");
                    }
                }

                InitializeNewConverter();
            }

            else if (currentAction == "A/I")
            {
                image.ASCiiConvert();
            }

            else if (currentAction == "A/I/С")
            {
                Console.Write("Введите путь, по которому будет сохранено ASCii-представление (пустой ввод сохранит файл в папку с оригиналом): ");
                image.ASCiiConvert(Console.ReadLine());
            }

            else if (currentAction == "A/I/П")
            {
                image.ASCiiShow();
            }

            else if (currentAction == "A/С")
            {
                Console.Write("Введите путь, по которому будет сохранено ASCii-представление (пустой ввод сохранит файл в папку с оригиналом): ");
                image.SaveASCii(Console.ReadLine());
            }

            else if (currentAction == "И/Р")
            {
                Double level;

                Console.Write("Введите степень скалирования размеров изображения: ");

                while (!Double.TryParse(Console.ReadLine(), out level))
                {
                    Console.Write("\nВведено некорректное значение, повторите ввод: ");
                }

                image.ResizeImage(level);
            }

            else if (currentAction == "С/А")
            {
                Console.Write($"Текущий используемый алгоритм: {(image.FastAlgorithm ? "Быстрый." : "Обычный.")}.\nПереключить его (Y/N)? ");
                if (Console.ReadKey().KeyChar == 'Y')
                {
                    image.ChangeAlgorithm();
                }
            }

            else if (currentAction == "А")
            {
                Console.Write($"Текущий алгоритм: {(image.FastAlgorithm ? "Быстрый" : "Обычный")}.");
            }

            else if (currentAction == "С")
            {
                Console.Write("Введите путь, по которому будет сохранено изображение (пустой ввод сохранит файл в папку с оригиналом): ");
                image.SaveImage(Console.ReadLine());
            }

            else if (currentAction == "О")
            {
                if (image.Changed)
                {
                    Console.ForegroundColor = ConsoleColour.Red;
                    Console.Write("Обнаружены несохраненные изменения. Все равно сбросить изменения (Y/N)? ");

                    if (Console.ReadKey().KeyChar == 'Y')
                    {
                        Console.ResetColor();

                        image.Reset();
                    }
                }

                else
                {
                    if (image.ASCiiImage != null)
                    {
                        Console.ForegroundColor = ConsoleColour.DarkYellow;
                        Console.Write("Обновить ASCii-представление (Y/N)? ");

                        if (Console.ReadKey().KeyChar == 'Y')
                        {
                            image.Reset(true);
                        }

                        else
                        {
                            image.Reset();
                        }

                        Console.ResetColor();
                    }
                }
            }

            else if (currentAction == "Ч")
            {
                Console.Write("\nПроцедура очистки инициализирована...\n");

                for (int i = 3; i > 0; i--)
                {
                    Console.WriteLine($"Очистка через {i}...");

                    System.Threading.Thread.Sleep(1000);
                }

                Console.Clear();
            }

            else if (currentAction == "?")
            {
                Console.WriteLine("Введите \"Ч/Б\", чтобы преобразовать изображение в Черно-Белый формат.");
                Console.WriteLine("Введите \"Ч/Б/С\", чтобы преобразовать изображение в Черно-Белый формат, а затем сохранить его.");
                Console.WriteLine("Введите \"Н/Г\", чтобы преобразовать изображение в Негатив.");
                Console.WriteLine("Введите \"Н/Г/С\", чтобы преобразовать изображение в Негатив, а затем сохранить его.");
                Console.WriteLine("Введите \"С/П\", чтобы преобразовать изображение в Сепию.");
                Console.WriteLine("Введите \"С/П/С\", чтобы преобразовать изображение в Сепию, а затем сохранить его.");
                Console.WriteLine("Введите \"A/I\", чтобы создать ASCii-представление текущего изображения.");
                Console.WriteLine("Введите \"A/I/С\", чтобы создать ASCii-представление текущего изображения, а затем записать его в файл.");
                Console.WriteLine("Введите \"A/I/П\", чтобы вывести в консоль ASCii-представление текущего изображения.");
                Console.WriteLine("Введите \"A/С\", чтобы сохранить ASCii-представление текущего изображения.");
                Console.WriteLine("Введите \"П/Д\", чтобы сменить активное изображение.");
                Console.WriteLine("Введите \"С/А\", чтобы сменить используемый Алгоритм конвертации.");
                Console.WriteLine("Введите \"А\", чтобы вывести на консоль текущий используемый Алгоритм конвертации.");
                Console.WriteLine("Введите \"О\", чтобы сбросить все изменения изображения.");
                Console.WriteLine("Введите \"С\", чтобы сохранить измененное изображение.");
                Console.WriteLine("Введите \"Ч\", чтобы очистить консоль.");
            }
        }

        /// <summary>
        /// Метод для вывода информации о выбранном пользователем действии.
        /// </summary>
        /// <param name="chosenAction">Выбранное пользователем действие.</param>>
        static void ShowInfo (String chosenAction)
        {
            try
            {
                Console.WriteLine($"\nВыбранное действие:\n{actions[chosenAction]}");
            }

            catch (KeyNotFoundException)
            {
                Console.WriteLine("\nВведено неизвестное действие.");
            }
        }
    }

    /// <summary>
    /// Класс для работы с изображениями.
    /// </summary>
    class Converter
    {
        /// <summary>
        /// Поле, содержащее массив с "ASCii-символами", которые нужны для конвертации изображения.
        /// </summary>
        readonly Char[] asciiSymbols = { '.', ',', ':', '+', '*', '?', '%', 'S', '#', '@' };

        /// <summary>
        /// Статическое Поле, содержащее допустимые файлы для работы.
        /// </summary>
        static List<String> allowedFiles = new List<String>
        {".bmp", ".png", ".jpg", ".jpeg"};

        /// <summary>
        /// Скрытое Поле, содержащее ASCii-изображение в Негативе. Нужно для записи в файл.
        /// </summary>
        Char[][] asciiNegativeImage;

        /// <summary>
        /// Поле, содержащее изображение, преобразованное в ASCii.
        /// </summary>
        Char[][] asciiImage;

        /// <summary>
        /// Поле, содержащее Полное Имя оригинального изображения.
        /// </summary>
        String fileName;

        /// <summary>
        /// Закрытое поле, содержащее Полный Путь к файлу. Нужно для работы сохранения по умолчанию.
        /// </summary>
        String fullPath;

        /// <summary>
        /// Поле, содержащее измененное изображение.
        /// </summary>
        Bitmap newImage;

        /// <summary>
        /// Поле, содержащее оригинальное изображение.
        /// </summary>
        Bitmap image;

        /// <summary>
        /// Поле, отвечающее за изменность файла после его сохранения/открытия.
        /// </summary>
        Bool changed;

        /// <summary>
        /// Поле, содержащее Логическое Значение, отвечающее за то, что экземпляр класса готов к работе.
        /// </summary>
        Bool ready;

        /// <summary>
        /// Поле, отвечающее за то, будет ли использоваться Быстрый Алгоритм конвертации при работе с изображениями.
        /// </summary>
        Bool fastAlg = false;

        /// <summary>
        /// Постоянная, содержащая модификатор спектра Red для получения Сепии.
        /// </summary>
        const Double sepiaModR = 1.0;

        /// <summary>
        /// Постоянная, содержащая модификатор спектра Green для получения Сепии.
        /// </summary>
        const Double sepiaModG = 0.95;

        /// <summary>
        /// Постоянная, содержащая модификатор спектра Blue для получения Сепии.
        /// </summary>
        const Double sepiaModB = 0.82;

        /// <summary>
        /// Свойство, содержащее оригинальное изображение.
        /// </summary>
        public Bitmap Image
        {
            get
            {
                return image;
            }

            private set
            {
                image = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее измененное изображение.
        /// </summary>
        public Bitmap NewImage
        {
            get
            {
                return newImage;
            }

            private set
            {
                newImage = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее преобразованное в ASCii изображение.
        /// </summary>
        public Char[][] ASCiiImage
        {
            get
            {
                return asciiImage;
            }

            private set
            {
                asciiImage = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее Логическое Значение, отвечающее за то, что экземпляр класса готов к работе.
        /// </summary>
        public Bool Ready
        {
            get
            {
                return ready;
            }

            private set
            {
                ready = value;
            }
        }

        /// <summary>
        /// Свойство, отвечающее за изменность файла после его сохранения/открытия.
        /// </summary>
        public Bool Changed
        {
            get
            {
                return changed;
            }

            private set
            {
                changed = value;
            }
        }

        /// <summary>
        /// Свойство, отвечающее за то, будет ли использоваться Быстрый Алгоритм конвертации при работе с изображениями. 
        /// </summary>
        public Bool FastAlgorithm
        {
            get
            {
                return fastAlg;
            }

            private set
            {
                fastAlg = value;
            }
        }

        /// <summary>
        /// Свойство, содержащее Полное Имя оригинального изображения.
        /// </summary>
        public String FileName
        {
            get
            {
                return fileName;
            }

            private set
            {
                fileName = value;
            }
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="pathToFile">Абсолютный путь к изображению.</param>
        /// <param name="useFastAlgorithm">Отвечает за то, будет ли применяться Быстрый Алгоритм конвертации изображения.</param>>
        public Converter (String pathToFile, Bool useFastAlgorithm)
        {
            if (File.Exists(pathToFile) && allowedFiles.Contains(Path.GetExtension(pathToFile)))
            {
                Image = Bitmap.FromFile(pathToFile) as Bitmap;
                FileName = Path.GetFileName(pathToFile);
                fastAlg = useFastAlgorithm;
                fullPath = pathToFile;
                ASCiiImage = null;
                NewImage = Image;

                Ready = true;
            }

            else if (File.Exists(pathToFile))
            {
                Console.ForegroundColor = ConsoleColour.Red;
                Console.WriteLine("УКАЗАН ФАЙЛ С НЕКОРРЕКТНЫМ РАСШИРЕНИЕМ.");
                Console.ResetColor();
            }

            else
            {
                Console.ForegroundColor = ConsoleColour.Red;
                Console.WriteLine("УКАЗАН НЕКОРРЕТНЫЙ ПУТЬ.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Метод для смены активного изображения у экземпляра класса.
        /// </summary>
        /// <param name="newPathToImage">Абсолютный Путь к новому изображению.</param>
        public void ChangeImage (String newPathToImage)
        {
            if (File.Exists(newPathToImage) && allowedFiles.Contains(Path.GetExtension(newPathToImage)))
            {
                if (changed)
                {
                    SaveImage("");
                }

                Image = Bitmap.FromFile(newPathToImage) as Bitmap;
                FileName = Path.GetFileName(newPathToImage);
                fullPath = newPathToImage;
                asciiNegativeImage = null;
                ASCiiImage = null;
                NewImage = Image;

                Changed = false;
                Ready = true;
            }

            else if (File.Exists(newPathToImage))
            {
                Console.ForegroundColor = ConsoleColour.Red;
                Console.WriteLine("УКАЗАН ФАЙЛ С НЕКОРРЕКТНЫМ РАСШИРЕНИЕМ.");
                Console.ResetColor();
            }

            else
            {
                Console.ForegroundColor = ConsoleColour.Red;
                Console.WriteLine("УКАЗАН НЕКОРРЕТНЫЙ ПУТЬ.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Метод для изменения размеров изображения и их скалирования.
        /// </summary>
        /// <param name="level">Степень изменения размера изображения.</param>>
        public void ResizeImage (Double level)
        {
            if (level < 0.5 || level > 2.25)
            {
                Console.ForegroundColor = ConsoleColour.DarkYellow;
                Console.Write("\nВНИМАНИЕ: Введено критическое значение скалирования, способное сильно исказить изображение.\n" +
                "Вы точно уверены, что хотите продолжить (Y/N)? ");

                if (Console.ReadKey().KeyChar == 'Y')
                {
                    Console.WriteLine();
                    Console.ResetColor();
                }

                else
                {
                    Console.WriteLine();
                    Console.ResetColor();

                    return;
                }
            }

            Image = Image.Resize(level);
        }

        /// <summary>
        /// Метод для преобразования изображения в черно-белый цвет.
        /// </summary>
        public void GrayGamma ()
        {
            if (FastAlgorithm)
            {
                BitmapData data = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Byte[] imageBytesMap = new Byte[Image.Width * Image.Height * 3];

                Marshal.Copy(data.Scan0, imageBytesMap, 0, imageBytesMap.Length);

                for (int i = 0; i < imageBytesMap.Length; i += 3)
                {
                    Int32 channelColours = 0;

                    for (int j = 0; j < 3; j++)
                    {
                        channelColours += imageBytesMap[i + j];
                    }

                    Byte newColour = Convert.ToByte(channelColours / 3);

                    for (int j = 0; j < 3; j++)
                    {
                        imageBytesMap[i + j] = newColour;
                    }
                }

                Marshal.Copy(imageBytesMap, 0, data.Scan0, imageBytesMap.Length);

                Image.UnlockBits(data);
            }

            else
            {
                for (int i = 0; i < Image.Width; i++)
                {
                    for (int j = 0; j < Image.Height; j++)
                    {
                        Int32 newColour = (Image.GetPixel(i, j).R + Image.GetPixel(i, j).G + Image.GetPixel(i, j).B) / 3;

                        NewImage.SetPixel(i, j, Colour.FromArgb(newColour, newColour, newColour));
                    }
                }
            }

            Changed = true;
        }

        /// <summary>
        /// Метод для преобразования изображения в черно-белый цвет.
        /// </summary>
        /// <param name="pathToSave">Абсолютный путь, по которому будет сохранено изображение.</param>
        public void GrayGamma (String pathToSave)
        {
            GrayGamma();

            SaveImage(pathToSave);
        }

        /// <summary>
        /// Метод для преобразования изображения в Негатив.
        /// </summary>
        public void NegativeGamma ()
        {
            if (FastAlgorithm)
            {
                BitmapData data = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                Byte[] imageBytesMap = new Byte[Image.Width * Image.Height * 3];

                Marshal.Copy(data.Scan0, imageBytesMap, 0, imageBytesMap.Length);

                for (int i = 0; i <imageBytesMap.Length; i += 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        imageBytesMap[i + j] = Convert.ToByte(255 - imageBytesMap[i + j]);
                    }
                }

                Marshal.Copy(imageBytesMap, 0, data.Scan0, imageBytesMap.Length);

                Image.UnlockBits(data);
            }

            else
            {
                for (int i = 0; i < Image.Width; i++)
                {
                    for (int j = 0; j < Image.Height; j++)
                    {
                        Int32 r = 255 - Image.GetPixel(i, j).R;
                        Int32 g = 255 - Image.GetPixel(i, j).G;
                        Int32 b = 255 - Image.GetPixel(i, j).B;

                        NewImage.SetPixel(i, j, Colour.FromArgb(r, g, b));
                    }
                }
            }

            Changed = true;
        }

        /// <summary>
        /// Метод для преобразования изображения в Негатив.
        /// </summary>
        /// <param name="pathToSave">Абсолютный путь, по которому будет сохранено изображение.</param>
        public void NegativeGamma (String pathToSave)
        {
            NegativeGamma();

            SaveImage(pathToSave);
        }

        /// <summary>
        /// Метод для преобразования изображения в Сепию.
        /// </summary>
        public void SepiaGamma ()
        {
            //Для преобразования в Сепию сперва необходимо сделать изображение Черно-Белым.
            //Данный метод сам изменит состояние Свойства "Changed" на true, так что здесь оно не изменяется.
            GrayGamma();

            if (FastAlgorithm)
            {
                BitmapData data = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                (Double, Double, Double) sepiaModifiers = (sepiaModR, sepiaModG, sepiaModB); 
                Byte[] imageByteMap = new Byte[Image.Width * Image.Height * 3];

                Marshal.Copy(data.Scan0, imageByteMap, 0, imageByteMap.Length);

                for (int i = 0; i < imageByteMap.Length; i += 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        //Карта байтов, которую мы получили является "инвертированной".
                        //Иными словами, значения цветов идут в таком порядке: Blue -> Green -> Red.
                        //Соответственно, чтобы получить "правильную" Сепию, нам нужно получать инвертированные индексы:
                        imageByteMap[i + j] = Convert.ToByte(imageByteMap[i + j] * sepiaModifiers.GetItemOnIndex(2 - j));
                    }
                }

                Marshal.Copy(imageByteMap, 0, data.Scan0, imageByteMap.Length);

                Image.UnlockBits(data);
            }

            else
            {
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        Int32 r = Convert.ToInt32(image.GetPixel(i, j).R * sepiaModR);
                        Int32 g = Convert.ToInt32(image.GetPixel(i, j).G * sepiaModG);
                        Int32 b = Convert.ToInt32(image.GetPixel(i, j).B * sepiaModB);

                        image.SetPixel(i, j, Colour.FromArgb(r, g, b));
                    }
                }
            }
        }

        /// <summary>
        /// Метод для преобразования изображения в Сепию.
        /// </summary>
        /// <param name="pathToSave">Абсолютный Путь, по которому будет сохранено изображение.</param>
        public void SepiaGamma (String pathToSave)
        {
            SepiaGamma();

            SaveImage(pathToSave);
        }

        /// <summary>
        /// Метод для конвертации изображения в ASCii-формат.
        /// </summary>
        public void ASCiiConvert ()
        {
            ASCiiImage = new Char[image.Height][];

            for (int i = 0; i < image.Height; i++)
            {
                ASCiiImage[i] = new Char[image.Width];

                for (int j = 0; j < image.Width; j++)
                {
                    Int32 index = (Int32)new Colour().GetIndex(image.GetPixel(j, i).R, 0, 255, 0, asciiSymbols.Length - 1);

                    ASCiiImage[i][j] = asciiSymbols[index];
                }
            }

            asciiNegativeImage = new Char[image.Height][];

            for (int i = 0; i < image.Height; i++)
            {
                asciiNegativeImage[i] = new Char[image.Width];

                for (int j = 0; j < image.Width; j++)
                {
                    Int32 index = (Int32)new Colour().GetIndex(image.GetPixel(j, i).R, 0, 255, 0, asciiSymbols.Length - 1);

                    asciiNegativeImage[i][j] = asciiSymbols.Reverse().ToArray()[index];
                }
            }
        }

        /// <summary>
        /// Метод для конвертации изображения в ASCii-формат.
        /// </summary>
        /// <param name="pathToSave">Путь, по которому будет сохранено изображение.</param>
        public void ASCiiConvert (String pathToSave)
        {
            ASCiiConvert();

            SaveASCii(pathToSave);
        }

        /// <summary>
        /// Метод для вывода ASCii-изображения в Консоль.
        /// </summary>
        public void ASCiiShow ()
        {
            if (ASCiiImage != null)
            {
                Console.WriteLine();

                for (int i = 0; i < ASCiiImage.Length; i++)
                {
                    Console.WriteLine(ASCiiImage[i]);
                }
            }
        }

        /// <summary>
        /// Метод для сохранения Нового Изображения.
        /// </summary>
        /// <param name="pathToSave">Путь, по которому необходимо сохранить новое изображение.</param>
        public void SaveImage (String pathToSave)
        {
            String newSavePath;

            if (!String.IsNullOrEmpty(pathToSave))
            {
                if (pathToSave.EndsWith("\\"))
                {
                    pathToSave.TrimEnd('\\');
                }

                newSavePath = pathToSave + $"\\New{FileName}";
            }

            else
            {
                newSavePath = fullPath.Substring(0, fullPath.LastIndexOf("\\")) + $"\\New{FileName}";
            }

            if (Directory.Exists(Path.GetFullPath(Path.GetDirectoryName(newSavePath))))
            {
                if (File.Exists(newSavePath))
                {
                    Int32 i = 1;
                    String extension = Path.GetExtension(newSavePath);
                    String cuttedPath = newSavePath.Remove(newSavePath.Length - extension.Length);

                    while (File.Exists($"{cuttedPath} ({i}){extension}"))
                    {
                        i++;
                    }

                    newSavePath = $"{cuttedPath} ({i}){extension}";
                }

                NewImage.Save(newSavePath);

                Changed = false;
            }

            else
            {
                Console.ForegroundColor = ConsoleColour.Red;
                Console.WriteLine("\nВВЕДЕН НЕКОРРЕКТНЫЙ ПУТЬ ДЛЯ СОХРАНЕНИЯ ФАЙЛА.\n");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Метод для сохранения ASCii-представления текущего изображения.
        /// </summary>
        /// <param name="pathToSave">Путь к сохраняемому файлу.</param>
        public void SaveASCii (String pathToSave)
        {
            if (ASCiiImage != null)
            {
                String newSavePath;

                if (!String.IsNullOrEmpty(pathToSave))
                {
                    if (pathToSave.EndsWith("\\"))
                    {
                        pathToSave.TrimEnd('\\');
                    }

                    newSavePath = pathToSave + $"\\New{FileName}";
                }

                else
                {
                    newSavePath = fullPath.Substring(0, fullPath.LastIndexOf("\\")) + $"\\New{Path.GetFileNameWithoutExtension(FileName)}.txt";
                }

                if (Directory.Exists(Path.GetFullPath(Path.GetDirectoryName(newSavePath))))
                {
                    if (File.Exists(newSavePath))
                    {
                        Int32 i = 1;
                        String extension = ".txt";
                        String cuttedPath = newSavePath.Remove(newSavePath.Length - extension.Length);

                        while (File.Exists($"{cuttedPath} ({i}){extension}"))
                        {
                            i++;
                        }

                        newSavePath = $"{cuttedPath} ({i}){extension}";
                    }

                    using (StreamWriter sw1 = new StreamWriter(newSavePath, false, System.Text.Encoding.Default))
                    {
                        for (int i = 0; i < asciiNegativeImage.Length; i++)
                        {
                            for (int j = 0; j < asciiNegativeImage[i].Length; j++)
                            {
                                sw1.Write(asciiNegativeImage[i][j]);
                            }

                            sw1.WriteLine();
                        }
                    }
                }

                else
                {
                    Console.ForegroundColor = ConsoleColour.Red;
                    Console.WriteLine("\nВВЕДЕН НЕКОРРЕКТНЫЙ ПУТЬ ДЛЯ СОХРАНЕНИЯ ФАЙЛА.\n");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Метод для смены используемого алгоритма.
        /// </summary>
        public void ChangeAlgorithm ()
        {
            FastAlgorithm = !FastAlgorithm;
        }

        /// <summary>
        /// Метод для приведения Измененного Изображения к оригинальному виду.
        /// </summary>
        /// <param name="updateASCii">Необязательный Параметр. Отвечает за дополнительное преобразование ASCii к оригинальному виду.</param>>
        public void Reset (Bool updateASCii = false)
        {
            NewImage = Image;
            Changed = false;

            if (updateASCii && ASCiiImage != null)
            {
                ASCiiConvert();
            }

            else
            {
                ASCiiImage = null;
                asciiNegativeImage = null;
            }
        }
    }
}
