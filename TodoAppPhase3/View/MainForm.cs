﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TodoAppPhase3.BLL;

namespace TodoAppPhase3
{
    public partial class MainForm : Form
    {
        private int _pointYTodo;
        private int _pointYDoing;
        private int _pointYDone;
        private List<Task> _listTask;
        private List<TextBox> _listTbTodo;
        private List<TextBox> _listTbDoing;
        private List<TextBox> _listTbDone;
        private BLLTask _bll;

        public MainForm()
        {
            InitializeComponent();
            _pointYTodo = 0;
            _pointYDoing = 0;
            _pointYDone = 0;
            _listTask = new List<Task>();
            _listTbTodo = new List<TextBox>();
            _listTbDoing = new List<TextBox>();
            _listTbDone = new List<TextBox>();
            _bll = new BLLTask();
            ShowData();
        }

        private void ShowData()
        {
            _listTask = _bll.GetAllTask();
            foreach (var item in _listTask)
            {
                CreateNewTextBox(item);
            }
        }

        private void CreateNewTextBox(Task t)
        {
            var tb = new TextBox();

            if (t.TypeList == 0)
            {
                tb.Location = new Point(0, _pointYTodo);
                tb.Text = t.Title;
                tb.Name = "tb" + (t.Id);
                tb.ReadOnly = true;
                tb.Size = new Size(325, 20);
                _listTbTodo.Add(tb);
                tb.MouseDown += TextBox_MouseDown;
                tb.Show();
                lvToDo.Controls.Add(tb);
                _pointYTodo += 25;
            }
            else if (t.TypeList == 1)
            {
                tb.Location = new Point(0, _pointYDoing);
                tb.Text = t.Title;
                tb.Name = "tb" + (t.Id);
                tb.ReadOnly = true;
                tb.Size = new Size(325, 20);
                _listTbDoing.Add(tb);
                tb.MouseDown += TextBox_MouseDown;
                tb.Show();
                lvDoing.Controls.Add(tb);
                _pointYDoing += 25;
            }
            else if (t.TypeList == 2)
            {
                tb.Location = new Point(0, _pointYDone);
                tb.Text = t.Title;
                tb.Name = "tb" + (t.Id);
                tb.ReadOnly = true;
                tb.Size = new Size(325, 20);
                _listTbDone.Add(tb);
                tb.MouseDown += TextBox_MouseDown;
                tb.Show();
                lvDone.Controls.Add(tb);
                _pointYDone += 25;
            }
        }

        private bool IsInListTextbox(string nameTb, List<TextBox> list)
        {
            foreach (var item in list)
            {
                if (item.Name == nameTb)
                {
                    return true;
                }
            }
            return false;
        }

