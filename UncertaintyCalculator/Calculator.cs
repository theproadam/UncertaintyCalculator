using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UncertaintyCalculator
{
    public static class UncertaintyCalculator
    {
        public static double EPSILON = 1E-5;

        public static double ComputeUncertainties(Type target, out double bestEst, out double maxError)
        {
            MemberInfo[] t = target.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (t.Length % 2 != 0)
                throw new Exception("error!");

            if (target.GetMethod("Compute") == null)
                throw new Exception("Compute Method Not Found!");

            dynamic dynamicInstance = Activator.CreateInstance(target);
          
            bestEst = 0;
            maxError = 0;

            for (int i = 0; i < t.Length / 2; i++)
            {
                double lwr = dynamicInstance.Compute();

                Increment(i * 2, dynamicInstance, t);
                double upr = dynamicInstance.Compute();
                Deincrement(i * 2, dynamicInstance, t);

                double uncert = Read(i * 2 + 1, dynamicInstance, t) * (lwr - upr) / EPSILON;

                bestEst += uncert * uncert;
                maxError += Math.Abs(uncert);
            }

            bestEst = Math.Sqrt(bestEst);

            return dynamicInstance.Compute();
        }

        static void Increment(int index, dynamic d, MemberInfo[] t)
        {
            FieldInfo prop = d.GetType().GetField(t[index].Name, BindingFlags.NonPublic | BindingFlags.Instance);
            double v = prop.GetValue(d);
            v += EPSILON;
            prop.SetValue(d, v);

        }

        static double Read(int index, dynamic d, MemberInfo[] t)
        {
            FieldInfo prop = d.GetType().GetField(t[index].Name, BindingFlags.NonPublic | BindingFlags.Instance);
            return (double)prop.GetValue(d);
        }

        static void Deincrement(int index, dynamic d, MemberInfo[] t)
        {
            FieldInfo prop = d.GetType().GetField(t[index].Name, BindingFlags.NonPublic | BindingFlags.Instance);         
            double v = prop.GetValue(d);
            v -= EPSILON;
            prop.SetValue(d, v);
        }
    }
}
