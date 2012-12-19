using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharp.oDesk.Api;
using CSharp.oDesk.Connect;
using Newtonsoft.Json.Linq;
using Spring.Json;
using Spring.Social.OAuth1;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Collections;
namespace LoginForm
{
    public class JobParserClass
    {
        private const string ODeskApiKey = "4102f6e4e47c2efeef0aaee5adefa7d6";
        private const string ODeskApiSecret = "7c62cd38dda6f115";
        private const string UrlSearchJobs = "https://www.odesk.com/api/profiles/v1/search/jobs.json?page=0;15&c1=Software Development&c2=Desktop Applications";
        private string UrlUserInfo = "https://www.odesk.com/api/profiles/v1/search/jobs.json?page=0;15&c1=Software Development&c2=Desktop Applications&";

        private OAuthToken _oauthToken = null;
        private oDeskServiceProvider _oDeskService = null;
        public OAuthToken _oauthAccessToken = null;

        public int NewJobs;
        public bool CanLogging = false;
        public delegate void MyOdeskDelegate();
        public delegate int MyOdeskDelegateOverloaded();
        public ListJobs timelyJobs = new ListJobs();
        public string pinCode = null;
        //---------------------------------------------------------------------------------
        //Вход в аккаунт
        public void SignIn()
        {
            OdeskOperation(new MyOdeskDelegate(OperationSignIn));
        }
        public void OperationSignIn()
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
            OdeskOperation(new MyOdeskDelegate(OperationConfirmLogin));
        }
        public void OperationConfirmLogin()
        {
            var requestToken = new AuthorizedRequestToken(_oauthToken, pinCode);
            _oauthAccessToken = _oDeskService.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
        }

        //---------------------------------------------------------------------------------
        //Получение списка работы
        public int SearchJobs()
        {
            return OdeskOperation(new MyOdeskDelegateOverloaded(OperationSearchJobs));
        }
        public int OperationSearchJobs()
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
                    Title = f["op_title"].ToString(),
                    Description = f["op_description"].ToString(),
                    Skills = f["op_required_skills"].ToString(),
                    County = f["op_country"].ToString(),
                    Type = f["job_type"].ToString(),
                    Amount = f["amount"].ToString()
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
