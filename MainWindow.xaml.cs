using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CSharp.oDesk.Api;
using CSharp.oDesk.Connect;
using Newtonsoft.Json.Linq;
using Spring.Json;
using Spring.Social.OAuth1;

namespace LoginForm
{
    public partial class MainWindow : Window
    {
        private const string ODeskApiKey = "4102f6e4e47c2efeef0aaee5adefa7d6";
        private const string ODeskApiSecret = "7c62cd38dda6f115";
        private const string UrlSearchJobs = "https://www.odesk.com/api/profiles/v1/search/jobs.json?page=0;5&c1=Software Development&c2=Desktop Applications";
        private string UrlUserInfo = "https://www.odesk.com/api/profiles/v1/search/jobs.json?page=0;5&c1=Software Development&c2=Desktop Applications&";

        private OAuthToken _oauthToken = null;
        private oDeskServiceProvider _oDeskService = null;
        public OAuthToken _oauthAccessToken = null;

        public int NewJobs;
        private bool CanLogging = false;
        public delegate void MyOdeskDelegate();
        public delegate int MyOdeskDelegateOverloaded();
        private ListJobs timelyJobs = new ListJobs();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void DragMove(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (this.Width + e.HorizontalChange > 10)
                this.Width += e.HorizontalChange;
            if (this.Height + e.VerticalChange > 10)
                this.Height += e.VerticalChange;
        }

