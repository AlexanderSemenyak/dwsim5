#if DEBUG
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;

namespace DotNumerics.Tests
{
    [TestFixture]
    public class ONITTextFileReader_Test
    {
        [Test]
        public void TestDotCommaDelimitedRead_modfac_txt()
        {
            var fName = @"d:\developer\dwsim_AlexanderSemenyak\DWSIM.Thermodynamics\Assets\modfac.txt";
            TestTemplate(fName, ONITTextFieldReader.dotCommaDelimiter );
        }

        [Test]
        public void TestDotCommaDelimitedRead_modfac_ip()
        {
            var fName = @"d:\developer\dwsim_AlexanderSemenyak\DWSIM.Thermodynamics\Assets\modfac_ip.txt";
            TestTemplate(fName, ONITTextFieldReader.spaceDelimiter);
        }

        [Test]
        public void TestDotCommaDelimitedRead_unifac_txt()
        {
            var fName = @"d:\developer\dwsim_AlexanderSemenyak\DWSIM.Thermodynamics\Assets\unifac.txt";
            TestTemplate(fName, ONITTextFieldReader.commaDelimiter );
        }

        [Test]
        public void TestDotCommaDelimitedRead_unifac_ip()
        {
            var fName = @"d:\developer\dwsim_AlexanderSemenyak\DWSIM.Thermodynamics\Assets\unifac_ip.txt";
            TestTemplate(fName, ONITTextFieldReader.tabDelimiter);
        }


        private void TestTemplate(string fName, char[] delimiters)
        {
            var stream = File.OpenRead(fName);
            var reader = new ONITTextFieldReader(stream, delimiters);

            var stream2 = File.OpenRead(fName);
            var reader2 = new TextFieldParser(stream2);
            reader2.Delimiters = delimiters.Select(x=>x.ToString()).ToArray();

            int countRowsParsed = 0;
            //сравниваем значения
            while (!reader2.EndOfData)
            {
                countRowsParsed++;
                var fileds = reader.ReadFields();
                var fileds2 = reader2.ReadFields();

                Assert.IsTrue(fileds.Length == fileds2.Length);
                for (var i = 0; i < fileds.Length; i++)
                {
                    var filed = fileds[i];
                    var filed2 = fileds2[i];

                    if (!Equals(filed, filed2))
                    {
                        Debugger.Break();
                    }

                    Assert.AreEqual(filed, filed2);
                }
            }

            //одновременно закончились массивы
            Assert.IsTrue(reader.EndOfData);
            Assert.IsTrue(reader2.EndOfData);

            Console.WriteLine("Parsed rows: "+countRowsParsed);
        }
    }
}
#endif