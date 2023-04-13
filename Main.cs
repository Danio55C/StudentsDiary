using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public bool IsMaxiMize
        {
            get
            {
                return Settings.Default.IsMaxiMazie;
            }
            set
            {
                Settings.Default.IsMaxiMazie = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            SetColumnsHeader();

            if (IsMaxiMize)
                WindowState = FormWindowState.Maximized;

            CreateStudentFilter();
            dgvDiary.ScrollBars = ScrollBars.Both;
            
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;
        }
        private void SetColumnsHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "jęz. polski";
            dgvDiary.Columns[8].HeaderText = "jęz. angielski";
            dgvDiary.Columns[9].HeaderText = "zajęcia dodatkowe";
            dgvDiary.Columns[10].HeaderText = "grupa ucznia";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            //addEditStudent.CreateStudentsGroups();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować ");
                return;
            }

            var addEditStudent = new AddEditStudent(Convert.ToInt32
                (dgvDiary.SelectedRows[0].Cells[0].Value));
            //addEditStudent.CreateStudentsGroups();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego chcesz usunąć ");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];
            var confirmDelete =
                MessageBox.Show($"Czy napewno chcesz usunąć ucznia {selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString().Trim()}",
                "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }
        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                IsMaxiMize = true;
            else
                IsMaxiMize = false;

            Settings.Default.Save();
        }

        private void CreateStudentFilter()
        {
            AddEditStudent addEditStudent = new AddEditStudent();
            string[] studentsGroups = addEditStudent.AllStudentGroups;
            Array.Resize(ref studentsGroups, studentsGroups.Length + 1);
            var all = "Wszyscy";
            studentsGroups[studentsGroups.Length - 1] = all;

            cbStudentGroupFilter.Items.AddRange(studentsGroups);
            cbStudentGroupFilter.Text = all;
        }
        private void cbStudentGroupFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            var filteredStudents = (from x in students
                                    where x.StudentGroup == GetSelectedStudentGroup(cbStudentGroupFilter)
                                    select x).ToList();

            dgvDiary.DataSource = filteredStudents;
            if (GetSelectedStudentGroup(cbStudentGroupFilter) == "Wszyscy")
                RefreshDiary();
        }
         public static string GetSelectedStudentGroup(ComboBox cb)
        {
            return cb.SelectedItem.ToString();
        }

    }
}



















        
























