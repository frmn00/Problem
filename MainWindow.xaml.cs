﻿using System;
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
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.IO;
using SQLite;


public class Goods
{
    private string name, decription, country;
    private int cost, id;

    [MaxLength(500), NotNull]
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    [MaxLength(50), NotNull]
    public string Country
    {
        get { return country; }
        set { country = value; }
    }

    [MaxLength(300), NotNull]
    public string Decription
    {
        get { return decription; }
        set { decription = value; }
    }

    [NotNull]
    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    [PrimaryKey, AutoIncrement, Unique]
    public int Id
    {
        get { return id; }
        set {  }
    } 
}



namespace problem
{
    public partial class MainWindow : Window
    {

        public void FillDb()
        {
            Random rnd = new Random(42);
            string[] names = File.ReadAllLines("name.txt");
            string[] countries = File.ReadAllLines("country.txt");
            System.Data.SQLite.SQLiteConnection.CreateFile("database.db");
            List<Goods> objs = new List<Goods>();
            int k = 100000;
            while (k != 0)
            {
                objs.Add(new Goods() { Name = names[rnd.Next(names.Length - 1)], Decription = "Описание", Cost = rnd.Next(2000), Country = countries[rnd.Next(countries.Length - 1)] });
                k--;
            }
            try
            {
                using (var db = new SQLite.SQLiteConnection("database.db"))
                {
                    db.CreateTable<Goods>();
                    db.InsertAll(objs);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("database.db"))
            {
                MessageBox.Show("База данных не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<Goods> products = new List<Goods>();
            using (var db = new SQLite.SQLiteConnection("database.db"))
            {
                products = db.Table<Goods>().ToList<Goods>();
            }
            LView.ItemsSource = products;
        }
    }
}