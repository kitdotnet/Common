using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Common.PersonBuilder
{
    /// <summary>
    /// Represents a random person generator.
    /// </summary>
    public partial class PersonBuilder
    {
        protected readonly static HashSet<string> maleNames = new HashSet<string>();
        protected readonly static HashSet<string> femaleNames = new HashSet<string>();
        protected readonly static HashSet<string> surnames = new HashSet<string>();

        protected readonly static int maleNameCount = 0;
        protected readonly static int femaleNameCount = 0;
        protected readonly static int surnameCount = 0;

        protected readonly static Random random = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Static constructor to load up embedded resources.
        /// </summary>
        static PersonBuilder()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream maleNamesFile = assembly.GetManifestResourceStream("Common.PersonBuilder.Resources.male-names.txt");
            Stream femaleNamesFile = assembly.GetManifestResourceStream("Common.PersonBuilder.Resources.female-names.txt");
            Stream surnamesFile = assembly.GetManifestResourceStream("Common.PersonBuilder.Resources.surnames.txt");

            byte[] buffer = new byte[maleNamesFile.Length];
            maleNamesFile.ReadAsync(buffer, 0, buffer.Length);
            string fileContent = Encoding.UTF8.GetString(buffer).Replace(Environment.NewLine, "|");

            string[] names = fileContent.Split('|');

            foreach (string name in names)
            {
                maleNames.Add(name);
            }

            buffer = new byte[femaleNamesFile.Length];
            femaleNamesFile.ReadAsync(buffer, 0, buffer.Length);
            fileContent = Encoding.UTF8.GetString(buffer).Replace(Environment.NewLine, "|");

            names = fileContent.Split('|');

            foreach (string name in names)
            {
                femaleNames.Add(name);
            }

            buffer = new byte[surnamesFile.Length];
            surnamesFile.ReadAsync(buffer, 0, buffer.Length);
            fileContent = Encoding.UTF8.GetString(buffer).Replace(Environment.NewLine, "|");

            names = fileContent.Split('|');

            foreach (string name in names)
            {
                surnames.Add(name);
            }

            maleNameCount = maleNames.Count;
            femaleNameCount = femaleNames.Count;
            surnameCount = surnames.Count;
        }

        /// <summary>
        /// Construct the person object given identified criteria.
        /// </summary>
        /// <returns>An instance of <see cref="Person"/>.</returns>
        public Person Build()
        {
            return person;
        }
    }
}
