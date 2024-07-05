using System.Runtime.InteropServices;

using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Runtime;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;

namespace CompiledFileFetcherUI
{
    public partial class Form1 : Form
    {
        List<string> ClientList = new List<string> { "BrysonUK", "SuezUK", "SuezAUS", "Cardiff", "Cleanaway", "VeoliaAUS", "VeoliaUK", "VeoliaFR" };

        //INIT
        public Form1()
        {
            InitializeComponent();

            //Populate Client Filter
            foreach (String client in ClientList)
            { Clients.Items.Add(client); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        // Handle Fetch Button Click
        private async void Fetch_Click(object sender, EventArgs e) { await Program2.Main(DropDown.Text, GetLatestDev.Checked); }

        // Handle Dev CheckBox Changed
        private void GetLatestDev_CheckedChanged(object sender, EventArgs e) { }
        
        //Handle Client Selected
        private async void Clients_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Start Fresh
            DropDown.SelectedItem = "";
            DropDown.Text = "Contract";
            DropDown.Items.Clear();

            //Populate Contracts
            List<string> BehaviorList = await Program2.GetDefaultLink();

            foreach (String contract in BehaviorList)
            {
                string pContract = contract.ToLower();
                string pClient = Clients.SelectedItem.ToString().ToLower();

                if (pContract.Contains(pClient) || (pClient.Contains("suezuk") && pContract.Contains("maldon")))
                    { DropDown.Items.Add(contract);
                }
            }
        }
    }


    class Program2
    {
        static public async Task<List<string>> GetDefaultLink()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {  // Fetch HTML content asynchronously
                    HttpResponseMessage response = await client.GetAsync("http://devapp3.echo.services/packages/");

                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the HTML content
                        string htmlContent = await response.Content.ReadAsStringAsync();

                        // Find the Behaviour Links with '.zip'
                        string behaviourRegex = @"(?<=<a href=.)([a-z]+(Behaviours))(?=\/)";
                        MatchCollection matches = Regex.Matches(htmlContent, behaviourRegex, RegexOptions.IgnoreCase);

                        // Create the List of Options
                        List<string> behaviorLinks = new List<string>();
                        foreach (Match match in matches) { behaviorLinks.Add(match.ToString()); }

                        // Manually Add This Straggler 
                        behaviorLinks.Add("MaldonWebsite");

                        return behaviorLinks;
                    }
                    else
                    {

                        MessageBox.Show("Please Check Your Connection & VPNs and Try Again");
                    }

                }
                catch (Exception ex) { Console.WriteLine($"An error occurred: {ex.Message}"); } // Print any exception that occurred
            }
            return new List<string>();
        }

        public static async Task Main(String category, Boolean devOnly)
        {
            //  String category = await GetDefaultLink();

            //URL of the website you want to fetch HTML from
            string url = "http://devapp3.echo.services/packages/" + category + "/";
            // Console.WriteLine(url);

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Fetch HTML content asynchronously
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content as a string
                        string htmlContent = await response.Content.ReadAsStringAsync();

                        // Extract URLs and their corresponding dates using regular expressions
                        string pattern = @"((?<=<a href=.)Deploy_.+zip(?=.>))(?:.+)((?<=\s)[0-9]{2}-[a-z]{3}-[0-9]{4}\s[0-9]{2}:[0-9]{2})";
                        Dictionary<DateTime, string> zipFiles = new Dictionary<DateTime, string>();
                        DateTime latestDate = DateTime.MinValue;
                        string latestLink = "";

                        MatchCollection builds = Regex.Matches(htmlContent, pattern, RegexOptions.IgnoreCase);

                        //Find the Latest Build
                        foreach (Match match in builds)
                        {
                            if (devOnly && (!match.Value.Contains("fef260e9") && !match.Value.Contains("f9c24f45"))) { continue; } //Skip Non-Dev Branches

                            DateTime testDate = DateTime.ParseExact(match.Groups[2].Value, "dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                            if (testDate >= latestDate)
                            {
                                latestDate = testDate;
                                latestLink = match.Groups[1].Value.ToString();
                            }
                        }

                        //Message to User about Found Builds
                        if (latestDate.Date != DateTime.Now.Date)
                        {
                            MessageBox.Show("Downloading Build: " + latestLink + Environment.NewLine + "Built On: " + latestDate);
                        }

                        //AutoDownload
                        System.Diagnostics.Process.Start("explorer.exe", url + latestLink);

                        //Diags
                        //Console.WriteLine(greatestDate + " UTC");
                        // Console.WriteLine(latestLink);


                    }
                }
                catch (Exception ex) { MessageBox.Show("An error occurred: " + ex.Message); }  // Print any exception that occurred
            }
        }


    }
}