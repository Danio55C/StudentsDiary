using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private int _studentId;

        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        private Student _student;

        public string[] _allStudentGroups = new string[]

        {
            "1A","1B","1C","1D","1E","1F","1G","1H","1I",
            "2A","2B","2C","2D","2E","2F","2G","2H","2I",
            "3A","3B","3C","3D","3E","3F","3G","3H","3I",
        };

        public string[] AllStudentGroups
        {
            get { return _allStudentGroups; }
            set { _allStudentGroups = value; }
        }
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            GetStudentData();
            tbFirstName.Select();
            cbStudentGroup.Items.AddRange(AllStudentGroups);
        }


        /*public string[] _allStudentGroups = { "1A", "1B", "1C", "1D", "1E", "1F", "1G" };

         public void CreateStudentsGroups()
         {
             cbStudentGroup.Items.AddRange(_allStudentGroups);
         }
         */
        private void GetStudentData()
        {
            if (_studentId != 0)
            {

                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id");

                FillTextBoxes();

            }
        }

        private void FillTextBoxes()
        {
            tbID.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbPhysic.Text = _student.Physics;
            tbTech.Text = _student.Technology;
            tbEnglishLang.Text = _student.EnglishLanguage;
            tbPolishLang.Text = _student.PolishLanguage;
            rtbComments.Text = _student.Comments;
            chbExtraActivities.Checked = _student.ExtraActivities;
            cbStudentGroup.Text = _student.StudentGroup;
        }
        private async void btnConfirm_Click(object sender, EventArgs e)
        {

            var students = _fileHelper.DeserializeFromFile();
            try
            {
                if (_studentId != 0)
                    students.RemoveAll(x => x.Id == _studentId);
                else

                    AssignIdToNewStudent(students);
                AddNewUserToList(students);
            }
            catch (Exception)
            {
                MessageBox.Show("Dodałeś właśne ucznia bez grupy! Jeśli będziesz chciał zmienić edytuj.");
            }

            finally
            {
                _fileHelper.SerializeToFile(students);

                await LongProcessAsync();
                Close();
            }
        }

        private async Task LongProcessAsync()
        {
            await Task.Run(() =>
             {
                 Thread.Sleep(2000);
             });
        }
        private void AddNewUserToList(List<Student> students)
        {

            var student = new Student

            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                EnglishLanguage = tbEnglishLang.Text,
                Math = tbMath.Text,
                Physics = tbPhysic.Text,
                PolishLanguage = tbPolishLang.Text,
                Technology = tbTech.Text,
                ExtraActivities = chbExtraActivities.Checked,
                StudentGroup = cbStudentGroup.Text
            };
            students.Add(student);
            if (cbStudentGroup.Text == string.Empty)
            {
                throw new Exception("Nie przypisano grupy do ucznia!!!");
            }
        }

        private void AssignIdToNewStudent(List<Student> students)
        {

            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbStudentGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        
    }
}










































































