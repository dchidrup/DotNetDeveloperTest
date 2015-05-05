using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                string ipString; //Store input string in a/b x/y format
                string n1; //Store numerator for 1st value
                string d1; //Store denominator for 1st value
                string n2; //Store numerator for 2nd value
                string d2; //Store denominator for 2nd value

                long intN1, intD1, intN2, intD2; //Store converted long values

                string[] ipKeys, key1, key2; //Store splitted values
                bool isInf = false; //Indicate overflow

                Console.WriteLine("Please Enter input string in a/b<space>x/y format!\n");
                ipString = Console.ReadLine();
                ipKeys = ipString.Split(' ');
                if (ipKeys.Length != 2)
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                key1 = ipKeys[0].Split('/');
                key2 = ipKeys[1].Split('/');
                if (key1.Length != 2)
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else
                {
                    n1 = key1[0];
                    d1 = key1[1];
                }
                if (key2.Length != 2)
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else
                {
                    n2 = key2[0];
                    d2 = key2[1];
                }

                if (!Int64.TryParse(n1, out intN1))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else if (!Int64.TryParse(d1, out intD1))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else if (!Int64.TryParse(n2, out intN2))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }
                else if (!Int64.TryParse(d2, out intD2))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

                if (intD1 == 0 || intD2 == 0)
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

                Fraction f1 = new Fraction(intN1, intD1);
                Fraction f2 = new Fraction(intN2, intD2);

                try
                {
                    Fraction fSum = f1 + f2;
                    Fraction fDiff = f1 - f2;
                    Fraction fProd = f1 * f2;

                    Fraction fQuot = new Fraction();
                    if (intN2 != 0)
                    {
                        fQuot = f1 / f2;
                    }
                    else
                    {
                        isInf = true;
                    }

                    Console.WriteLine("\nResult:\n");

                    Console.WriteLine("{0} + {1} = {2}", getSimplestForm(intN1, intD1), getSimplestForm(intN2, intD2), getSimplestForm(fSum.Num, fSum.Denom));
                    Console.WriteLine("{0} - {1} = {2}", getSimplestForm(intN1, intD1), getSimplestForm(intN2, intD2), getSimplestForm(fDiff.Num, fDiff.Denom));
                    Console.WriteLine("{0} * {1} = {2}", getSimplestForm(intN1, intD1), getSimplestForm(intN2, intD2), getSimplestForm(fProd.Num, fProd.Denom));
                    if (isInf)
                    {
                        Console.WriteLine("{0} / {1} = {2}", getSimplestForm(intN1, intD1), getSimplestForm(intN2, intD2), "Inf");
                    }
                    else
                    {
                        Console.WriteLine("{0} / {1} = {2}", getSimplestForm(intN1, intD1), getSimplestForm(intN2, intD2), getSimplestForm(fQuot.Num, fQuot.Denom));
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Some error occur while calculation !");
                    continue;
                }

                Console.WriteLine("\nPress Enter to continue...\n");
                Console.ReadLine();
            }


        }

        //Used to convert Rational number to its simplest form
        #region getSimplestForm
        public static string getSimplestForm(long numerator, long denominators)
        {
            long dividend = 0;
            long quotient = 0;
            if (numerator == 0 || denominators == 0)
            {
                return "0";
            }

            quotient = numerator / denominators;
            dividend = numerator % denominators;
            if (dividend < 0)
            {
                dividend = dividend * -1;
            }
            if (dividend == 0)
            {
                if (quotient < 0)
                {
                    return "(" + quotient.ToString() + ")";
                }
                else
                {
                    return quotient.ToString();
                }

            }
            else
            {

                if (numerator < 0 || denominators < 0)
                {
                    if (quotient == 0)
                    {
                        return "(-" + dividend.ToString() + "/" + (denominators < 0 ? -denominators : denominators).ToString() + ")";
                    }
                    else
                    {
                        return "(" + quotient.ToString() + " " + dividend.ToString() + "/" + (denominators < 0 ? -denominators : denominators).ToString() + ")";
                    }

                }
                else
                {
                    if (quotient == 0)
                    {
                        return dividend.ToString() + "/" + denominators.ToString();
                    }
                    else
                    {
                        return quotient.ToString() + " " + dividend.ToString() + "/" + denominators.ToString();
                    }

                }

            }
        }
        #endregion getSimplestForm
    }

    //Used to handle all arithmetic operation for Rational values
    struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        public readonly long Num;
        public readonly long Denom;

        public Fraction(long num, long denom)
        {
            if (num == 0)
            {
                //denom = 0;
            }
            else if (denom == 0)
            {
                throw new ArgumentException("Denominator may not be zero", "denom");
            }
            else if (denom < 0)
            {
                num = -num;
                denom = -denom;
            }

            long d = GCD(num, denom);
            this.Num = num / d;
            this.Denom = denom / d;
        }

        private static long GCD(long x, long y)
        {
            return y == 0 ? (x == 0 ? 1 : x) : GCD(y, x % y);
        }

        private static long LCM(long x, long y)
        {
            return x / GCD(x, y) * y;
        }

        public Fraction Abs()
        {
            return new Fraction(Math.Abs(Num), Denom);
        }

        public Fraction Reciprocal()
        {
            return new Fraction(Denom, Num);
        }

        #region Conversion Operators

        public static implicit operator Fraction(long i)
        {
            return new Fraction(i, 1);
        }

        public static explicit operator double(Fraction f)
        {
            return f.Num == 0 ? 0 : (double)f.Num / f.Denom;
        }

        #endregion

        #region Arithmetic Operators

        public static Fraction operator -(Fraction f)
        {
            return new Fraction(f.Num, -f.Denom);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            long m = LCM(a.Denom, b.Denom);
            long na = a.Num * m / a.Denom;
            long nb = b.Num * m / b.Denom;
            return new Fraction(na + nb, m);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return a + (-b);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.Num * b.Num, a.Denom * b.Denom);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return a * b.Reciprocal();
        }

        public static Fraction operator %(Fraction a, Fraction b)
        {
            long l = a.Num * b.Denom, r = a.Denom * b.Num;
            long n = l / r;
            return new Fraction(l - n * r, a.Denom * b.Denom);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Fraction a, Fraction b)
        {
            return a.Num == b.Num && a.Denom == b.Denom;
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return a.Num != b.Num || a.Denom != b.Denom;
        }

        public static bool operator <(Fraction a, Fraction b)
        {
            return (a.Num * b.Denom) < (a.Denom * b.Num);
        }

        public static bool operator >(Fraction a, Fraction b)
        {
            return (a.Num * b.Denom) > (a.Denom * b.Num);
        }

        public static bool operator <=(Fraction a, Fraction b)
        {
            return !(a > b);
        }

        public static bool operator >=(Fraction a, Fraction b)
        {
            return !(a < b);
        }

        #endregion

        #region Object Members

        public override bool Equals(object obj)
        {
            if (obj is Fraction)
                return ((Fraction)obj) == this;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Num.GetHashCode() ^ Denom.GetHashCode();
        }

        public override string ToString()
        {
            return Num.ToString() + "/" + Denom.ToString();
        }

        #endregion

        #region IEquatable<Fraction> Members

        public bool Equals(Fraction other)
        {
            return other == this;
        }

        #endregion

        #region IComparable<Fraction> Members

        public int CompareTo(Fraction other)
        {
            return (this.Num * other.Denom).CompareTo(this.Denom * other.Num);
        }

        #endregion
    }

}
