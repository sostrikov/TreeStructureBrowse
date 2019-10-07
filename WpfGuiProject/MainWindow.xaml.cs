
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

//Добавляем ссылки на нужные нам пространства имён
using Linq2Sql;
using System.Xml.Linq;

namespace WpfGuiProject
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		XElement xml;
		public MainWindow()
		{
			InitializeComponent();
			xml = XElement.Load(@"..\..\XMLFile1.xml");
		}

		//В этом методе заключается вся логика по указанию источника данных, которые следует отображать в окне
		private void Change_DataSource(object sender, RoutedEventArgs e)
		{
			if (rbDatabase.IsChecked == true) //В качестве источника данных выбрана база данных
			{
				listBooks.ItemTemplate = null;
				treeStructure.ItemsSource = new MyTestDbDataContext().Categories.Where(n => n.ParrentCategoryId == null);
				treeStructure.ItemTemplate = (HierarchicalDataTemplate)FindResource("key1");
				//Настраиваем привязку примечаний
				DescriptionBinding(selectedNodeDescription, "SelectedItem.Description", treeStructure);
				DescriptionBinding(selectedBookDescription, "SelectedItem.Description", listBooks);
				//Настраиваем привязку отображения книг
				Binding bind = new Binding("SelectedItem.Books") { Source = treeStructure };
				listBooks.SetBinding(ItemsControl.ItemsSourceProperty, bind);
			}
			else //В качестве источника данных выбран xml-файл
			{
				treeStructure.ItemsSource = xml.Elements("Category");
				treeStructure.ItemTemplate = (HierarchicalDataTemplate)FindResource("key2");
				//Настраиваем привязку примечаний
				DescriptionBinding(selectedNodeDescription, "SelectedItem.Attribute[Description].Value", treeStructure);
				DescriptionBinding(selectedBookDescription, "SelectedItem.Attribute[Description].Value", listBooks);
				//Настраиваем привязку отображения книг
				listBooks.ItemTemplate = (DataTemplate)FindResource("key3");
				Binding bind = new Binding("SelectedItem.Elements[Book]") { Source = treeStructure };
				listBooks.SetBinding(ItemsControl.ItemsSourceProperty, bind);

			}
		}

		/// <summary>
		/// Настройка привязки отображения данных
		/// </summary>
		/// <param name="textBlock">Текстовый объект, который должен отображать текст примечания</param>
		/// <param name="pathValue">значение Path привязки</param>
		/// <param name="source">ссылка на объект-источник, из которого считываются данные через свойство Path</param>
		void DescriptionBinding(TextBlock textBlock, string pathValue, Control source)
		{
			textBlock.SetBinding(TextBlock.TextProperty, new Binding(pathValue) { Source = source });
		}
	}
}
