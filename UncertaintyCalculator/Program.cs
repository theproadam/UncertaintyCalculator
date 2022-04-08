using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UncertaintyCalculator
{
    class MyEquation
    {
        double v1 = 3.42, v1_ = 0.00244140625;
        double v2 = 0.64, v2_ = 0.00244140625;

        double v3 = 3.47, v3_ = 0.00244140625;
        double v4 = 0.65, v4_ = 0.00244140625;

        double v5 = 3.43, v5_ = 0.00244140625;
        double v6 = 0.64, v6_ = 0.00244140625;


        double t1 = 21.1, t1_ = 0.3;
        double t2 = 100, t2_ = 5;

        double thickness = 0.71, ut = 0.05;
        double L = 8.51, uL = 0.05;
        double W = 50.58, uW = 0.05;
        double B = 99.19, uB = 0.05;

        double h = 10, uh = 3;
        double k = 237, uk = 10;

        public double Compute()
        {
            double tBase = ReComputeT2(80.86);
            double tSur = ReComputeT1(21.1);
            double tFinAvrg = ReComputeT3(43.47);

            double P = 2 * (W + thickness);
            double Ac = thickness * W;
            double Le = L + Ac / P;
            
            double m = Math.Sqrt(h * (P / 1000) / (k * Ac / (1000 * 1000)));
            double M = Math.Sqrt(h * (P / 1000) * (Ac / (1000 * 1000)) * (tBase - tSur));

            double singleFinHeatTransfer = M * (cosh(m * Le / 1000) - (tFinAvrg - tSur) / (tBase - tSur)) / sinh(m * Le / 1000);

         //   double finEffectiveness = singleFinHeatTransfer / (h * (tBase - tSur) * Ac / (1000 * 1000));

            return singleFinHeatTransfer;
        }

        double ReComputeT1(double T)
        {
            double voltage = (T - 118.1640288) / -28.38129496;

            double slope = (t2 - t1) / (v2 - v1);
            double b = -slope * v1 + t1;
            return slope * voltage + b;
        }

        double ReComputeT2(double T)
        {
            double voltage = (T - 118.1861702) / -27.9787234;

            double slope = (t2 - t1) / (v4 - v3);
            double b = -slope * v3 + t1;
            return slope * voltage + b;
        }

        double ReComputeT3(double T)
        {
            double voltage = (T - 118.0989247) / -28.27956989;

            double slope = (t2 - t1) / (v6 - v5);
            double b = -slope * v5 + t1;
            return slope * voltage + b;
        }
        
        double cosh(double x)
        {
            return 0.5 * (Math.Exp(x) + Math.Exp(-x));
        }

        double sinh(double x)
        {
            return 0.5 * (Math.Exp(x) - Math.Exp(-x));
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            double maxerror;
            double bestest;

            double result = UncertaintyCalculator.ComputeUncertainties(typeof(MyEquation), out bestest, out maxerror);

            Console.WriteLine("Best-Estimate: " + result + " ± " + bestest);
            Console.WriteLine("Max Error: " + result + " ± " + maxerror);

            Console.ReadLine();
           
        }
    }
}
