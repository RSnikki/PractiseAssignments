using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ques3
{
    class Program
    {
        public double CalculateCourseFee(double courseFee, double marks, double serviceFee)
        {
            double feeToPay = (courseFee - (marks / 200) * courseFee ) + serviceFee;
            return feeToPay;
        }
        static void Main(string[] args)
        {
            double actualCourseFee, marks, serviceFee;

            Console.Write("Enter Course Fee: ");
            actualCourseFee = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Marks obtained: ");
            marks = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Service Fee charged: ");
            serviceFee = Convert.ToDouble(Console.ReadLine());

            Program p1 = new Program();

            Console.WriteLine("Fee to be paid: {0}", p1.CalculateCourseFee(actualCourseFee, marks, serviceFee)) ;

            Console.ReadLine();       
        }
    }
}