        private void Icon_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var sb = (Storyboard) this.FindResource("FadeOutStoryboard");
            sb.Begin();

        }
        private void FadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            Close();
        }
        private void ButtonActionStoryboard_Completed(object sender, EventArgs e)
        {
            CanLogging = true;
            SignIn();
        }
        private void ButtonAction_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!CanLogging)
            {
                var sb = (Storyboard) this.FindResource("ButtonActionStoryboard");
                sb.Begin();
                sb = (Storyboard) this.FindResource("ActionLabelStoryboard");
                sb.Begin();
                sb = (Storyboard) this.FindResource("ActionTextBoxStoryboard");
                sb.Begin();

                return;
            }
         
            ConfirmLogin();

            if (_oauthAccessToken == null)
            {
                WhatAction.Content = "The verification code is incorrect.";
                return;
            }

            MessageBox.Show(SearchJobs().ToString());
        }
        //---------------------------------------------------------------------------------
        //Вход в аккаунт
        public void SignIn()
        {
            _oDeskService = new oDeskServiceProvider(ODeskApiKey, ODeskApiSecret);
            _oauthToken = _oDeskService.OAuthOperations.FetchRequestTokenAsync(String.Empty, null).Result;
            var authenticateUrl = _oDeskService.OAuthOperations.BuildAuthorizeUrl(_oauthToken.Value, null);
            Process.Start(authenticateUrl);
        }
        //---------------------------------------------------------------------------------
        //Верификация временного токена и получение постонного
        public void ConfirmLogin()
        {
            var pinCode = WhatActionValue.Text.Trim(' ');
            var requestToken = new AuthorizedRequestToken(_oauthToken, pinCode);
            _oauthAccessToken = _oDeskService.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
        }
        //---------------------------------------------------------------------------------
        //Получение списка работы
        public int SearchJobs()
        {
            var oDesk = _oDeskService.GetApi(_oauthAccessToken.Value, _oauthAccessToken.Secret);

            return oDesk.RestOperations.GetForObjectAsync<JsonValue>(UrlSearchJobs).ContinueWith(
                task => ParseJsonResult(task.Result.ToString()
                            )).Result;
        }
        //---------------------------------------------------------------------------------
        //Общий метод
        public void OdeskOperation(MyOdeskDelegate useApiMethod)
        {
            try
            {
                useApiMethod();
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is oDeskApiException)
                    {
                        MessageBox.Show(ex.Message);
                        return true;
                    }
                    return false;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //---------------------------------------------------------------------------------
        //Общий метод 2
        public int OdeskOperation(MyOdeskDelegateOverloaded useApiMethod)
        {
            try
            {
                return useApiMethod();
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is oDeskApiException)
                    {
                        MessageBox.Show(ex.Message);
                        return true;
                    }
                    return false;
                });
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        //---------------------------------------------------------------------------------
        //Разбор результат с одеска
        public int ParseJsonResult(string json)
        {
            var o = JObject.Parse(json);
            var totalInfo = (JObject)o["jobs"];
            var jobsInfo = (JArray)totalInfo["job"];

            var tempTimelyJobs = new ListJobs();

            foreach (var f in jobsInfo)
            {
                tempTimelyJobs.AddJob(new JobsInfo()
                {
                    Title       = f["op_title"].ToString(),
                    Description = f["op_description"].ToString(),
                    Skills      = f["op_required_skills"].ToString(),
                    County      = f["op_country"].ToString(),
                    Type        = f["job_type"].ToString(),
                    Amount      = f["amount"].ToString()
                });
            }

            if (!timelyJobs.Any())
            {
                timelyJobs = tempTimelyJobs;
                return timelyJobs.Count();
            }

            int newJobs = 0;
            if (!tempTimelyJobs.Equals(timelyJobs))
            {
                for (int i = 0; i < tempTimelyJobs.Count(); i++)
                    if (tempTimelyJobs.At(i).Title != timelyJobs.At(i).Title)
                        newJobs++;

                timelyJobs = tempTimelyJobs;
            }

            NewJobs = newJobs > 0 ? newJobs : 0;

            return newJobs;
        }
        //---------------------------------------------------------------------------------
        //Разбор результат с одеска
        public void FirstLoadResultWithoutUrl(string json)
        {
            var o = JObject.Parse(File.ReadAllText(json));
            var totalInfo = (JObject)o["jobs"];
            var jobsInfo = (JArray)totalInfo["job"];

            timelyJobs.Clear();
            foreach (var f in jobsInfo)
            {
                timelyJobs.AddJob(new JobsInfo()
                {
                    Title = f["op_title"].ToString(),
                    Description = f["op_description"].ToString(),
                    Skills = f["op_required_skills"].ToString(),
                    County = f["op_country"].ToString(),
                    Type = f["job_type"].ToString(),
                    Amount = f["amount"].ToString()
                });
            }
        }
        public int UpdateListJobs()
        {
            if (_oauthAccessToken == null)
                return 0;
            return OdeskOperation(new MyOdeskDelegateOverloaded(SearchJobs));
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            OdeskOperation(new MyOdeskDelegate(SignIn));
            //FirstLoadResult(File.ReadAllText(@"PathToFile"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OdeskOperation(new MyOdeskDelegate(ConfirmLogin));

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(OdeskOperation(new MyOdeskDelegateOverloaded(SearchJobs)).ToString());
            //ParseJsonResult(File.ReadAllText(@""));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            timelyJobs.RemoveLast();
        }


    }

    //---------------------------------------------------------------------------------
    public class ListJobs : IEnumerable<JobsInfo>
    {
        private List<JobsInfo> Jobs = new List<JobsInfo>();

        public ListJobs() { }
        public ListJobs(List<JobsInfo> jobs)
        {
            this.Jobs = jobs;
        }
        public IEnumerator<JobsInfo> GetEnumerator()
        {
            return Jobs.GetEnumerator();
        }
        public void AddJob(JobsInfo job)
        {
            Jobs.Add(job);
        }
        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is ListJobs))
                return false;
            else
                return Jobs == ((ListJobs)obj).Jobs;
        }
        public override int GetHashCode()
        {
            return Jobs.GetHashCode();
        }
        public void Clear()
        {
            Jobs.Clear();
        }
        //TODO неверно срабатывает исключение
        public JobsInfo At(int id)
        {
            if (id < Jobs.Count)
                return Jobs[id];
            else
            {
                return new JobsInfo();
            }
        }
        public int Count()
        {
            return Jobs.Count;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void RemoveLast()
        {
            Jobs.RemoveAt(Jobs.Count - 1);
        }
    }
    //---------------------------------------------------------------------------------
    public class JobsInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Skills { get; set; }
        public string County { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
    }
    //---------------------------------------------------------------------------------
}