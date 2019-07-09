using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PropertyChanged;
using System.Threading;
using System.Collections.ObjectModel;

namespace OtClientBot.WpfControls
{

    [AddINotifyPropertyChangedInterface]
    public class PolicyPanelViewModel
    {
        public ObservableCollection<Cavebot.TargetPolicy> PolicyListSource { get; set; }



        public PolicyPanelViewModel() {



        }

    }


    /// <summary>
    /// Interaction logic for TargetingPolicyPanel.xaml
    /// </summary>
    public partial class TargetingPolicyPanel : UserControl
    {
        public PolicyPanelViewModel ViewModel;


        public TargetingPolicyPanel()
        {
            ViewModel = new PolicyPanelViewModel();
            this.DataContext = ViewModel;
            InitializeComponent();            
        }



        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var policyIndex = Int32.Parse(((Button)(sender)).Tag.ToString());
            
            ViewModel.PolicyListSource[policyIndex].name = string.Empty;

            ViewModel.PolicyListSource.RemoveAt(policyIndex);

           

        }

        private void txtCreatureName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }




        }
    }
}
