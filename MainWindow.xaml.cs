//Created by: Jordan Hurst
//Date: 10/21/2020
//This WPF client is used to interface with my WebAPI
//Allows user to input names to SQL Server database through API
//Allows user to read list of names from SQL Server database through API
//Allows user to delete individual names from SQL Server database through API
using System;
using System.Collections.Generic;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PersonInfoAPIClient
{
    public partial class MainWindow : Window
    {  
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44322/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var person = new Person
            {
                Id = ID.Text,
                First_Name = FirstName.Text,
                Last_Name = LastName.Text
            };

            var response = client.PostAsJsonAsync("api/Person", person).Result;

            if (response.IsSuccessStatusCode)
            {
                ID.Text = "";
                FirstName.Text = "";
                LastName.Text = "";
                MessageBox.Show("Person Added");
            }
            else
            {
                MessageBox.Show("Error Code " + response.StatusCode + " : Message - "
                    + response.ReasonPhrase);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44322/")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/Person").Result;

            if (response.IsSuccessStatusCode)
            {
                var person = response.Content.ReadAsAsync<IEnumerable<Person>>().Result;
                dataGrid.ItemsSource = person;                
            }
            else
            {
                MessageBox.Show("Error Code " + response.StatusCode + " : Message - "
                    + response.ReasonPhrase);
            }
        }

        private void DeleteID_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44322/")
            };
            var id = ID.Text.Trim();
            HttpResponseMessage response = client.DeleteAsync("api/Person/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("User Deleted");
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }
    }
}
