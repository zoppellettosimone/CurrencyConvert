using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurrencyConverterForm
{
    public partial class CurrencyConverter : Form
    {
        //Chiamata Api
        string apiUrl = "https://open.er-api.com/v6/latest/USD";

        JObject apiRes;

        public CurrencyConverter()
        {
            InitializeComponent();

            timeLabel.Text += " " + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

            //Chiamata Api
            Uri address = new Uri(apiUrl);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "text/json";

            //Stringa Json dalla chiamata Api
            string jsonString = "";

            //Salvataggio su "jsonString" del Json String
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                jsonString = reader.ReadToEnd();
            }

            //Conversione da Json String a Json
            apiRes = JObject.Parse(jsonString);

            //Lista di tutte le Valute disponibili
            List<string> allValues = apiRes["rates"].Select(s => new string(s.ToString().Substring(1,3))).ToList();

            fromComboBox.Items.AddRange(allValues.ToArray());
            fromComboBox.SelectedIndex = 0;
            fromValuesLabel.Text = apiRes["rates"][fromComboBox.Text].ToString();

            toComboBox.Items.AddRange(allValues.ToArray());
            toComboBox.SelectedIndex = 0;
            toValuesLabel.Text = apiRes["rates"][toComboBox.Text].ToString();
        }

        private void UserBotton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            decimal valueFrom = decimal.Parse(fromValuesLabel.Text);
            decimal valueTo = decimal.Parse(toValuesLabel.Text);

            decimal numberToConvert = valuesNumericUpDown.Value;

            //Calcolo Conversione
            decimal resultConvert = Math.Round((numberToConvert * valueTo / valueFrom), 2);

            resultLabel.Text = resultConvert.ToString();
            resultLabel.Visible = true;
        }

        private void fromComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fromValuesLabel.Text = apiRes["rates"][fromComboBox.Text].ToString();
        }

        private void toComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            toValuesLabel.Text = apiRes["rates"][toComboBox.Text].ToString();
        }

        private void linkLabel_LinkClicked(object sender, EventArgs e)
        {
            Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", "https://open.er-api.com");
        }
    }
}
