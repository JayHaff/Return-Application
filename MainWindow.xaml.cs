using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.IO;

using Excel = Microsoft.Office.Interop.Excel;

namespace SNNReturn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Query1> list = new List<Query1> { };
        List<CalData> list2 = new List<CalData> { };

        bool run1;
        bool run2;
        int count;
        int length;
        string input;
        int index;
        bool stop;
        BackgroundWorker bg3;
        public ObservableCollection<CalData> ReturnList { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();
            run1 = true;
            run2 = false;
            MyDataGrid.SelectAll();
            length = 10;
            txtinput.Text = "Scan Assemby No";
            defectDesc.IsEnabled = false;
            defectDesc.Background = Brushes.White;
            Submit_Button.IsEnabled = false;
            NextButton.IsEnabled = false;
            PrevButton.IsEnabled = false;
            ReturnList = new ObservableCollection<CalData>();
            DataContext = this;
            SerialNumber.Focus();

        }


        public event PropertyChangedEventHandler PropertyChanged
        {
            add => ((INotifyPropertyChanged)ReturnList).PropertyChanged += value;

            remove => ((INotifyPropertyChanged)ReturnList).PropertyChanged -= value;
        }


        private void SerialNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            

            if (SerialNumber.Text.Length >= length )
            {

                count = 0;
                NextButton.IsEnabled = false;
                PrevButton.IsEnabled = false;
                Submit_Button.IsEnabled = false;
                BackgroundWorker bg = new BackgroundWorker();
                bg.DoWork += Bg_DoWork;
                bg.RunWorkerCompleted += Bg_RunWorkerCompleted;
                bg.RunWorkerAsync();
            }
        }

        private void Bg_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(50);


        }
        private void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Console.WriteLine(count);
            if (count == 0)
            {
                ++count;

                if (run1 == true)
                {
                    input = SerialNumber.Text.Trim();
                    if (SerialNumber.Text == " " && length == 1)
                    {
                        if (list.Count > 0)
                        {
                            txtinput.Text = "Scan Mac Address";
                            SerialNumber.Text = "";
                            length = 16;
                            SerialNumber.Background = Brushes.White;
                        }
                        else
                        {
                            txtinput.Text = "Scan Assemby No";
                            SerialNumber.Background = Brushes.White;
                            SerialNumber.Text = "";
                            length = 10;
                        }

                    }
                    else if (length == 10)
                    {
                        BackgroundWorker bg1 = new BackgroundWorker();
                        bg1.DoWork += Bg1_DoWork;
                        bg1.RunWorkerCompleted += Bg1_RunWorkerCompleted;
                        bg1.RunWorkerAsync();


                    }
                    else
                        SerialNumber.Text = "";

                }


                if (run2 == true)
                {
                    input = SerialNumber.Text.Trim();
                    if (SerialNumber.Text == " " && length ==1)
                    {
                        length = 10;
                        txtinput.Text = "Scan Assemby No";
                        SerialNumber.Background = Brushes.White;
                        SerialNumber.Text = "";
                        run2 = false;
                        run1 = true;


                    }
                    else if (length == 16)
                    {
                        BackgroundWorker bg2 = new BackgroundWorker();
                        bg2.DoWork += Bg2_DoWork;
                        bg2.RunWorkerCompleted += Bg2_RunWorkerCompleted;
                        bg2.RunWorkerAsync();

                        list2 = DatabaseHelper.CalData(SerialNumber.Text.Trim());


                    }
                    else
                        SerialNumber.Text = "";
                }
            }
        }

        private void Bg1_DoWork(object sender, DoWorkEventArgs e)
        {

            list = DatabaseHelper.BoardData(input);

        }


        private void Bg1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (list.Count > 0)
            {

                NextButton.IsEnabled = true;
                PrevButton.IsEnabled = true;
                var x = list[0];
                if (x.option_board_type.Trim() == "I210_Silver_Springs")
                {
                    run1 = false;
                    run2 = true;
                    length = 16;
                    SerialNumber.Text = "";
                    txtinput.Text = "Scan Mac Address";
                    Console.WriteLine("here2");


                }

                else
                {
                    txtinput.Text = "AMR is not an SSN";
                    SerialNumber.Background = Brushes.Red;
                    SerialNumber.Text = "";
                    length = 1;
                }
            }
            else
            {
                txtinput.Text = "Assembly No not found";
                SerialNumber.Text = "";
                SerialNumber.Background = Brushes.Red;
                length = 1;
            }

        }


        private void Bg2_DoWork(object sender, DoWorkEventArgs e)
        {

            list2 = DatabaseHelper.CalData(input);

        }


        private void Bg2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (list2.Count > 0)
            {
                run2 = false;
                run1 = true;
                length = 10;
                SerialNumber.Text = "";
                txtinput.Text = "Scan Assemby No";
                Launch();
            }
            else
            {
                txtinput.Text = "AMR not found";
                SerialNumber.Text = "";
                SerialNumber.Background = Brushes.Red;
                length = 1;
            }
        }

        public void Launch()
        {
            Mac.Text = input;
            Submit_Button.IsEnabled = true;
            index = 0;

            if (list2.Count > 1)
            {
                NextButton.IsEnabled = true;
            }

            Console.WriteLine("Lunch in·i·ti·ate.ed!");
            AmrInfo();

        }


        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = true;
            if (index > 0)
            {
                --index;
                AmrInfo();
            }
            else
            {
                PrevButton.IsEnabled = false;
            }

        }


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            PrevButton.IsEnabled = true;
            if (index < list2.Count - 1)
            {
                ++index;
                AmrInfo();
            }
            else
            {
                NextButton.IsEnabled = false;
            }
        }


        public void AmrInfo()
        {

            var x = list2[index];

            Date.Text = x.test_start_time.ToString();
            Status.Text = x.status;
            Error.Text = x.reject_reason_1;

            if (Status.Text.Trim() == "P")
            {
                Status.Foreground = Brushes.Green;
            }

            if (Status.Text.Trim() == "F")
            {
                Status.Foreground = Brushes.Red;
            }




        }

        private void Defect_Checked(object sender, RoutedEventArgs e)
        {
            defectDesc.IsEnabled = true;
            defectDesc.Background = Brushes.LightGreen;
            defectDesc.Text = "Enter descition";
            stop = false;


            bg3 = new BackgroundWorker();
            bg3.DoWork += Bg3_DoWork;
            bg3.RunWorkerCompleted += Bg3_RunWorkerCompleted;
            bg3.RunWorkerAsync();


        }

        private void Bg3_DoWork(object sender, DoWorkEventArgs e)
        {

            while (Application.Current.Dispatcher.Invoke(() => defectDesc.IsMouseDirectlyOver == false))
            {
                if (stop == true)
                {
                    break;
                }
            }

        }

        private void Bg3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            defectDesc.Text = "";

        }

        private void Defect_Unchecked(object sender, RoutedEventArgs e)
        {
            stop = true;
            bg3.Dispose();
            defectDesc.Text = " ";
            defectDesc.IsEnabled = false;
            defectDesc.Background = Brushes.White;
            defectDesc.Text = "";

        }

       

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Pro.IsChecked == false && Man.IsChecked == false && Scrap.IsChecked == false && FA.IsChecked == false)
            {
                MessageBox.Show("Plese Select an Option");
            }


            else
            {
                if (Pro.IsChecked == true )
                {
                    var x = list2[index];

                    x.Return_Production = true;
                    x.Return_Manufacturer = false;
                    x.Scrap = false;
                    x.Further_Analysis = false;

                }

                if (Man.IsChecked == true)
                {
                    var x = list2[index];

                    x.Return_Production = false;
                    x.Return_Manufacturer = true;
                    x.Scrap = false;
                    x.Further_Analysis = false;
                }
                if (Scrap.IsChecked == true)
                {
                    var x = list2[index];

                    x.Return_Production = false;
                    x.Return_Manufacturer = false;
                    x.Scrap = true;
                    x.Further_Analysis = false;
                }
                if (FA.IsChecked == true)
                {
                    var x = list2[index];

                    x.Return_Production = false;
                    x.Return_Manufacturer = false;
                    x.Scrap = false;
                    x.Further_Analysis = true;
                }
                if (defect.IsChecked == true)
                {
                    var x = list2[index];

                    x.Physical_defect = true;
                    x.Description = defectDesc.Text;

                }
                if (defect.IsChecked == false)
                {
                    var x = list2[index];

                    x.Physical_defect = false;
                    x.Description = "";

                }

                var y = list2[index];
                y.Date_Documented = DateTime.Now;


                var cal = list2[index];



                ReturnList.Add(cal);


                Clear();

            }
            
        }

        public void Clear()
        {
            list.Clear();
            list2.Clear();
           
            Date.Text = "";
            Status.Text = "";
            Error.Text = "";
            Mac.Text = "";

            Pro.IsChecked = false;
            Man.IsChecked = false;
            Scrap.IsChecked = false;
            FA.IsChecked = false;
            defect.IsChecked = false;
            Submit_Button.IsEnabled = false;
            NextButton.IsEnabled = false;
            PrevButton.IsEnabled = false;

        }



        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnList.Count > 0)
            {


                int selectedItem = MyDataGrid.SelectedIndex;
                Console.WriteLine(selectedItem + " is the index");
                Console.WriteLine(ReturnList.Count + " is the length");



                if (ReturnList.Count > selectedItem && selectedItem >= 0)
                {
                    ReturnList.RemoveAt(selectedItem);
                    Console.WriteLine(ReturnList.Count + " Deleted");
                }

                if (selectedItem == -1)
                {
                    MessageBox.Show("Selct a Row");
                }
            }
            else
                MessageBox.Show("No data to delete");

        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (ReturnList.Count > 0)
            {
              
                if (MessageBox.Show("Are you sure you want to record data. The working will be lost", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Record.IsEnabled = false;

                    Check check = new Check(ReturnList);
                    ReturnList = check.CheckNull();
                    if (check.Form() == true)
                    {


                        SavetoExcel Rec = new SavetoExcel();
                        Rec.Save_to_Excel(ReturnList);



                        foreach (var x in ReturnList)
                        {
                            DatabaseHelper.AddData(x.ge_serial_no, x.assembly_catalog_no, x.amr_serial_no,x.amr_assembly_no, x.test_start_time, x.test_complete_time, x.status, x.reject_reason_1, x.Return_Production.ToString(), x.Return_Manufacturer.ToString(),x.Scrap.ToString(), x.Further_Analysis.ToString(),x.Physical_defect.ToString(), x.Description, x.Date_Documented);

                        }


                        ReturnList.Clear();

                        Record.IsEnabled = true;
                    }
                    else
                    {
                        Record.IsEnabled = true;
                    }
                }
                
            }
            else
            {
                MessageBox.Show("no data to record");
            }

        }

        private void Pro_Checked(object sender, RoutedEventArgs e)
        {
            Man.IsChecked = false;
            Scrap.IsChecked = false;
            FA.IsChecked = false;
        }

        private void Man_Checked(object sender, RoutedEventArgs e)
        {
            Pro.IsChecked = false;
            Scrap.IsChecked = false;
            FA.IsChecked = false;
        }

        private void Scrap_Checked(object sender, RoutedEventArgs e)
        {
            Man.IsChecked = false;
            Pro.IsChecked = false;
            FA.IsChecked = false;
        }

        private void FA_Checked(object sender, RoutedEventArgs e)
        {
            Man.IsChecked = false;
            Scrap.IsChecked = false;
            Pro.IsChecked = false;
        }

     
    }
}