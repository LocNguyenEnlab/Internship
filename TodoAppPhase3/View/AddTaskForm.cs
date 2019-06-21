using System;
using System.Windows.Forms;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3
{
    public partial class AddTaskForm : Form
    {
        private BLLTask _bllTask;
        private Task taskEdit;
        public delegate void PassData(Task task);
        public delegate void ShowMainForm();
        public PassData passData;
        public ShowMainForm showMainForm;

        public AddTaskForm()
        {
            InitializeComponent();
            btnUpdate.Visible = false;
            _bllTask = new BLLTask();
        }

        public AddTaskForm(Task task)
        {
            InitializeComponent();
            tbTitle.Text = task.Title;
            tbDescription.Text = task.Description;
            btnOk.Visible = false;
            taskEdit = task;
            _bllTask = new BLLTask();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var task = new Task
            {
                Id = -1,
                Title = tbTitle.Text,
                Description = tbDescription.Text,
                TimeCreate = DateTime.Now,
                TypeList = (int)TypeList.Todo
            };

            if (!_bllTask.IsEmpty(task))
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

            if (!_bllTask.IsEmpty(taskEdit))
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
