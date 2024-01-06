using System;
using System.Collections;
using System.Threading;
class Program
{
    // Arraylistleri oluşturur.
    static ArrayList allNumbers = new ArrayList();
    static ArrayList primeNumbers = new ArrayList();
    static ArrayList evenNumbers = new ArrayList();
    static ArrayList oddNumbers = new ArrayList();
    // Aynı anda birden threadin listeye erişimini engeller.
    static object lockObject = new object();
    static void Main()
    {
        // 1 milyon sayıyı tüm sayılar dizisine ekler.
        for (int i = 1; i <= 1000000; i++)
        {
            allNumbers.Add(i);
        }
        // Threadleri oluşturur.
        Thread[] threads = new Thread[4];
        for (int i = 0; i < 4; i++)
        {
            int start = i * 250000;
            int end = (i + 1) * 250000;
            threads[i] = new Thread(() => ProcessNumbers(start, end));
            threads[i].Start();
        }
        //  Main metodun diğer threadleri beklemesini ve karışıklığı önlemesini sağlar. 
        foreach (Thread t in threads)
        {
            t.Join();
        }

        // Sonuçları yazdırır.
        Console.WriteLine("İşlem başarıyla gerçekleştirildi.");
        Console.WriteLine("Asal Sayı Adeti: " + primeNumbers.Count);
        Console.WriteLine("Çift Sayı Adeti: " + evenNumbers.Count);
        Console.WriteLine("Tek Sayı Adeti: " + oddNumbers.Count);
    }
    static void ProcessNumbers(int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            int number = (int)allNumbers[i];
            if (IsPrime(number))
            {
                AddToArrayList(primeNumbers, number);
                // Asal sayı çift ise çift sayılar dizisine, tek ise tek sayılar dizisine de ekler.
                if (number % 2 == 0)
                {
                    AddToArrayList(evenNumbers, number);
                }
                else
                {
                    AddToArrayList(oddNumbers, number);
                }
            }
            // Çift sayıları çift sayılar dizisine ekler.
            else if (number % 2 == 0)
            {
                AddToArrayList(evenNumbers, number);
            }
            // Tek sayıları tek sayılar dizisine ekler.
            else
            {
                AddToArrayList(oddNumbers, number);
            }
        }
    }
    // Asal sayıları bulur.
    static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        int boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i += 2)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
    static void AddToArrayList(ArrayList list, int number)
    {
        // Aynı anda birden threadin listeye erişimini engeller.
        lock (lockObject)
        {
            list.Add(number);
        }
    }
}