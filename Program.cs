using System;
using System.Collections.Generic;
using System.Linq;

public class Group
{
    private List<Student> students;
    public string Name { get; set; }
    public string Specialization { get; set; }
    public int Course { get; set; }

    public Group()
    {
        students = new List<Student>();
    }

    public Group(Student[] studentsArray)
    {
        students = studentsArray.ToList();
    }

    public Group(List<Student> studentsList)
    {
        students = new List<Student>(studentsList);
    }

    public Group(Group otherGroup)
    {
        Name = otherGroup.Name;
        Specialization = otherGroup.Specialization;
        Course = otherGroup.Course;
        students = new List<Student>(otherGroup.students);
    }

    public void ShowAllStudents()
    {
        Console.WriteLine($"{Name} ({Specialization}), {Course} курс:");
        var orderedStudents = students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
        for (int i = 0; i < orderedStudents.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {orderedStudents[i]}");
        }
    }

    public void AddStudent(Student student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student), "Студент не может быть null");
        }
        students.Add(student);
    }

    public void EditStudent(Student oldStudent, Student newStudent)
    {
        if (oldStudent == null)
        {
            throw new ArgumentNullException(nameof(oldStudent), "Старый студент не может быть null");
        }
        if (newStudent == null)
        {
            throw new ArgumentNullException(nameof(newStudent), "Новый студент не может быть null");
        }
        var index = students.IndexOf(oldStudent);
        if (index < 0)
        {
            throw new ArgumentException("Студент не найден в группе", nameof(oldStudent));
        }
        students[index] = newStudent;
    }

    public void TransferStudent(Student student, Group newGroup)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student), "Студент не может быть null");
        }
        if (newGroup == null)
        {
            throw new ArgumentNullException(nameof(newGroup), "Новая группа не может быть null");
        }
        if (!students.Contains(student))
        {
            throw new ArgumentException("Студент не найден в группе", nameof(student));
        }
        students.Remove(student);
        newGroup.AddStudent(student);
    }

    public void ExpelAllFailedStudents()
    {
        students.RemoveAll(s => !s.PassedSession);
    }

    public void ExpelOneFailedStudent()
    {
        var failedStudents = students.Where(s => !s.PassedSession).ToList();
        if (failedStudents.Count == 0)
        {
            throw new InvalidOperationException("Нет неудачников в группе");
        }
        var leastGrades = failedStudents.Min(s => s.AverageGrade);
        var studentToExpel = failedStudents.First(s => s.AverageGrade == leastGrades);
        students.Remove(studentToExpel);
    }
}

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public bool PassedSession { get; set; }
    public Dictionary<string, int> Grades { get; set; }
    public double AverageGrade
    {
        get
        {
            if (Grades.Count == 0)
            {
                return 0.0;
            }
            var sum = Grades.Sum(g => g.Value);
            return (double)sum / Grades.Count;
        }
    }

    private List<Student> students;

    public Student()
    {
        Grades = new Dictionary<string, int>();
        students = new List<Student>();
    }

    public Student(string firstName, string lastName, int age) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public void AddGrade(string subject, int grade)
    {
        Grades[subject] = grade;
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName}, возраст: {Age}, средний балл: {AverageGrade:0.00}";
    }

    public void AddStudent(Student student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student), "Переданный объект студента не может быть null.");
        }

        if (string.IsNullOrEmpty(student.FirstName))
        {
            throw new ArgumentException("Имя студента не может быть пустым.");
        }

        students.Add(student);
    }
}