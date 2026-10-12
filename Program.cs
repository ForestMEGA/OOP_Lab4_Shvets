using Lab_4_OOP;
using System;
using System.Collections.Generic;

namespace Lab_4_OOP
{
    class Program
    {
        private static DroneManager manager = new DroneManager();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                //  оновлена структура меню
                Console.WriteLine("\n МЕНЮ КЕРУВАННЯ ДРОНАМИ");
                Console.WriteLine("1 - Додати об'єкт");
                Console.WriteLine("2 - Переглянути всі об'єкти");
                Console.WriteLine("3 - Знайти об'єкт");
                Console.WriteLine("4 - Продемонструвати поведінку");
                Console.WriteLine("5 - Видалити об'єкт");
                Console.WriteLine("6 - Продемонструвати static-методи");
                Console.WriteLine("0 - Вийти з програми");
                Console.WriteLine("Виберіть дію:");

                string choice = Console.ReadLine()?.Trim();

                if (choice == "1") CreateDroneMenu();
                else if (choice == "2") ShowAllMenu();
                else if (choice == "3") FindMenu();
                else if (choice == "4") FlightMenu();
                else if (choice == "5") DeleteMenu();
                else if (choice == "6") StaticDemoMenu();
                else if (choice == "0") { Console.WriteLine("Вихід з програми..."); break; }
                else Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
            }
        }

        static void CreateDroneMenu()
        {
            try
            {
                // додавання через введення рядка характеристик
                Console.WriteLine("\nВведіть дані дрона в ОДИН РЯДОК через крапку з комою (Формат: Модель;Батарея;Швидкість):");
                Console.WriteLine("Приклад: DJI Phantom 4;5350;72");
                string inputRow = Console.ReadLine();

                // виклик методу TryParse для безпечного створення об'єкта з рядка
                if (Drone.TryParse(inputRow, out Drone parsedDrone))
                {
                    manager.AddDroneDirectly(parsedDrone);
                    Console.WriteLine("[УСПІХ]: Рядок успішно розпарсено за допомогою TryParse!");
                    Console.WriteLine("Дрон додано до системи.");
                }
                else
                {
                    Console.WriteLine("[ПОМИЛКА]: Метод TryParse повернув false. Некоректний формат даних.");

                    // демонстрація виклику parse, щоб показати деталі помилки
                    Console.WriteLine("Спроба розпарсити через прямий Parse для отримання деталей помилки...");
                    Drone.Parse(inputRow);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ПЕРЕХОПЛЕНО EXCEPTION]: {ex.Message}");
            }
        }

        static void ShowAllMenu()
        {
            // виведення значення статичного лічильника
            Console.WriteLine($"\nВсього КОРЕКТНО створених об'єктів у пам'яті (статичний лічильник): {Drone.CorrectlyCreatedObjectsCount}");

            List<Drone> all = manager.GetList();
            if (all.Count == 0) { Console.WriteLine("Список порожній."); return; }

            for (int i = 0; i < all.Count; i++)
            {
                Console.WriteLine(all[i].GetDetails());
                // демонстрація виклику tostring
                Console.WriteLine($"   ↳ Рядок для копіювання (ToString): {all[i].ToString()}");
            }
        }

        static void FindMenu()
        {
            Console.WriteLine("Введіть ID дрона для пошуку:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Drone found = manager.FindById(id);
                if (found != null) Console.WriteLine(found.GetDetails());
                else Console.WriteLine("Дрон із таким ID не знайдений.");
            }
        }

        static void FlightMenu()
        {
            try
            {
                Console.WriteLine("Введіть ID дрона:");
                int id = Convert.ToInt32(Console.ReadLine());
                Drone activeDrone = manager.FindById(id);
                if (activeDrone == null) { Console.WriteLine("Дрон не знайдений!"); return; }

                Console.WriteLine("Введіть дистанцію польоту (км):");
                double dist = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine(activeDrone.StartFlight(dist));
            }
            catch (Exception ex) { Console.WriteLine($"Помилка: {ex.Message}"); }
        }

        // демонстрація статик методів та властивостей
        static void StaticDemoMenu()
        {
            Console.WriteLine("\n ДЕМОНСТРАЦІЯ СТАТИЧНИХ МЕТОДІВ ТА ВЛАСТИВОСТЕЙ");

            // статична характеристикою предметної області
            Console.WriteLine($"Поточна країна-виробник за замовчуванням: {Drone.GlobalManufacturerCountry}");
            Console.WriteLine("Бажаєте змінити її для всієї системи? Введіть нову країну (або натисніть Enter щоб залишити):");
            string newCountry = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newCountry))
            {
                Drone.GlobalManufacturerCountry = newCountry;
                Console.WriteLine($"[Успіх]: Статична властивість змінена! Нове значення: {Drone.GlobalManufacturerCountry}");
            }

            // демонстрація довільного статичного методу
            Console.WriteLine("\nТестування статичного методу визначення категорії дрона за вагою.");
            Console.WriteLine("Введіть вагу дрона в грамах (наприклад, 249 або 3500):");
            if (double.TryParse(Console.ReadLine(), out double weight))
            {
                string category = Drone.GetDroneCategoryByWeight(weight);
                Console.WriteLine($"Результат виконання static-методу: {category}");
            }
            else
            {
                Console.WriteLine("Некоректна вага.");
            }
        }

        static void DeleteMenu()
        {
            Console.WriteLine("Введіть ID дрона для видалення:");
            int id = Convert.ToInt32(Console.ReadLine());
            if (manager.RemoveDrone(id)) Console.WriteLine("Об'єкт видалено.");
            else Console.WriteLine("Дрон не знайдено.");
        }
    }
}

