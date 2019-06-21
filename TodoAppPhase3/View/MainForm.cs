using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private BLLTask _bllTask;

        public MainForm()
        {
            InitializeComponent();
            _pointYTodo = 0;
            _pointYDoing = 0;
            _pointYDone = 0;
            _listTbTodo = new List<TextBox>();
            _listTbDoing = new List<TextBox>();
            _listTbDone = new List<TextBox>();
            _bllTask = new BLLTask();
            ShowData();
        }

        private void ShowData()
        {
            _listTask = _bllTask.GetAllTask();
            foreach (var task in _listTask)
            {
                CreateNewTextBox(task);
            }
        }

        private void CreateNewTextBox(Task task)
        {
            if (task.TypeList == (int)TypeList.Todo)
            {
                CreateAndImportTextbox(lvToDo, _listTbTodo, task, ref _pointYTodo);
            }
            else if (task.TypeList == (int)TypeList.Doing)
            {
                CreateAndImportTextbox(lvDoing, _listTbDoing, task, ref _pointYDoing);
            }
            else if (task.TypeList == (int)TypeList.Done)
            {
                CreateAndImportTextbox(lvDone, _listTbDone, task, ref _pointYDone);
            }
        }

        private void CreateAndImportTextbox(ListView listView, List<TextBox> listTextBox, Task task, ref int pointY)
        {
            var textBox = new TextBox();
            textBox.Location = new Point(0, pointY);
            textBox.Text = task.Title;
            textBox.Name = "tb" + (task.Id);
            textBox.ReadOnly = true;
            textBox.Size = new Size(325, 20);
            listTextBox.Add(textBox);
            textBox.MouseDown += TextBox_MouseDown;
            textBox.Show();
            listView.Controls.Add(textBox);
            pointY += 25;
        }

        private bool IsTextBoxInList(string textBoxName, List<TextBox> list)
        {
            if (list.FirstOrDefault(s => s.Name == textBoxName) != null)
                return true;
            else
                return false;
        }

        private TextBox FindTextbox(string name, List<TextBox> list)
        {
            return list.FirstOrDefault(_ => _.Name == name);
        }        

        private int FindTextBoxIndexInList(List<TextBox> list, TextBox textBox)
        {
            return list.FindIndex(_ => _.Name == textBox.Name);
        }

        private void RemoveTextBoxFromListView(TextBox textBox, ListView listView, List<TextBox> list, ref int pointY)
        {
            int textBoxIndex = FindTextBoxIndexInList(list, textBox);

            if (textBoxIndex == list.Count)
            {
                pointY -= 25;
            }
            else
            {
                for (int i = textBoxIndex + 1; i < list.Count; i++)
                {
                    list[i].Location = new Point(0, list[i].Location.Y - 25);
                }
                pointY -= 25;
            }
            
            listView.Controls.Remove(textBox);
            list.Remove(textBox);
        }

        private void EditTask(Task task)
        {
            var addTaskForm = new AddTaskForm(task)
            {
                Text = "Edit task",
                passData = new AddTaskForm.PassData(PassData),
                showMainForm = new AddTaskForm.ShowMainForm(Show)
            };
            addTaskForm.ShowDialog();
        }

        private void DeleteTask(int taskId)
        {
            if (IsTextBoxInList("tb" + taskId, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + taskId, _listTbTodo);
                RemoveTextBoxFromListView(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
                _bllTask.DeleteTask(taskId);
                _bllTask.Commit();
            }
            else if (IsTextBoxInList("tb" + taskId, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + taskId, _listTbDoing);
                RemoveTextBoxFromListView(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
                _bllTask.DeleteTask(taskId);
                _bllTask.Commit();
            }
            else if (IsTextBoxInList("tb" + taskId, _listTbDone))
            {
                var textBox = FindTextbox("tb" + taskId, _listTbDone);
                RemoveTextBoxFromListView(textBox, lvDone, _listTbDone, ref _pointYDone);
                _bllTask.DeleteTask(taskId);
                _bllTask.Commit();
            }
        }

        private void ShowTaskDetail(Task task)
        {
            MessageBoxManager.Yes = "Edit";
            MessageBoxManager.No = "Delete";
            MessageBoxManager.Cancel = "Close";
            MessageBoxManager.Register();

            DialogResult dialogResult = MessageBox.Show(string.Format("Title: {0}\nDescription: {1}\nTime create: {2}",
                task.Title, task.Description, Convert.ToDateTime(task.TimeCreate)),
                "Task Detail", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();

            if (dialogResult == DialogResult.Yes)
            {
                EditTask(task);
            }
            else if (dialogResult == DialogResult.No)
            {
                DialogResult confirmResult = MessageBox.Show("Do you want to delete this task?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteTask(task.Id);
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

        public void PassData(Task task)
        {
            if (task.Id == -1) //add new task
            {
                if (!_bllTask.IsDuplicateTask(task))
                {
                    this.Show();
                    _bllTask.AddTask(task);
                    _bllTask.Commit();
                    int id = _bllTask.GetMaxId();
                    task = _bllTask.GetTask(id);
                    CreateNewTextBox(task);
                }
                else
                {
                    MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show();
                }
            }
            else //update an exists task
            {
                _bllTask.UpdateTask(task);
                _bllTask.Commit();
                UpdateTextBox(task);
                this.Show();
            }
        }

        public void UpdateTextBox(Task task)
        {
            if (FindTextbox("tb" + task.Id, _listTbTodo) != null)
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                _listTbTodo[FindTextBoxIndexInList(_listTbTodo, textBox)].Text = task.Title;
            }
            else if (FindTextbox("tb" + task.Id, _listTbDoing) != null)
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                _listTbDoing[FindTextBoxIndexInList(_listTbDoing, textBox)].Text = task.Title;
            }
            else if (FindTextbox("tb" + task.Id, _listTbDone) != null)
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDone);
                _listTbDone[FindTextBoxIndexInList(_listTbDone, textBox)].Text = task.Title;
            }
        }

        private void LvDoing_DragEnter(object sender, DragEventArgs e)
        {
            int textBoxId = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsTextBoxInList("tb" + textBoxId, _listTbDoing))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDone_DragEnter(object sender, DragEventArgs e)
        {
            int textBoxId = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsTextBoxInList("tb" + textBoxId, _listTbDone))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvTodo_DragEnter(object sender, DragEventArgs e)
        {
            int textBoxId = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsTextBoxInList("tb" + textBoxId, _listTbTodo))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDoing_DragDrop(object sender, DragEventArgs e)
        {
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            task.TypeList = (int)TypeList.Doing;
            _bllTask.UpdateTask(task);
            _bllTask.Commit();
            CreateNewTextBox(task);

            if (IsTextBoxInList("tb" + task.Id, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                RemoveTextBoxFromListView(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsTextBoxInList("tb" + task.Id, _listTbDone))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDone);
                RemoveTextBoxFromListView(textBox, lvDone, _listTbDone, ref _pointYDone);
            }
        }

        private void LvDone_DragDrop(object sender, DragEventArgs e)
        {
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            task.TypeList = (int)TypeList.Done ;
            _bllTask.UpdateTask(task);
            _bllTask.Commit();
            CreateNewTextBox(task);

            if (IsTextBoxInList("tb" + task.Id, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                RemoveTextBoxFromListView(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsTextBoxInList("tb" + task.Id, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                RemoveTextBoxFromListView(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void LvTodo_DragDrop(object sender, DragEventArgs e)
        {
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            task.TypeList = (int)TypeList.Todo;
            _bllTask.UpdateTask(task);
            _bllTask.Commit();
            CreateNewTextBox(task);

            if (IsTextBoxInList("tb" + task.Id, _listTbDone))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDone);
                RemoveTextBoxFromListView(textBox, lvDone, _listTbDone, ref _pointYDone);
            }

            if (IsTextBoxInList("tb" + task.Id, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                RemoveTextBoxFromListView(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBox;
            string textBoxName = textBox.Name;
            int textBoxId = Convert.ToInt32(textBoxName.Replace("tb", ""));
            var task = _bllTask.GetTask(textBoxId);

            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                textBox.DoDragDrop(task, DragDropEffects.Move);
            }
            else
            {
                ShowTaskDetail(task);
            }
        }

    }
}
