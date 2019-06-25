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
        private BusinessLogic _bll;

        public MainForm()
        {
            InitializeComponent();
            _pointYTodo = 0;
            _pointYDoing = 0;
            _pointYDone = 0;
            _listTbTodo = new List<TextBox>();
            _listTbDoing = new List<TextBox>();
            _listTbDone = new List<TextBox>();
            _bll = new BusinessLogic();
            ShowData();
        }

        private void ShowData()
        {
            _listTask = _bll.GetAllTask();
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

        private void EditTask(Task task, Author author)
        {
            var addTaskForm = new AddTaskForm(task, author)
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
                try
                {
                    _bll.CreateTransaction();
                    var textBox = FindTextbox("tb" + taskId, _listTbTodo);
                    RemoveTextBoxFromListView(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
                    _bll.DeleteTask(taskId);
                    _bll.Commit();
                }
                catch (Exception)
                {
                    _bll.RollBack();
                }
                
            }
            else if (IsTextBoxInList("tb" + taskId, _listTbDoing))
            {
                try
                {
                    _bll.CreateTransaction();
                    var textBox = FindTextbox("tb" + taskId, _listTbDoing);
                    RemoveTextBoxFromListView(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
                    _bll.DeleteTask(taskId);
                    _bll.Commit();
                }
                catch (Exception)
                {
                    _bll.RollBack();
                }
                
            }
            else if (IsTextBoxInList("tb" + taskId, _listTbDone))
            {
                try
                {
                    _bll.CreateTransaction();
                    var textBox = FindTextbox("tb" + taskId, _listTbDone);
                    RemoveTextBoxFromListView(textBox, lvDone, _listTbDone, ref _pointYDone);
                    _bll.DeleteTask(taskId);
                    _bll.Commit();
                }
                catch (Exception)
                {
                    _bll.RollBack();
                }
            }
        }

        private void ShowTaskDetail(Task task, Author author)
        {
            MessageBoxManager.Yes = "Edit";
            MessageBoxManager.No = "Delete";
            MessageBoxManager.Cancel = "Close";
            MessageBoxManager.Register();

            DialogResult dialogResult = MessageBox.Show(string.Format("Title: {0}\nDescription: {1}\nTime create: {2}" +
                "\nAuthor: {3}", task.Title, task.Description, Convert.ToDateTime(task.TimeCreate), author.AuthorName),
                "Task Detail", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();

            if (dialogResult == DialogResult.Yes)
            {
                EditTask(task, author);
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

        public void PassData(Task task, Author author)
        {
            if (task.Id == -1) //add new task
            {
                if (!_bll.IsDuplicateTask(task))
                {
                    this.Show();                    
                    try
                    {
                        _bll.CreateTransaction();
                        if (_bll.IsDuplicateAuthor(author))
                        {
                            author = _bll.GetAuthor(author.Id);
                        }

                        try
                        {
                            task.Id = _bll.GetMaxTaskId() + 1;
                        }
                        catch (Exception)
                        {
                            task.Id = 1;
                        }

                        task.Author = author;
                        task.AuthorName = author.AuthorName;
                        author.Tasks.Add(task);
                        _bll.AddTask(task);
                        _bll.Commit();
                        int id = _bll.GetMaxTaskId();
                        task = _bll.GetTask(id);
                        CreateNewTextBox(task);
                    }
                    catch (Exception)
                    {
                        _bll.RollBack();
                    }
                }
                else
                {
                    MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Show();
                }
            }
            else //update an exists task 
            {
                try
                {
                    _bll.CreateTransaction();
                    var oldAuthor = _bll.GetAuthor(task.Author.Id);                    
                    if (oldAuthor.AuthorName == author.AuthorName)
                    {
                        //not change author, update task as normal
                        _bll.UpdateTask(task);
                    }
                    else
                    {
                        //update task author and task info
                        author = _bll.GetAuthor(author.Id);
                        task.Author = author;
                        task.AuthorName = author.AuthorName;
                        author.Tasks.Add(task);
                        _bll.UpdateTask(task);
                    }
                    _bll.Commit();
                    UpdateTextBox(task);
                }
                catch (Exception)
                {
                    _bll.RollBack();
                }
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
            _bll.CreateTransaction();
            _bll.UpdateTask(task);
            _bll.Commit();
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
            task.TypeList = (int)TypeList.Done;
            _bll.CreateTransaction();
            _bll.UpdateTask(task);
            _bll.Commit();
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
            _bll.CreateTransaction();
            _bll.UpdateTask(task);
            _bll.Commit();
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
            var textBoxName = textBox.Name;
            int textBoxId = Convert.ToInt32(textBoxName.Replace("tb", ""));
            var task = _bll.GetTask(textBoxId);
            var author = _bll.GetAuthor(task.Author.Id);

            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                textBox.DoDragDrop(task, DragDropEffects.Move);
            }
            else
            {
                ShowTaskDetail(task, author);
            }
        }

    }
}
