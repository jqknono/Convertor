using System;
using System.Collections.Generic;
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
using System.Windows.Forms;

namespace HouseKeeper
{
	using JLibrary.Utilities;
	using System.Globalization;
	using System.Reflection;
	using System.Resources;
	using WPF = System.Windows.Controls;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string exeName = "Automation4ID";
		public MainWindow()
		{
			InitializeComponent();

			string s = LocalizationHelper.GetString("Edit");

		}

		private void Textbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			WPF.TextBox tb = sender as WPF.TextBox;
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			System.Windows.Forms.DialogResult result = fbd.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				tb.Text = fbd.SelectedPath;
			}
		}

		private void Textbox_Drop(object sender, System.Windows.DragEventArgs e)
		{
			WPF.TextBox tb = sender as WPF.TextBox;
			if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
				tb.Text = files[0];
			}
		}

		private void Textbox_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
		{
			e.Handled = true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			// input
			string arguments = string.Empty;
			if (!string.IsNullOrWhiteSpace(this.Input_Textbox.Text))
			{
				arguments += " -i " + this.Input_Textbox.Text;
			}
			else
			{
				System.Windows.MessageBox.Show("None input.");
				return;
			}
			if (this.Input_Textbox.Text.Contains(' '))
			{
				System.Windows.MessageBox.Show("Error:path contains space.");
				return;
			}

			// output
			if (!string.IsNullOrWhiteSpace(this.Output_Textbox.Text))
			{
				arguments += " -o " + this.Output_Textbox.Text;
			}
			if (this.Output_Textbox.Text.Contains(' '))
			{
				System.Windows.MessageBox.Show("Error:path contains space.");
				return;
			}

			// ini
			if (!string.IsNullOrWhiteSpace(this.Ini_Textbox.Text))
			{
				arguments += " -ini " + this.Ini_Textbox.Text;
			}
			else
			{
				System.Windows.MessageBox.Show("None ini.");
				return;
			}
			if (this.Ini_Textbox.Text.Contains(' '))
			{
				System.Windows.MessageBox.Show("Error:path contains space.");
				return;
			}

			// log
			if (!string.IsNullOrWhiteSpace(this.LogPath_Textbox.Text))
			{
				arguments += " -l " + this.LogPath_Textbox.Text;
			}
			if (this.LogPath_Textbox.Text.Contains(' '))
			{
				System.Windows.MessageBox.Show("Error:path contains space.");
				return;
			}

			try
			{
				System.Diagnostics.Process.Start(exeName, arguments);
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show(ex.Message);
			}
		}
	}
}
