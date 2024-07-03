using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace CompiledFileFetcherUI
{
    public partial class Form1 : Form
    {

        //INIT
        public Form1(){InitializeComponent();}

        private async void Form1_Load(object sender, EventArgs e)
        {
            List<string> listTest = await Program2.GetDefaultLink();
            
            
            AllocConsole();

            foreach (String item in listTest) {DropDown.Items.Add(item);}
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        static extern bool AllocConsole();

        // Handle Fetch Button Click
        private async void Fetch_Click(object sender, EventArgs e) {await Program2.Main(DropDown.Text, GetLatestDev.Checked);}

        // Handle Dev CheckBox Changed
        private void GetLatestDev_CheckedChanged(object sender, EventArgs e) {}
    }


    class Program2
    {
        public static async Task Main(String category, Boolean devOnly)
        {
            //String category = await GetDefaultLink();

            //URL of the website you want to fetch HTML from
            string url = "http://devapp3.echo.services/packages/" + category + "/";
            Console.WriteLine(url);

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

                        // Regular expression pattern to match href attributes containing '.zip'
                        //string pattern = "<a\\s+(?:[^>]*?\\s+)?href=\"([^\"]*\\.[^\"]*)\"";
                        // ;

                        // Extract URLs and their corresponding dates using regular expressions
                        Dictionary<DateTime, string> zipFiles = new Dictionary<DateTime, string>();
                        string pattern = @"((?<=<a href=.)Deploy_.+zip(?=.>))(?:.+)((?<=\s)[0-9]{2}-[a-z]{3}-[0-9]{4}\s[0-9]{2}:[0-9]{2})";
                        MatchCollection matches = Regex.Matches(htmlContent, pattern, RegexOptions.IgnoreCase);

                        string pattern2 = @"\d{2}-\w{3}-\d{4} \d{2}:\d{2}";
                        MatchCollection matches2 = Regex.Matches(htmlContent, pattern2, RegexOptions.IgnoreCase);
                        foreach (Match match2 in matches2)
                        {
                            match2.Value.ToString();
                            //Console.WriteLine(match2.Value.ToString());
                        }

                        int maxLinks = matches.Count;
                        int x = 1;
                        string latestLink = "";
                        DateTime greatestDate = DateTime.MinValue;

                        while (x < matches.Count)
                        {
                            //Console.WriteLine(matches[x].Value.ToString());
                            DateTime currentDate = DateTime.Parse(matches2[x - 1].Value.ToString());
                            if (currentDate > greatestDate)
                            {
                                if (matches[x].Value.ToString().Contains(".zip"))
                                {
                                    if (devOnly)
                                    {
                                        if (matches[x].Value.ToString().Contains("fef260e9") || matches[x].Value.ToString().Contains("f9c24f45"))
                                        {
                                            greatestDate = currentDate;
                                            latestLink = matches[x].Value.ToString();
                                        }

                                    }
                                    else
                                    {
                                        greatestDate = currentDate;
                                        latestLink = matches[x].Value.ToString();
                                    }
                                }
                            }
                            x++;
                        }

                        int length = latestLink.Length;
                        latestLink = latestLink.Substring(9, length - 10);

                        //Diags
                        Console.WriteLine(greatestDate + " UTC");
                        Console.WriteLine(latestLink);
                        System.Diagnostics.Process.Start("explorer.exe", url + latestLink);

                    }
                }
               catch (Exception ex) {Console.WriteLine($"An error occurred: {ex.Message}"); }  // Print any exception that occurred
            }
        }

        static public async Task<List<string>> GetDefaultLink()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Fetch HTML content asynchronously
                    HttpResponseMessage response = await client.GetAsync("http://devapp3.echo.services/packages/");

                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the content as a string
                        string htmlContent = await response.Content.ReadAsStringAsync();

                        // Regular expression pattern to match href attributes containing '.zip'
                        string pattern = @"(?<=<a href=.)([a-z]+(Behaviours))(?=\/)";

                        MatchCollection matches = Regex.Matches(htmlContent, pattern, RegexOptions.IgnoreCase);
                        List<string> compiledCategories = new List<string>();

                        foreach (Match match in matches)
                        {
                            compiledCategories.Add(match.ToString());
                        }
                        compiledCategories.Add("MaldonWebsite");
                        return compiledCategories;
                    }
                }
                catch (Exception ex) {Console.WriteLine($"An error occurred: {ex.Message}");} // Print any exception that occurred
            }
          return new List<string>();
        }
    }
}