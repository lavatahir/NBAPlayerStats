using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace NBA_Player_Stats
{
    public partial class Form1 : Form
    {
        private string m_strSite;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(textBox1.Text))
            {
                WebRequest req = WebRequest.Create("http://www.nba.com/playerfile/"+textBox1.Text.Replace(" ","_").ToLower()+"/");
                StreamReader stream = new StreamReader(req.GetResponse().GetResponseStream());
                StringBuilder sb = new StringBuilder();
                string strLine;
                while((strLine = stream.ReadLine()) != null)
                {
                    if (strLine.Length > 0)
                    {
                        sb.Append(strLine);
                    }
                }
                stream.Close();
                m_strSite = sb.ToString();
                if(!m_strSite.Contains(textBox1.Text))
                {
                    MessageBox.Show("Player not found.","NBA Player Stats", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                }
                else
                {
                    textBox2.Text = ExtractValueFromTag("td class=\"PTS\"", "td");
                    textBox2.Text = ExtractValueFromTag("td class=\"REBS\"", "td");
                    textBox2.Text = ExtractValueFromTag("td class=\"AST\"", "td");
                    textBox2.Text = ExtractValueFromTag("h3 itemprop=\"affliation\" class=\"player-team\"", "h3");
                    try
                    {
                        pictureBox1.Load("http://i.cdn.turner.com/nba/nba/.element/img/2.0/sect/statscube/players/large"+textBox1.Text.Replace(" ","_").ToLower() + ".png");
                        pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize; //Stretch Image too
                    }
                    catch (Exception)
                    {
                        pictureBox1.Load("http://i.cdn.turner.com/nba/nba/.element/img/2.0/sect/statscube/players/large/default/nba_headshot_v2.png");
                        pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize; //Stretch Image too
                    }
                }
            }
        }
        private string ExtractValueFromTag(string openTag, string closeTag)
        {
            var startTag = "<" + openTag + ">";
            int startIndex = m_strSite.IndexOf(startTag) + startTag.Length;
            int endIndex = m_strSite.IndexOf("</" + closeTag + ">", startIndex);
            return m_strSite.Substring(startIndex, endIndex - startIndex);
        }
    }
}