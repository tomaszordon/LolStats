using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
using System.Xml.Linq;

namespace LolStats
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            CloseButton.Click += (s, e) => Close();
        }

        private async void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            string summonerName = SummonerName.Text;
            string region = RegionComboBox.Text;

            string apiKey = ConfigurationManager.AppSettings["RiotApiKey"].ToString();

            if (!String.IsNullOrEmpty(summonerName))
            {
                if (region == "EUNE")
                    region = "eun1.api.riotgames.com";
                if (region == "EUW")
                    region = "euw1.api.riotgames.com";
                if (region == "NA")
                    region = "americas.api.riotgames.com";

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("https://" + region + "/lol/summoner/v4/summoners/by-name/" + summonerName + "?api_key=" + apiKey);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseText = await response.Content.ReadAsStringAsync();
                        dynamic data = JObject.Parse(responseText);
                        string temp = data.name;
                        string summonerLevel = data.summonerLevel;
                        Trace.WriteLine(temp);
                        Trace.WriteLine("Poziom gracza: " + summonerLevel);   
                    }
                    else
                        Trace.WriteLine("Error API");
                }     
            }

        }
    }
}
