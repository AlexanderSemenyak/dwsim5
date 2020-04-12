using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System
{
    using System.Runtime.CompilerServices;
    public static class NumericExtensionMethods
    {
        /// <summary>
        /// Только цифры в строке
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigitsOnly(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;

            int y = 0;
            foreach (byte code in value)
            {
                if (code < 48/*0*/ || code > 57/*9*/) return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Упрощенный но более скоростной аналог VB TextFieldReader 
    /// </summary>
    public class ONITTextFieldReader : IDisposable, IEnumerable<string[]>
    {
        private readonly char[] delimiters;
        public static char[] tabDelimiter = new[] {'\t'};
        public static char[] commaDelimiter = new[] {','};
        public static char[] dotCommaDelimiter = new[] {';'};
        public static char[] spaceDelimiter = new[] {' '};
        public static string[] emptyVallues = new string[] {};
        private StreamReader reader;

        public struct ONITTextFieldRow
        {
            public string[] arrayOfValues;

            public ONITTextFieldRow(string[] arrayOfValues)
            {
                this.arrayOfValues = arrayOfValues;
            }

            public ONITTextFieldRow(string row, char[] delimiters)
            {
                this.arrayOfValues = string.IsNullOrEmpty(row) ? emptyVallues : row.Split(delimiters);
            }
            public int Count => this.arrayOfValues.Length;

            /// <summary>
            /// Безопасно читает поля, даже если их больше, чем физически распарсилось
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public string this[int index]
            {
                get
                {
                    var loc = arrayOfValues;
                    if (index >= loc.Length) return null;

                    return loc[index];
                }
            }
        }

        public ONITTextFieldReader(Stream stream, char[] delimiters)
        {
            if (delimiters == null || delimiters.Length==0) throw new ArgumentNullException(nameof(delimiters));
            this.delimiters = delimiters;
            this.reader = new StreamReader(stream);
        }

        public bool EndOfData
        {
            get
            {
                return this.reader.EndOfStream;
            }
        }

        public IEnumerator<ONITTextFieldRow> GetTypedEnumerator()
        {
            var r = this.reader;
            var d = this.delimiters;
            while (!EndOfData)
            {
                var row = r.ReadLine();
                yield return new ONITTextFieldRow(row, d);
            }
        }
        public IEnumerator<string[]> GetEnumerator()
        {
            var r = this.reader;
            var d = this.delimiters;
            while (!EndOfData)
            {
                var row = r.ReadLine();
                yield return ParseRow(row,d);
            }
        }

        /// <summary>
        /// Парсим строку
        /// </summary>
        /// <param name="row"></param>
        /// <param name="delimiters"></param>
        /// <returns></returns>
        private static string[] ParseRow(string row, char[] delimiters)
        {
            if (row == null)
            {
                return emptyVallues;
            }

            return FixSpaces(row.Split(delimiters));
        }

        /// <summary>
        /// Фиксим пробелы в конце
        /// </summary>
        /// <param name="forSplit"></param>
        /// <returns></returns>
        private static string[] FixSpaces(string[] forSplit)
        {
            for (var i = 0; i < forSplit.Length; i++)
            {
                var item = forSplit[i];
                if (string.IsNullOrEmpty(item)) continue;
                if (string.Equals(item[item.Length-1],' '))
                {
                    forSplit[i] = item.TrimEnd();
                }
            }

            return forSplit;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            this.reader.Dispose();
        }

        /// <summary>
        /// Читаем строку
        /// </summary>
        /// <returns></returns>
        public string[] ReadFields()
        {
            var row = this.reader.ReadLine();
            return ParseRow(row, this.delimiters);
        }
    }
    
}
