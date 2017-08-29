namespace KeesTalksTech.Utilities.Latin.Numerals
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class RomanNumeral
    {
        #region Constants

        //0
        public const string NULLA = "NULLA";

        //values - a readonly dictionary where the numerals are the keys to values
        public static readonly IReadOnlyDictionary<string, int> VALUES = new ReadOnlyDictionary<string, int>(new Dictionary<string, int>
        {
            {"I",       1 },
            {"IV",      4 },
            {"V",       5 },
            {"IX",      9 },
            {"X",       10 },
            {"XIIX",    18 },
            {"IIXX",    18 },
            {"XL",      40 },
            {"L",       50 },
            {"XC",      90 },
            {"C",       100 },
            {"CD",      400 },
            {"D",       500 },
            {"CM",      900 },
            {"M",       1000 },

            //alternatives from Middle Ages and Renaissance
            {"O",       11 },
            {"F",       40 },
            {"P",       400 },
            {"G",       400 },
            {"Q",       500 }
        });

        //all the options that are used for parsing, in their order of value
        public static readonly string[] NUMERAL_OPTIONS =
        {
            "M", "CM", "D", "Q", "CD", "P", "G", "C", "XC", "L", "F", "XL", "IIXX", "XIIX", "O", "X", "IX", "V", "IV", "I"
        };

        //subtractive notation uses these numerals
        public static readonly string[] SUBTRACTIVE_NOTATION =
        {
            "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"
        };

        //the addative notation uses these numerals
        public static readonly string[] ADDITIVE_NOTATION =
        {
            "M", "D", "C", "L", "X", "V", "I"
        };

        #endregion

        private readonly int _number;

        public int Number => _number;

        public RomanNumeral(int number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Number should be positive.");
            }

            _number = number;
        }

        public override string ToString()
        {
            return ToString(RomanNumeralNotation.Substractive);
        }

        public string ToString(RomanNumeralNotation notation)
        {
            if (Number == 0)
            {
                return NULLA;
            }

            //check notation for right set of characters
            string[] numerals;
            switch (notation)
            {
                case RomanNumeralNotation.Additive:
                    numerals = ADDITIVE_NOTATION;
                    break;
                default:
                    numerals = SUBTRACTIVE_NOTATION;
                    break;
            }

            var resultRomanNumeral = "";

            //start with the M and iterate back
            var position = 0;

            //substract till the number is 0
            var value = Number;

            do
            {
                var numeral = numerals[position];
                var numeralValue = VALUES[numeral];

                //check if the value is in the number
                if (value >= numeralValue)
                {
                    //substract from the value
                    value -= numeralValue;

                    //add the numeral to the string
                    resultRomanNumeral += numeral;

                    //multiple numeral? advance position because things like 'IVIV' does not exist
                    bool isMultipleNumeral = numeral.Length > 1;
                    if(isMultipleNumeral)
                    {
                        position++;
                    }

                    continue;
                }

                position++;
            }
            while (value != 0);


            return resultRomanNumeral;
        }

        private static bool IsNumeral(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return false;
            }

            return Parse(str) != null;
        }

        public static RomanNumeral Parse(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return new RomanNumeral(0);
            }

            //upper case the string
            var strToRead = str.ToUpper();

            //nulla? means nothing 0 wasn't invented yet ;-)
            if (strToRead == NULLA)
            {
                return new RomanNumeral(0);
            }

            //if ends in J -> replace it to I (used in medicine)
            if (strToRead.EndsWith("J"))
            {
                strToRead = strToRead.Substring(0, strToRead.Length - 1) + "I";
            }

            //if a U is present, assume a V
            strToRead = strToRead.Replace("U", "V");

            //check simple numbers directly in dictionary
            if (VALUES.ContainsKey(str))
            {
                return new RomanNumeral(VALUES[str]);
            }

            var resultNumber = 0;

            //start with M and iterate through the options
            var numeralOptionPointer = 0;

            //continue to read until the string is empty or the numeral options pointer has exceeded all options
            while (!String.IsNullOrEmpty(strToRead) && numeralOptionPointer < NUMERAL_OPTIONS.Length)
            {
                //select the current numeral
                var numeral = NUMERAL_OPTIONS[numeralOptionPointer];

                //read numeral -> check if the numeral is used, otherwise move on to the next one
                if (!strToRead.StartsWith(numeral))
                {
                    numeralOptionPointer++;
                    continue;
                }

                //add the vaue of the found numeral
                var value = VALUES[numeral];
                resultNumber += value;

                //remove the letters of the numeral from the string
                strToRead = strToRead.Substring(numeral.Length);


                //short hand like IX? -> move on to the next numeral option
                if (numeral.Length > 1)
                {
                    numeralOptionPointer++;
                }
            }

            //whole string is read, return the numeral
            if (String.IsNullOrEmpty(strToRead))
            {
                return new RomanNumeral(resultNumber);
            }

            //string is invalid
            return null;
        }

        public static int operator +(int r1, RomanNumeral r2)
        {
            var r = new RomanNumeral(r1) + r2;
            return r.Number;
        }

        public static string operator +(string r1, RomanNumeral r2)
        {
            var r = RomanNumeral.Parse(r1) + r2;
            return r.ToString();
        }

        public static RomanNumeral operator +(RomanNumeral r1, string r2)
        {
            return r1 + RomanNumeral.Parse(r2);
        }

        public static RomanNumeral operator +(RomanNumeral r1, int r2)
        {
            var n = r1.Number + r2;
            return new RomanNumeral(n);
        }

        public static RomanNumeral operator +(RomanNumeral r1, RomanNumeral r2)
        {
            var n = r1.Number + r2.Number;
            return new RomanNumeral(n);
        }


        public static int operator -(int r1, RomanNumeral r2)
        {
            var r = new RomanNumeral(r1) - r2;
            return r.Number;
        }

        public static string operator -(string r1, RomanNumeral r2)
        {
            var r = RomanNumeral.Parse(r1) + r2;
            return r.ToString();
        }

        public static RomanNumeral operator -(RomanNumeral r1, RomanNumeral r2)
        {
            var n = r1.Number - r2.Number;

            if (n < 0)
            {
                n = 0;
            }

            return new RomanNumeral(n);
        }

        public static implicit operator int(RomanNumeral r)
        {
            return (r?.Number).GetValueOrDefault();
        }

        public static implicit operator string(RomanNumeral r)
        {
            return r?.ToString();
        }

        public static implicit operator RomanNumeral(int r)
        {
            return new RomanNumeral(r);
        }

        public static implicit operator RomanNumeral(string r)
        {
            return Parse(r);
        }
    }
}