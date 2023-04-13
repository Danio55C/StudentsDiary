using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class Student
    {
        private string _filePath =
       Path.Combine(Environment.CurrentDirectory, "students.txt");

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public string Math { get; set; }
        public string Technology { get; set; }
        public string Physics { get; set; }
        public string PolishLanguage { get; set; }
        public string EnglishLanguage { get; set; }
        public bool ExtraActivities { get; set; }
        public string StudentGroup { get; set; }
    }
}


































