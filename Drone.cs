using System;

namespace Lab_4_OOP
{
    public class Drone
    {
        private string model;
        private double batteryCapacity;
        private double currentBatteryLevel;
        private double maxSpeed;

        // cтатичні приватні поля з відповідними публічними властивостями
        private static int correctlyCreatedObjectsCount = 0; // лічильник
        private static string globalManufacturerCountry = "Україна"; // загальна характеристика предметної області

        public static int CorrectlyCreatedObjectsCount
        {
            get { return correctlyCreatedObjectsCount; }
        }

        public static string GlobalManufacturerCountry
        {
            get { return globalManufacturerCountry; }
            set { globalManufacturerCountry = value; }
        }

        public string Status { get; set; } = "Готовий до вильоту";
        public int Id { get; private set; }

        public Drone()
        {
            Id = -1;
            CurrentBatteryLevel = 100.0;
        }

        public Drone(int id, string model, double batteryCapacity, double maxSpeed)
        {
            Id = id;
            Model = model;
            BatteryCapacity = batteryCapacity;
            CurrentBatteryLevel = 100.0;
            MaxSpeed = maxSpeed;

            // у випадку створення успішного обєкта збільшується лічльник
            correctlyCreatedObjectsCount++;
        }

        public Drone(int id, string model, double batteryCapacity) : this(id, model, batteryCapacity, 60.0)
        {
        }

        // довільний статик метод
        public static string GetDroneCategoryByWeight(double weightGrams)
        {
            if (weightGrams < 250) return "Мікро-дрон";
            if (weightGrams <= 2000) return "Легкий споживчий дрон";
            return "Важкий промисловий або військовий БПЛА";
        }

        // метод parse для перетворення рядка формату модель, батарея, швидкість
        public static Drone Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("Рядок для парсингу не може бути порожнім!");

            // розбиття рядку
            string[] parts = s.Split(';');
            if (parts.Length != 3)
                throw new FormatException("Некоректний формат рядка! Повинно бути: Модель;Батарея;Швидкість");

            string modelName = parts[0].Trim();

            if (!double.TryParse(parts[1], out double cap))
                throw new FormatException("Помилка парсингу: ємність батареї повинна бути числом!");

            if (!double.TryParse(parts[2], out double speed))
                throw new FormatException("Помилка парсингу: максимальна швидкість повинна бути числом!");

            return new Drone(0, modelName, cap, speed);
        }

        // метод tryparse
        public static bool TryParse(string s, out Drone obj)
        {
            try
            {
                obj = Parse(s);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
        }

        public void SetAssignedId(int assignedId)
        {
            if (Id <= 0)
            {
                Id = assignedId;
            }
        }

        // властивості з перевірками на коректність даних ведених
        public string Model
        {
            get { return model; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Назва моделі не може бути порожньою!");
                model = value;
            }
        }

        public double BatteryCapacity
        {
            get { return batteryCapacity; }
            set
            {
                if (value <= 0) throw new ArgumentException("Ємність батареї повинна бути більшою за 0!");
                batteryCapacity = value;
            }
        }

        public double CurrentBatteryLevel
        {
            get { return currentBatteryLevel; }
            set
            {
                if (value < 0 || value > 100) throw new ArgumentException("Заряд має бути від 0 до 100%!");
                currentBatteryLevel = value;
            }
        }

        public double MaxSpeed
        {
            get { return maxSpeed; }
            set
            {
                if (value <= 0) throw new ArgumentException("Максимальна швидкість повинна бути більшою за 0!");
                maxSpeed = value;
            }
        }

        public double RemainingEnergy => (batteryCapacity * currentBatteryLevel) / 100.0;

        public string StartFlight(double distance)
        {
            if (currentBatteryLevel < 15) throw new InvalidOperationException("Критичний рівень заряду!");
            CurrentBatteryLevel -= (distance * 2.5);
            return $"Політ успішний на {distance} км.";
        }

        // перевизначення методу tostring
        public override string ToString()
        {
            return $"{Model};{BatteryCapacity};{MaxSpeed}";
        }

        public string GetDetails()
        {
            return $"[ID: {Id}] {model} | Швидкість: {maxSpeed} км/год | Батарея: {currentBatteryLevel}% | Країна: {GlobalManufacturerCountry}";
        }
    }
}
