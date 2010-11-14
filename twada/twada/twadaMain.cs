using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace twada
{
    class twadaMain
    {
        static public string UserArg;

        static void Main(string[] args)
        {
            var UserOutput = Console.Out;
            Console.ForegroundColor = ConsoleColor.Blue;Console.WriteLine("twada, a vague Twitter console application...");
            Console.Clear();
            if (args.Length > 0)
            {
                if (args[0] == "-public")
                {
                    UserOutput.WriteLine("Retrieving public timeline...");
                    var ThreadStart = new System.Threading.ThreadStart(GetTwitterPublic);
                    var BeginGet = new System.Threading.Thread(ThreadStart);
                    BeginGet.Start();
                }
                if (args[0].StartsWith("@"))
                {
                    UserArg = args[0].Substring(1);
                    UserOutput.WriteLine("Retrieving " + UserArg + "'s timeline...");
                    var ThreadStart = new System.Threading.ThreadStart(GetTwitterUser);
                    var BeginGet = new System.Threading.Thread(ThreadStart);
                    BeginGet.Start();
                }
            }
            else
            {
                UserOutput.WriteLine("twada started with no arguments...");
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            
        }

        //ThreadStart void-function to get the "public_timeline" response of the Twitter API
        static public void GetTwitterPublic()
        {

            var twRequest = System.Net.HttpWebRequest.Create("http://api.twitter.com/1/statuses/public_timeline.xml");
            twRequest.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Anonymous;
            twRequest.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;

            var twDocument = new XmlDocument();
            twDocument.LoadXml(new System.IO.StreamReader(twRequest.GetResponse().GetResponseStream()).ReadToEnd());
            int i;
            var Statuses = GetStatuses(twDocument);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            for (i = 0; i != Statuses.Length ; i += 1)
            {
                Console.Out.WriteLine(Statuses[i]);
            }

            twRequest = null;
            twDocument = null;
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        static public void GetTwitterUser()
        {

            var twRequest = System.Net.HttpWebRequest.Create("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + UserArg);
            twRequest.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Anonymous;
            twRequest.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;

            var twDocument = new XmlDocument();
            twDocument.LoadXml(new System.IO.StreamReader(twRequest.GetResponse().GetResponseStream()).ReadToEnd());
            int i;
            var Statuses = GetStatuses(twDocument);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            for (i = 0; i != Statuses.Length; i += 1)
            {
                Console.Out.WriteLine(Statuses[i]);
            }

            twRequest = null;
            twDocument = null;
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        //Function to parse statuses into an array of string variables
        static public string[] GetStatuses(XmlDocument XmlResponse)
        {
            var ListString = new List<string>();
            var twDocument = XmlResponse;
            int i;
            for (i = 0; i != twDocument.SelectNodes("//statuses/status").Count; i += 1)
            {
                ListString.Add(twDocument.SelectNodes("//statuses/status")[i]["text"].InnerText);
            }
            return ListString.ToArray();
        }
    }
}