        private TextBox FindTextbox(string name, List<TextBox> list)
        {
            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        private int FindIndexTbInList(List<TextBox> list, TextBox tb)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == tb)
                {
                    return i;
                }
            }
            return -1;
        }

        private void RemoveTextBoxFromListView(TextBox tb, ListView lv, List<TextBox> list, ref int pointY)
        {
            int indexTb = FindIndexTbInList(list, tb);

            if (indexTb == list.Count)
            {
                pointY -= 25;
            }
            else
            {
                for (int i = indexTb + 1; i < list.Count; i++)
                {
                    list[i].Location = new Point(0, list[i].Location.Y - 25);
                }
                pointY -= 25;
            }

            lv.Controls.Remove(tb);
            list.Remove(tb);
        }

        private void EditTask(Task t)
        {
            var addTaskForm = new AddTaskForm(t)
            {
                Text = "Edit task",
                passData = new AddTaskForm.PassData(PassData),
                showMainForm = new AddTaskForm.ShowMainForm(Show)
            };
            addTaskForm.Show();
            this.Hide();
        }

        private void DeleteTask(int idTask)
        {
            if (IsInListTextbox("tb" + idTask, _listTbTodo))
            {
                var tb = FindTextbox("tb" + idTask, _listTbTodo);
                RemoveTextBoxFromListView(tb, lvToDo, _listTbTodo, ref _pointYTodo);
                _bll.DeleteTask(idTask);
            }
            else if (IsInListTextbox("tb" + idTask, _listTbDoing))
            {
                var tb = FindTextbox("tb" + idTask, _listTbDoing);
                RemoveTextBoxFromListView(tb, lvDoing, _listTbDoing, ref _pointYDoing);
                _bll.DeleteTask(idTask);
            }
            else if (IsInListTextbox("tb" + idTask, _listTbDone))
            {
                var tb = FindTextbox("tb" + idTask, _listTbDone);
                RemoveTextBoxFromListView(tb, lvDone, _listTbDone, ref _pointYDone);
                _bll.DeleteTask(idTask);
            }
        }

        private void ShowTaskDetail(Task t)
        {
            MessageBoxManager.Yes = "Edit";
            MessageBoxManager.No = "Delete";
            MessageBoxManager.Cancel = "Close";
            MessageBoxManager.Register();

            DialogResult dialogResult = MessageBox.Show(string.Format("Title: {0}\nDescription: {1}\nTime create: {2}",
                t.Title, t.Description, Convert.ToDateTime(t.TimeCreate)),
                "Task Detail", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();

            if (dialogResult == DialogResult.Yes)
            {
                EditTask(t);
            }
            else if (dialogResult == DialogResult.No)
            {
                DialogResult resultConfirm = MessageBox.Show("Do you want to delete this task?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultConfirm == DialogResult.Yes)
                {
                    DeleteTask(t.Id);
                }
            }
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            var addTaskForm = new AddTaskForm()
            {
                passData = new AddTaskForm.PassData(PassData),
                showMainForm = new AddTaskForm.ShowMainForm(Show)
            };
            addTaskForm.Show();
            this.Hide();
        }

        public void PassData(Task t)
        {
            if (t.Id == -1) //add new task
            {
                if (!_bll.IsDuplicateTask(t))
                {
                    this.Show();
                    _bll.AddTask(t);
                    int id = _bll.GetMaxId();
                    t = _bll.GetTaskById(id);
                    CreateNewTextBox(t);
                }
                else
                {
                    MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show();
                }
            }
            else //update an exists task
            {
                _bll.UpdateTask(t);
                UpdateTextBox(t);
                this.Show();
            }
        }

        public void ShowForm1()
        {
            this.Show();
        }

        public void UpdateTextBox(Task t)
        {
            if (FindTextbox("tb" + t.Id, _listTbTodo) != null)
            {
                var tb = FindTextbox("tb" + t.Id, _listTbTodo);
                _listTbTodo[FindIndexTbInList(_listTbTodo, tb)].Text = t.Title;
            }
            else if (FindTextbox("tb" + t.Id, _listTbDoing) != null)
            {
                var tb = FindTextbox("tb" + t.Id, _listTbDoing);
                _listTbDoing[FindIndexTbInList(_listTbDoing, tb)].Text = t.Title;
            }
            else if (FindTextbox("tb" + t.Id, _listTbDone) != null)
            {
                var tb = FindTextbox("tb" + t.Id, _listTbDone);
                _listTbDone[FindIndexTbInList(_listTbDone, tb)].Text = t.Title;
            }
        }

        private void LvDoing_DragEnter(object sender, DragEventArgs e)
        {
            int idTb = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsInListTextbox("tb" + idTb, _listTbDoing))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDone_DragEnter(object sender, DragEventArgs e)
        {
            int idTb = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsInListTextbox("tb" + idTb, _listTbDone))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvTodo_DragEnter(object sender, DragEventArgs e)
        {
            int idTb = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsInListTextbox("tb" + idTb, _listTbTodo))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDoing_DragDrop(object sender, DragEventArgs e)
        {
            var t = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            t.TypeList = 1;
            _bll.UpdateTask(t);
            CreateNewTextBox(t);

            if (IsInListTextbox("tb" + t.Id, _listTbTodo))
            {
                var tb = FindTextbox("tb" + t.Id, _listTbTodo);
                RemoveTextBoxFromListView(tb, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsInListTextbox("tb" + t.Id, _listTbDone))
            {
                var tb = FindTextbox("tb" + t.Id, _listTbDone);
                RemoveTextBoxFromListView(tb, lvDone, _listTbDone, ref _pointYDone);
            }
        }

        private void LvDone_DragDrop(object sender, DragEventArgs e)
        {
            var t = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            t.TypeList = 2;
            _bll.UpdateTask(t);
            CreateNewTextBox(t);

            if (IsInListTextbox("tb" + t.Id, _listTbTodo))
            {
                var tb = FindTextbox("tb" + t.Id, _listTbTodo);
                RemoveTextBoxFromListView(tb, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsInListTextbox("tb" + t.Id, _listTbDoing))
            {
                var tb = FindTextbox("tb" + t.Id, _listTbDoing);
                RemoveTextBoxFromListView(tb, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void LvTodo_DragDrop(object sender, DragEventArgs e)
        {
            var t = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            t.TypeList = 0;
            _bll.UpdateTask(t);
            CreateNewTextBox(t);

            if (IsInListTextbox("tb" + t.Id, _listTbDone))
            {
                var tb = FindTextbox("tb" + t.Id, _listTbDone);
                RemoveTextBoxFromListView(tb, lvDone, _listTbDone, ref _pointYDone);
            }

            if (IsInListTextbox("tb" + t.Id, _listTbDoing))
            {
                var tb = FindTextbox("tb" + t.Id, _listTbDoing);
                RemoveTextBoxFromListView(tb, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            var tb = sender as TextBox;
            string nameTb = tb.Name;
            int idTb = Convert.ToInt32(nameTb.Replace("tb", ""));
            Task t = _bll.GetTaskById(idTb);

            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                tb.DoDragDrop(t, DragDropEffects.Move);
            }
            else
            {
                ShowTaskDetail(t);
            }
        }

    }
}
