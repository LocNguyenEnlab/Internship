﻿using System;
using System.Windows.Forms;

namespace ToDoAppPhase2
{
    public partial class AddTaskForm : Form
    {
        private Task _taskEdit;
        public delegate void PassData(Task task);
        public delegate void ShowMainForm();
        public PassData passData;
        public ShowMainForm showMainForm;

        
        public AddTaskForm()
        {
            InitializeComponent();
            btnUpdate.Visible = false;
        }
        public AddTaskForm(Task t)
        {
            InitializeComponent();
            tbTitle.Text = t.Title;
            tbDescription.Text = t.Description;
            btnOk.Visible = false;
            _taskEdit = t;
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
            if(!task.IsEmpty())
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
            _taskEdit.Title = tbTitle.Text;
            _taskEdit.Description = tbDescription.Text;
            if (!_taskEdit.IsEmpty())
            {
                passData(_taskEdit);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Please fill in full info", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
