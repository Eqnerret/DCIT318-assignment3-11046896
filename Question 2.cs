using System;
using System.Collections.Generic;
using System.Linq;

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item) => items.Add(item);

    public List<T> GetAll() => new List<T>(items);

    public T? GetById(Func<T, bool> predicate) =>
        items.FirstOrDefault(predicate);

    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
        return false;
    }
}

public class Patient
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id;
        Name = name;
        Age = age;
        Gender = gender;
    }
}

public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; }
    public DateTime DateIssued { get; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
        Id = id;
        PatientId = patientId;
        MedicationName = medicationName;
        DateIssued = dateIssued;
    }
}

public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "John Mensah", 30, "Male"));
        _patientRepo.Add(new Patient(2, "Ama Serwaa", 25, "Female"));
        _patientRepo.Add(new Patient(3, "Kwesi Ofori", 40, "Male"));

        _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now.AddDays(-5)));
        _prescriptionRepo.Add(new Prescription(2, 1, "Amoxicillin", DateTime.Now.AddDays(-3)));
        _prescriptionRepo.Add(new Prescription(3, 2, "Ibuprofen", DateTime.Now.AddDays(-10)));
        _prescriptionRepo.Add(new Prescription(4, 3, "Vitamin C", DateTime.Now.AddDays(-7)));
        _prescriptionRepo.Add(new Prescription(5, 2, "Cough Syrup", DateTime.Now.AddDays(-1)));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap.Clear();
        foreach (var prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] = new List<Prescription>();
            }
            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("=== All Patients ===");
        foreach (var patient in _patientRepo.GetAll())
        {
            Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
        }
        Console.WriteLine();
    }
    public void PrintPrescriptionsForPatient(int patientId)
    {
        if (_prescriptionMap.ContainsKey(patientId))
        {
            Console.WriteLine($"=== Prescriptions for Patient ID {patientId} ===");
            foreach (var prescription in _prescriptionMap[patientId])
            {
                Console.WriteLine($"ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date: {prescription.DateIssued:d}");
            }
        }
        else
        {
            Console.WriteLine("No prescriptions found for this patient.");
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main()
    {
        var app = new HealthSystemApp();

        app.SeedData();             
        app.BuildPrescriptionMap(); 
        app.PrintAllPatients();     

        Console.Write("Enter Patient ID to view prescriptions: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            app.PrintPrescriptionsForPatient(id);
        }
        else
        {
            Console.WriteLine("Invalid ID entered.");
        }
    }
}
