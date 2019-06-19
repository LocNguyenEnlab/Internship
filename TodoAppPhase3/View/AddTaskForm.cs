using System;
using System.Windows.Forms;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3
{
    public partial class AddTaskForm : Form
    {
        private BLLTask _bll;
        private Task taskEdit;
        public delegate void PassData(Task t);
        public delegate void ShowMainForm();
        public PassData passData;
        public ShowMainForm showMainForm;

        public AddTaskForm()
        {
            InitializeComponent();
            btnUpdate.Visible = false;
            _bll = new BLLTask();
        }

        public AddTaskForm(Task t)
        {
            InitializeComponent();
            tbTitle.Text = t.Title;
            tbDescription.Text = t.Description;
            btnOk.Visible = false;
            taskEdit = t;
            _bll = new BLLTask();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var task = new Task
            {
                Id = -1,
                Title = tbTitle.Text,
                Description = tbDescription.Text,
                TimeCreate = DateTime.Now,
                TypeList = 0
            };

            if (!_bll.IsEmpty(task))
            {
                passData(task);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Please fill in full info", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormAddTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            showMainForm();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            taskEdit.Title = tbTitle.Text;
            taskEdit.Description = tbDescription.Text;

            if (!_bll.IsEmpty(taskEdit))
            {
                passData(taskEdit);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Please fill in full info", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
