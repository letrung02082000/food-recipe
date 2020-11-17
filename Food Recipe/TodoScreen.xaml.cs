using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Food_Recipe
{
    public partial class TodoScreen : UserControl
    {
        public class Todo
        {
            public bool Done { get; set; } = false;
            public string TodoData { get; set; }
        }
        public ObservableCollection<Todo> TodoList = new ObservableCollection<Todo>();
        
        public TodoScreen()
        {
            InitializeComponent();
            
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string todoData = TodoTextBox.Text.Trim();
            Todo todo = new Todo()
            {
                TodoData = todoData,
                Done = false
            };
            
            if(todoData != "")
            {
                TodoList.Add(todo);

                if (File.Exists("todo.xml"))
                {
                    XDocument todoDoc = XDocument.Load("todo.xml");
                    XElement todoElement = todoDoc.Root;
                    todoElement.Add(new XElement("tododata", todoData, new XAttribute("done", 0)));
                    todoDoc.Save("todo.xml");
                }
                else
                {
                    XDocument todoDoc = new XDocument(
                        new XElement("todo", new XElement("tododata", todoData, new XAttribute("done", 0))
                        ));
                    todoDoc.Save("todo.xml");
                }
                TodoTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Vui lòng nhập nội dung");
            }

        }

        private void ReadTodoList(ObservableCollection<Todo> todoList)
        {
            if (File.Exists("todo.xml"))
            {
                XDocument todoDoc = XDocument.Load("todo.xml");
                XElement todoElement = todoDoc.Root;
                IEnumerable<XElement> todoDataList = from todoData in todoElement.Descendants() select todoData;

                foreach (XElement todoData in todoDataList)
                {
                    int doneInt = int.Parse(todoData.Attribute("done").Value);
                    bool done = doneInt == 1 ? true : false;
                    Todo todo = new Todo()
                    {
                        Done = done,
                        TodoData = todoData.Value
                    };
                    todoList.Add(todo);
                }
            }
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var index = TodoListView.Items.IndexOf((sender as FrameworkElement).DataContext);
            XDocument todoDoc = XDocument.Load("todo.xml");
            XElement todoElement = todoDoc.Root;
            IEnumerable<XElement> todoDataList = from todoData in todoElement.Descendants() select todoData;

            todoDataList.ElementAt(index).SetAttributeValue("done", 1);
            todoDoc.Save("todo.xml");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var index = TodoListView.Items.IndexOf((sender as FrameworkElement).DataContext);
            XDocument todoDoc = XDocument.Load("todo.xml");
            XElement todoElement = todoDoc.Root;
            IEnumerable<XElement> todoDataList = from todoData in todoElement.Descendants() select todoData;

            todoDataList.ElementAt(index).SetAttributeValue("done", 0);
            todoDoc.Save("todo.xml");
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var index = TodoListView.Items.IndexOf((sender as FrameworkElement).DataContext);
            TodoList.RemoveAt(index);
            XDocument todoDoc = XDocument.Load("todo.xml");
            XElement todoElement = todoDoc.Root;
            IEnumerable<XElement> todoDataList = from todoData in todoElement.Descendants() select todoData;

            todoDataList.ElementAt(index).Remove();
            todoDoc.Save("todo.xml");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReadTodoList(TodoList);
            TodoListView.ItemsSource = TodoList;
        }
    }
}
