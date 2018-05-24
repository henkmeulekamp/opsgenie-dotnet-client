using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using OpsGenieApi.Model;

namespace OpsGenieApi.UnitTests
{
    public static class Extensions
    {
        public static object ToJson(this object obj)
        {
            return obj != null ? new MyPreferedJsonizer().SerializeToString(obj) : string.Empty;
        }
    }
    
    [TestFixture]
    public class OpsGenieClientTests
    {

        private OpsGenieClient CreateClient()
        {
            return new OpsGenieClient(new OpsGenieClientConfig
            {
                ApiKey = ConfigurationManager.AppSettings["OpsGenieApiKey"]
            }, new MyPreferedJsonizer());
        }


        [Test]
        public void CheckSetting()
        {
            var apikey = ConfigurationManager.AppSettings["OpsGenieApiKey"];

            Assert.IsNotNullOrEmpty(apikey,"Please set apikey in config");
            Assert.That(apikey, Is.Not.SameAs("Your-Secret-Api-Key"), "Please replace default api key");
        }

        [Test]
        public void GetLast()
        {
            var client = CreateClient();

            var response = client.GetLastOpenAlerts();
            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.requestId != null);
        }


        [Test]
        public void Raise()
        {
            var client = CreateClient();

            var response = client.Raise(new Alert
            {
                Alias = Guid.NewGuid().ToString(),
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);
        }


        [Test]
        public void RaisetoTeam()
        {
            var client = CreateClient();

            var response = client.Raise(new Alert
            {
                Alias = new Guid().ToString(),
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing team api",
                Note = "Just kill this alert..",
                Teams = new List<string> { "Henk J Meulekamp"}

            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);
        }


        [Test]
        public void CloseByAlertId()
        {
            var client = CreateClient();

            var response = client.Raise(new Alert
            {
                Alias = Guid.NewGuid().ToString(),
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api - CloseByAlertId",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);
            //give them some time to compleet
            Thread.Sleep(TimeSpan.FromMilliseconds(250));
            var alert = client.GetStatus(response.requestId);

            Assert.IsTrue(alert.data != null && alert.data.success);
            
            var responseClose = client.Close(alert.data.alertId, null, "Closing alert, all ok");

            Assert.IsTrue(responseClose);
            
       }

        [Test]
        public void CloseByAlias()
        {
            var client = CreateClient();

            string alias = Guid.NewGuid().ToString();
            var response = client.Raise(new Alert
            {
                Alias = alias,
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api - CloseByAlertId",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok);

            var responseClose = client.Close(null, alias, "Closing alert, all ok");

            Assert.IsTrue(responseClose);

        }



        [Test]
        public void AckAndClose()
        {
            var client = CreateClient();
            var alias = Guid.NewGuid().ToString();
            var response = client.Raise(new Alert
            {
                Alias = alias,
                Description = "Unittest alert",
                Source = "Developer",
                Message = "Testing api - CloseByAlertId",
                Note = "Just kill this alert.."
            });

            Trace.WriteLine(response.ToJson());
            Assert.IsTrue(response.Ok, "Should succesfully create new alert");

            var responseAck = client.Acknowledge(null, alias, "Ack, working on it");
            
            Assert.IsTrue(responseAck, "Should succesfully ackowledge");

            var responseClose = client.Close(null, alias, "Closing alert, all ok");

            Assert.IsTrue(responseClose, "Should succesfully close");

        }


        //Code example readme
        //public void x()
        //{
        //    var opsClient = new OpsGenieClient(new OpsGenieClientConfig
        //    {
        //        ApiKey = ".. your api key",
        //        ApiUrl = "https://api.opsgenie.com/v1/json/alert"
        //    });

        //    var response = opsClient.Raise(new Alert {Alias = "alert2", Source = "Test", Message = "All systems down"});

        //    if (response.Ok)
        //    {
        //        var respAck =  opsClient.Acknowledge(response.AlertId, null, "Working on it!");

        //        var respClose = opsClient.Close(response.AlertId, null, "Fixed by ..");
        //    }
        //}
    }
}
