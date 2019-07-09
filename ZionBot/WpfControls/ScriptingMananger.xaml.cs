using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace OtClientBot.WpfControls
{
    /// <summary>
    /// Interaction logic for ScriptingMananger.xaml
    /// </summary>
    public partial class ScriptingMananger : UserControl
    {

        [AddINotifyPropertyChangedInterface]
        public class ScriptingManangerViewModel
        {
            public ObservableCollection<Script> ScriptListSource { get; set; }



            public ScriptingManangerViewModel()
            {



            }

        }

        [AddINotifyPropertyChangedInterface]
        public class Script
        {
            public string path { get; set; }

            public string name { get; set; }

            public string code { get { return ReadCode(); } }

            public string output { get { return this.instance == null ? null : this.instance.output; } }


            RunningScript instance { get; set; }


            public RunningScript.Status status { get; set; }


            public Script(string path, string name)
            {
                this.status = RunningScript.Status.Ready;
                this.path = path;
                this.name = name;

            }


            public string ReadCode()
            {
                return System.IO.File.ReadAllText(path);
            }

            public void SaveCode(string code)
            {
                System.IO.File.WriteAllText(path, code);
            }


            public void UpdateStatus(RunningScript.Status status)
            {
                this.status = status;
            }

            public bool Run()
            {
                if (this.instance != null && this.instance.status == RunningScript.Status.Running) return false;

                this.instance = ScriptingEngine.RunScript(this.code, this.name,UpdateStatus);

                return true;

            }


            public bool Stop()
            {
                if (this.instance != null && this.instance.status == RunningScript.Status.Running)
                {
                    this.instance.Stop();
                    return true;
                }
                return false;
            }









        }


        public ScriptingManangerViewModel ViewModel;


        public ScriptingMananger()
        {
            ViewModel = new ScriptingManangerViewModel();

            ViewModel.ScriptListSource = new ObservableCollection<Script>();

            this.DataContext = ViewModel;
            InitializeComponent();

            LoadScriptList();
        }


        public void NewScript()
        {

            string name = Microsoft.VisualBasic.Interaction.InputBox("What is the new name you want to give to the script?", "Rename", "");



            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name can't be empty");
                return;
            }

            if ((from x in Directory.GetFiles("scripts") select System.IO.Path.GetFileName(x).ToLower()).ToList().Contains(name.ToLower()))
            {
                MessageBox.Show("File with this name already exists.");
                return;
            }


            string path = System.IO.Path.Combine("scripts", name);

            File.WriteAllText(path, "");

            var s = new Script(path, name);

            s.path = path;

            s.name = name;


            ViewModel.ScriptListSource.Add(s);
        }


        private void LoadScriptList()
        {
            ViewModel.ScriptListSource.Clear();

            if (!Directory.Exists("scripts"))
                Directory.CreateDirectory("scripts");

            string[] scripts = System.IO.Directory.GetFiles("scripts");

            foreach (string path in scripts)
            {
                ViewModel.ScriptListSource.Add(new Script(path, System.IO.Path.GetFileName(path)));

            }
        }

        private void btnPlayScript_Click(object sender, RoutedEventArgs e)
        {
            var index = Int32.Parse(((Button)(sender)).Tag.ToString());

            Script s = ViewModel.ScriptListSource[index];


            if (!s.Run()) MessageBox.Show("Script already running.");

        }



        private void btnStopScript_Click(object sender, RoutedEventArgs e)
        {
            var index = Int32.Parse(((Button)(sender)).Tag.ToString());

            Script s = ViewModel.ScriptListSource[index];

            if (!s.Stop()) MessageBox.Show("Script is not running.");


        }

        private void btnEditScript_Click(object sender, RoutedEventArgs e)
        {
            var index = Int32.Parse(((Button)(sender)).Tag.ToString());

            Script s = ViewModel.ScriptListSource[index];

            var sEditorForm = new Forms.ScriptEditor(s.code,s.path,s.name);


            sEditorForm.Show();
            

        }

        private void btnSeeOutputFromScript_Click(object sender, RoutedEventArgs e)
        {
            var index = Int32.Parse(((Button)(sender)).Tag.ToString());

            Script s = ViewModel.ScriptListSource[index];

            new Forms.TextViewer(() => { return s.output; }, "Output of: " + s.name).Show();


        }

        private void btnDeleteScript_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this script?", "Press Yes to confirm.",MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {                
                var index = Int32.Parse(((Button)(sender)).Tag.ToString());

                Script s = ViewModel.ScriptListSource[index];
                
                s.Stop();

                System.IO.File.Delete(s.path);

                ViewModel.ScriptListSource.Remove(s);

            }

        }


        private void btnRenameScript_Click(object sender, RoutedEventArgs e)
        {
            var index = Int32.Parse(((Button)(sender)).Tag.ToString());

            Script s = ViewModel.ScriptListSource[index];


            string newName = Microsoft.VisualBasic.Interaction.InputBox("What is the new name you want to give to the script?", "Rename",s.name);

            if (newName == s.name) return;

            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Name can't be empty");
                return;
            }

            if ((from x in System.IO.Directory.GetFiles("scripts") select System.IO.Path.GetFileName(x).ToLower()).ToList().Contains(newName.ToLower()))
            {
                MessageBox.Show("File with this name already exists.");
                return;
            }

            string newPath = System.IO.Path.Combine("scripts", newName);

            File.WriteAllText(newPath,s.code);
            File.Delete(s.path);


            s.path = newPath;

            s.name = newName;




        }
    }
}
