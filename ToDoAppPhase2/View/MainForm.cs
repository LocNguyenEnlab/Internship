using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ToDoAppPhase1.BLL;
using System.IO;
using System.Linq;

namespace ToDoAppPhase2
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
        private int _dataOptions; //0: sql server, 1: file sytem
        private BLLSqlTask _bllSql;
        private BLLFileSystemTask _bllFileSystem;

        public MainForm()
        {
            InitializeComponent();
            _pointYTodo = 0;
            _pointYDoing = 0;
            _pointYDone = 0;
            _listTbTodo = new List<TextBox>();
            _listTbDoing = new List<TextBox>();
            _listTbDone = new List<TextBox>();
            _bllSql = new BLLSqlTask();
            _bllFileSystem = new BLLFileSystemTask();
        }

        private void ShowData()
        {
            if (_dataOptions == (int)DataOptions.SqlServer)
            {
                _listTask = _bllSql.GetAllTask();
            }
            else
            {
                _listTask = _bllFileSystem.GetAllTask();
            }
            foreach(var item in _listTask)
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

        private bool IsTextBoxInList(string nameTb, List<TextBox> list)
        {
            if (FindTextbox(nameTb, list) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private TextBox FindTextbox(string textBoxName, List<TextBox> textBoxList)
        {
            return textBoxList.FirstOrDefault(s => s.Name == textBoxName);
        }

        private int FindTextBoxIndex(List<TextBox> textBoxList, TextBox textBox)
        {
            return textBoxList.FindIndex(s=>s.Name == textBox.Name);
        }       

        private void RemoveTextBoxFromListView(TextBox textBox, ListView listView, List<TextBox> textBoxList, ref int pointY)
        {
            int textBoxIndex = FindTextBoxIndex(textBoxList, textBox);

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
                Text = "Edit task",
                passData = new AddTaskForm.PassData(PassData),
                showMainForm = new AddTaskForm.ShowMainForm(Show)
            };
            addTaskForm.Show();
            this.Hide();
        }
        
        private void DeleteTask(int taskId)
        {
            if (IsTextBoxInList("tb" + taskId, _listTbTodo))
            {
                var textBox = FindTextbox("tb" + taskId, _listTbTodo);
                RemoveTextBoxFromListView(textBox, lvToDo, _listTbTodo, ref _pointYTodo);
                if (_dataOptions == (int)DataOptions.SqlServer)
                {
                    _bllSql.DeleteTask(taskId);
                }
                else
                {
                    _bllFileSystem.DeleteTask(taskId);
                }
            }
            else if (IsTextBoxInList("tb" + taskId, _listTbDoing))
            {
                var textBox = FindTextbox("tb" + taskId, _listTbDoing);
                RemoveTextBoxFromListView(textBox, lvDoing, _listTbDoing, ref _pointYDoing);
                if (_dataOptions == (int)DataOptions.SqlServer)
                {
                    _bllSql.DeleteTask(taskId);
                }
                else
                {
                    _bllFileSystem.DeleteTask(taskId);
                }
            } 
            else if (IsTextBoxInList("tb" + taskId, _listTbDone)) {
                var textBox = FindTextbox("tb" + taskId, _listTbDone);
                RemoveTextBoxFromListView(textBox, lvDone, _listTbDone, ref _pointYDone);
                if (_dataOptions == (int)DataOptions.SqlServer)
                {
                    _bllSql.DeleteTask(taskId);
                }
                else
                {
                    _bllFileSystem.DeleteTask(taskId);
                }
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
                DialogResult confirmResult = MessageBox.Show("Do you want to delete this task?", "Warning", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                if (_dataOptions == (int)DataOptions.SqlServer)
                {
                    if (!_bllSql.IsDuplicateTask(task))
                    {
                        this.Show();
                        _bllSql.AddTask(task);
                        int id = _bllSql.GetMaxId();
                        task = _bllSql.GetTask(id);
                        CreateNewTextBox(task);
                    }
                    else
                    {
                        MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Show();
                    }
                }
                else
                {
                    if (!_bllFileSystem.IsDuplicateTask(task))
                    {
                        this.Show();
                        task.Id = _bllFileSystem.GetMaxId();
                        _bllFileSystem.AddTask(task);
                        CreateNewTextBox(task);
                    }
                    else
                    {
                        MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Show();
                    }
                }
            } 
            else //update an exists task
            {
                if (_dataOptions == (int)DataOptions.SqlServer)
                {
                    _bllSql.UpdateTask(task);
                } 
                else
                {
                    _bllFileSystem.UpdateTask(task);
                }
                UpdateTextBox(task);
                this.Show();
            }
        }

        public void UpdateTextBox(Task task)
        {
            if (FindTextbox("tb" + task.Id, _listTbTodo) != null)
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbTodo);
                _listTbTodo[FindTextBoxIndex(_listTbTodo, textBox)].Text = task.Title;
            }
            else if (FindTextbox("tb" + task.Id, _listTbDoing) != null)
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDoing);
                _listTbDoing[FindTextBoxIndex(_listTbDoing, textBox)].Text = task.Title;
            }
            else if (FindTextbox("tb" + task.Id, _listTbDone) != null)
            {
                var textBox = FindTextbox("tb" + task.Id, _listTbDone);
                _listTbDone[FindTextBoxIndex(_listTbDone, textBox)].Text = task.Title;
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
            
            if(!IsTextBoxInList("tb" + textBoxId, _listTbTodo))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDoing_DragDrop(object sender, DragEventArgs e)
        {
            var task = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            task.TypeList = (int)TypeList.Doing;
            if (_dataOptions == (int)DataOptions.SqlServer)
            {
                _bllSql.UpdateTask(task);
            } 
            else
            {
                _bllFileSystem.UpdateTask(task);
            }
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
            if (_dataOptions == (int)DataOptions.SqlServer)
            {
                _bllSql.UpdateTask(task);
            }
            else
            {
                _bllFileSystem.UpdateTask(task);
            }
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
            task.TypeList = 0;
            if (_dataOptions == (int)DataOptions.SqlServer)
            {
                _bllSql.UpdateTask(task);
            }
            else
            {
                _bllFileSystem.UpdateTask(task);
            }
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
            Task t;

            if (_dataOptions == (int)DataOptions.SqlServer)
            {
                t = _bllSql.GetTask(textBoxId);
            } 
            else
            {
                t = _bllFileSystem.GetTask(textBoxId);
            }

            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                textBox.DoDragDrop(t, DragDropEffects.Move);
            }
            else
            {
                ShowTaskDetail(t);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBoxManager.Yes = "Sql server";
            MessageBoxManager.No = "File system";
            MessageBoxManager.Register();
            DialogResult result = MessageBox.Show("Do you want to use sql server or file system?", "Inform", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            MessageBoxManager.Unregister();
            if (result == DialogResult.Yes)
            {
                _dataOptions = (int)DataOptions.SqlServer;
                ShowData();
            } 
            else if (result == DialogResult.No)
            {
                _dataOptions = (int)DataOptions.FileSystem;
                ShowData();                
            } 
        }
    }
}
