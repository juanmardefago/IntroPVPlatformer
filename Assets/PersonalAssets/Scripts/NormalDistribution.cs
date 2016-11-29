using UnityEngine;
using System.Collections;
using System;

public class NormalDistribution {

    public static int CalculateNormalDistRandom(int mean, int deviation)
    {
        double u, v, S;

        do
        {
            u = 2.0 * UnityEngine.Random.value - 1.0;
            v = 2.0 * UnityEngine.Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        double fac = Math.Sqrt(-2.0 * Math.Log(S) / S);
        return (int)((u * fac) * deviation + mean);
    }

    public static double CalculateNormalDistRandomDouble(int mean, int deviation)
    {
        double u, v, S;

        do
        {
            u = 2.0 * UnityEngine.Random.value - 1.0;
            v = 2.0 * UnityEngine.Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        double fac = Math.Sqrt(-2.0 * Math.Log(S) / S);
        return ((u * fac) * deviation + mean);
    }
}
