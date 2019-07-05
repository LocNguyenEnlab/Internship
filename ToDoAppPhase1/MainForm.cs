using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ToDoAppPhase1
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
        }

        private void CreateNewTextBox(Task task, ListView listView, ref int pointY, List<TextBox> textBoxList)
        {
            var textBox = new TextBox();
            textBox.Location = new Point(0, pointY);
            textBox.Text = task.Title;
            textBox.Name = "tb" + task.Id;
            textBox.ReadOnly = true;
            textBox.Size = new Size(325, 20);
            textBox.MouseDown += TextBox_MouseDown;
            textBox.Show();
            textBoxList.Add(textBox);
            listView.Controls.Add(textBox);
            pointY += 25;
        }

        private bool IsTextBoxInList(string textBoxName, List<TextBox> textBoxList)
        {
            if (FindTextbox(textBoxName, textBoxList) != null)
                return true;
            else
                return false;
        }

        private TextBox FindTextbox(string textBoxName, List<TextBox> textBoxList)
        {
            return textBoxList.FirstOrDefault(s => s.Name == textBoxName);
        }

        private int FindTextBoxIndex(List<TextBox> textBoxList, TextBox textBox)
        {
            return textBoxList.FindIndex(s => s.Name == textBox.Name);
        }

        private int FindIndexTask(List<Task> taskList, string title)
        {
            return taskList.FindIndex(s => s.Title == title);
        }

        /// <summary>
        /// Check duplicate task
        /// </summary>
        /// <param name="taskList"></param>
        /// <param name="task"></param>
        /// <returns>true if t already exists in list, otherwise return false</returns>
        private bool IsDuplicateTask(List<Task> taskList, Task task)
        {
            foreach (var item in taskList)
            {
                if (item.Compare(task))
                    return true;
            }
            return false;
        }

        private void RemoveTextBox(TextBox textBox, ListView listView, List<TextBox> textBoxList, ref int pointY)
        {
            var textBoxIndex = FindTextBoxIndex(textBoxList, textBox);

            if (textBoxIndex == textBoxList.Count - 1)
            {
                pointY -= 25;
            }
            else
            {
                for (int i = textBoxIndex + 1; i < textBoxList.Count; i++)
                {
                    textBoxList[i].Location = new Point(0, textBoxList[i].Location.Y - 25);
                }
                pointY -= 25;
            }

            listView.Controls.Remove(textBox);
            textBoxList.Remove(textBox);
        }

        private void EditTask(Task task)
        {
            var addTaskForm = new AddTaskForm(task)
            {
                passData = new AddTaskForm.PassData(PassData),
                showMainForm = new AddTaskForm.ShowMainForm(Show),
                Text = "Edit task"
            };
            addTaskForm.Show();
            this.Hide();
        }

        private void DeleteTask(Task task)
        {
            var textBoxId = task.Id;
            if (IsTextBoxInList("tb" + textBoxId, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + textBoxId, _listTbTodo);
                RemoveTextBox(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
            }
            else if (IsTextBoxInList("tb" + textBoxId, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + textBoxId, _listTbDoing);
                RemoveTextBox(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
            }
            else if (IsTextBoxInList("tb" + textBoxId, _listTbDone))
            {
                var textBox = FindTextbox("tb" + textBoxId, _listTbDone);
                RemoveTextBox(textBox, lvDone, _listTbDone, ref _pointYDone);
            }
            _listTask.Remove(task);
        }

        private void ShowTaskDetail(Task task)
        {
            MessageBoxManager.Yes = "Edit";
            MessageBoxManager.No = "Delete";
            MessageBoxManager.Cancel = "Close";
            MessageBoxManager.Register();

            var dialogResult = MessageBox.Show(string.Format("Title: {0}\nDescription: {1}\nTime create: {2}",
                task.Title, task.Description, Convert.ToDateTime(task.TimeCreate)),
                "Task Detail", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            MessageBoxManager.Unregister();
            if (dialogResult == DialogResult.Yes)
            {
                EditTask(task);
            }
            else if (dialogResult == DialogResult.No)
            {
                var confirmResult = MessageBox.Show("Do you want to delete this task?", "Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    DeleteTask(task);
                }
            }
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            var formAddTask = new AddTaskForm
            {
                passData = new AddTaskForm.PassData(PassData),
                showMainForm = new AddTaskForm.ShowMainForm(Show)
            };
            formAddTask.Show();
            this.Hide();
        }

        public void PassData(Task task)
        {
            if (task.Id == -1) //add new task
            {
                if (!IsDuplicateTask(_listTask, task))
                {
                    this.Show();
                    task.Id = _listTask.Count;
                    _listTask.Add(task);
                    CreateNewTextBox(task, lvToDo, ref _pointYTodo, _listTbTodo);
                }
                else
                {
                    MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show();
                }
            }
            else //update an exists task
            {
                _listTask[task.Id].Title = task.Title;
                _listTask[task.Id].Description = task.Description;
                UpdateTextBox(task);
                this.Show();
            }
        }

        public void UpdateTextBox(Task task)
        {
            TextBox textBox;

            if (FindTextbox("tb" + task.Id, _listTbTodo) != null)
            {
                textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                _listTbTodo[FindTextBoxIndex(_listTbTodo, textBox)].Text = task.Title;
            }
            else if (FindTextbox("tb" + task.Id, _listTbDoing) != null)
            {
                textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                _listTbDoing[FindTextBoxIndex(_listTbDoing, textBox)].Text = task.Title;
            }
            else if (FindTextbox("tb" + task.Id, _listTbDone) != null)
            {
                textBox = FindTextbox("tb" + task.Id, _listTbDone);
                _listTbDone[FindTextBoxIndex(_listTbDone, textBox)].Text = task.Title;
            }
        }

        private void LvDoing_DragEnter(object sender, DragEventArgs e)
        {
            var textBoxId = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsTextBoxInList("tb" + textBoxId, _listTbDoing))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDone_DragEnter(object sender, DragEventArgs e)
        {
            var textBoxId = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsTextBoxInList("tb" + textBoxId, _listTbDone))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvTodo_DragEnter(object sender, DragEventArgs e)
        {
            var textBoxId = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsTextBoxInList("tb" + textBoxId, _listTbTodo))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDoing_DragDrop(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            CreateNewTextBox(task, listView, ref _pointYDoing, _listTbDoing);

            if (IsTextBoxInList("tb" + task.Id, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                RemoveTextBox(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsTextBoxInList("tb" + task.Id, _listTbDone))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDone);
                RemoveTextBox(textBox, lvDone, _listTbDone, ref _pointYDone);
            }
        }

        private void LvDone_DragDrop(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            CreateNewTextBox(task, listView, ref _pointYDone, _listTbDone);

            if (IsTextBoxInList("tb" + task.Id, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                RemoveTextBox(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsTextBoxInList("tb" + task.Id, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                RemoveTextBox(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void LvTodo_DragDrop(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            CreateNewTextBox(task, listView, ref _pointYTodo, _listTbTodo);

            if (IsTextBoxInList("tb" + task.Id, _listTbDone))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDone);
                RemoveTextBox(textBox, lvDone, _listTbDone, ref _pointYDone);
            }

            if (IsTextBoxInList("tb" + task.Id, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                RemoveTextBox(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBox;
            var title = textBox.Text;
            var taskIndex = FindIndexTask(_listTask, title);

            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                textBox.DoDragDrop(_listTask[taskIndex], DragDropEffects.Move);
            }
            else
            {
                ShowTaskDetail(_listTask[taskIndex]);
            }
        }
    }
}
