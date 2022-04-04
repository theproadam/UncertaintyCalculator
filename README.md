# UncertaintyCalculator
Best-estimate and maximum error uncertainty calculator

### Usage
First declare your class, the entry point should be `Compute()`. The fields are ordered by (variable) (variable uncertainty):
```c#
class MyEquation
{
    //double [Equation Variable], [Equation Variable Uncertainty]
    double thickness = 0.71, ut = 0.05;
    double L = 8.51, uL = 0.05;
    double W = 50.58, uW = 0.05;
    double B = 99.19, uB = 0.05;

    public double Compute()
    {
        double count = 10;

        double P = 2 * (W + thickness);
        double Ac = thickness * W;
        double Le = L + Ac / P;

        double area = count * P * Le / (1000 * 1000) + ((B * W) / (1000 * 1000) - count * thickness * W / (1000 * 1000));
        return area;
    }
}
```

Next execute the calculator and show the results.
```c#
double maxerror;
double bestest;

double result = UncertaintyCalculator.ComputeUncertainties(typeof(MyEquation), out bestest, out maxerror);

Console.WriteLine("Best-Estimate: " + result + " ± " + bestest);
Console.WriteLine("Max Error: " + result + " ± " + maxerror);
```

### Notes
- Do not use any kind of compiler/user optimization as the calculator is field order sensitive.
