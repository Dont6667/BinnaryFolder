using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageGrade { get; set; }

    public override string ToString()
    {
        return $"{Name}, {DateOfBirth.ToShortDateString()}, {AverageGrade}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Пожалуйста, укажите путь к бинарному файлу студентов.");
            return;
        }

        string binaryFilePath = args[0];

        try
        {
            // Читаем данные из бинарного файла
            List<Student> students = ReadStudentsFromBinaryFile(binaryFilePath);

            // Создаем директорию на рабочем столе для сохранения данных
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string studentsDirectoryPath = Path.Combine(desktopPath, "Students");
            Directory.CreateDirectory(studentsDirectoryPath);

            // Распределяем студентов по группам и сохраняем в текстовые файлы
            DistributeStudentsToGroupFiles(students, studentsDirectoryPath);

            Console.WriteLine("Программа успешно завершила выполнение.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static List<Student> ReadStudentsFromBinaryFile(string filePath)
    {
        List<Student> students;

        // Читаем все байты из файла
        byte[] jsonBytes = File.ReadAllBytes(filePath);

        // Десериализуем JSON
        students = JsonSerializer.Deserialize<List<Student>>(jsonBytes);

        return students;
    }

    static void DistributeStudentsToGroupFiles(List<Student> students, string directoryPath)
    {
        // Группируем студентов по группам
        var groups = students.GroupBy(s => s.Group);

        foreach (var group in groups)
        {
            string groupFilePath = Path.Combine(directoryPath, $"{group.Key}.txt");

            using (StreamWriter writer = new StreamWriter(groupFilePath))
            {
                foreach (var student in group)
                {
                    writer.WriteLine(student.ToString());
                }
            }
        }
    }
}
