using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SNNReturn
{
    public class Check
    {

        ObservableCollection<CalData> checklist;
        public Check (ObservableCollection<CalData> ObsList)
        {
            this.checklist = ObsList;

        }


        public ObservableCollection<CalData> CheckNull()
        {
            foreach (var x in checklist)
            {
                if (x.ge_serial_no == null)
                {
                    x.ge_serial_no = "";
                }
                if (x.assembly_catalog_no == null)
                {
                    x.assembly_catalog_no = "";
                }
                if (x.amr_assembly_no == null)
                {
                    x.amr_serial_no = "";

                }
                if (x.reject_reason_1 == null)
                {
                    x.reject_reason_1 = "";
                }
                if (x.Description == null)
                {
                    x.Description = "";
                }
                if (x.Date_Documented == null)
                {
                    x.Date_Documented = DateTime.Now;
                }



            }

            return checklist;
        }

       public bool Form()
        {
            bool result = true;
            bool stop = false;
            string trim ;
            int count = 0;
            List<char> charlist = new List<char> {'A','a','B','b','C','c','D','d','E','e','F','G','g','H','h','I','i','J','j','K','L','l','M','m','N','n','O','o','P','p','Q','q','R','r','S','s','T','t','U','u','V','v','W','w','X','x','Y','y','Z','z'};
            try
            {


                foreach (var x in checklist)
                {
                    Console.WriteLine("ge length = " + x.ge_serial_no.Trim().Length);
                    Console.WriteLine("Count value " + count);
                    if (x.ge_serial_no.Trim().Length < 8 || x.ge_serial_no.Trim().Length > 10)
                    {
                        Application.Current.Dispatcher.Invoke(() => MessageBox.Show("check ge serial number length at index " + count.ToString()));
                        result = false;
                        break;

                    }

                    trim = x.ge_serial_no.Trim();
                    if (x.ge_serial_no[0] == 'X' || x.ge_serial_no[0] == 'R')
                    {
                        trim = x.ge_serial_no.Substring(1);
                    }
                    if (x.ge_serial_no.Trim()[x.ge_serial_no.Trim().Length - 1] == 'X' || x.ge_serial_no.Trim()[x.ge_serial_no.Trim().Length - 1] == 'R')
                    {
                        trim = x.ge_serial_no.Trim().Remove(x.ge_serial_no.Length - 1);
                    }

                    for (int i = 0; i < charlist.Count(); i++)
                    {
                        //Console.WriteLine("Loop");

                        if (trim.Contains(charlist[i]))
                        {

                            Application.Current.Dispatcher.Invoke(() => MessageBox.Show("check ge serial form at index " + count.ToString()));
                            result = false;
                            stop = true;
                            break;
                        }
                    }
                    if (stop == true)
                    {
                        break;
                    }

                    if (x.amr_serial_no.Trim().Length != 16)
                    {
                        Application.Current.Dispatcher.Invoke(() => MessageBox.Show("check amr length at index " + count.ToString()));
                        result = false;
                        stop = true;
                        break;
                    }


                    count++;
                }

            }

            catch (Exception ex)
            {

            }
            return result ;
        }
    }
}
