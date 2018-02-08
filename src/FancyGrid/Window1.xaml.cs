using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FancyGrid
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            // Initialize the list
            List<TestRow> orders = new List<TestRow>();

            // Add random words
            Random r = new Random();
            for (int i = 0; i < 10000; i++)
            {
                orders.Add(new TestRow() { String = GetRandomString(r, 25), Int = r.Next(), Double = r.NextDouble() });
            }

            // Set the data context
            this.DataContext = orders;
        }

        public string GetRandomString(Random rnd, int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder sb = new StringBuilder();
            while (length-- > 0)
                sb.Append(chars[(int)(rnd.NextDouble() * chars.Length)]);
            return sb.ToString();
        }
    }
}
