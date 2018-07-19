using Dapper;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using static Dapper.SqlMapper;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class BaseApiController : ApiController
    {
        internal TelemetryClient telemetryClient = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration(ConfigurationManager.AppSettings["ApplicationInsightsInstrumentationKey"]));
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;

        internal string GetTripleStoreId()
        {
            string id = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Api-Version", ConfigurationManager.AppSettings["ApiVersion"]);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["SubscriptionKey"]);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));
                    using (HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings["GenerateIdUrl"]).Result)
                    {
                        if (response.IsSuccessStatusCode)
                            telemetryClient.TrackTrace($"Problem with getting new id, status code is {response.StatusCode}", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error);
                        id = response.Content.ReadAsStringAsync().Result;
                    }
                }
                id = new Uri(id).Segments.Last();
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            return id;
        }

        internal string EMail
        {
            get
            {
                return "test";
                return (User.Identity as ClaimsIdentity)
                    ?.FindFirst(System.IdentityModel.Claims.ClaimTypes.Email).Value;
            }
        }

        internal List<T> GetItems<T>(CommandDefinition command) where T : new()
        {
            List<T> result = new List<T>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    result = connection.Query<T>(command)
                        .AsList<T>();
                }
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            return result;
        }

        internal Tuple<List<T>, List<K>> GetItems<T, K>(CommandDefinition command)
            where T : new()
            where K : new()
        {
            List<T> resultT = new List<T>();
            List<K> resultK = new List<K>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (GridReader grid = connection.QueryMultiple(command))
                    {
                        resultT = grid.Read<T>().ToList();
                        resultK = grid.Read<K>().ToList();
                    }
                }
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            return Tuple.Create<List<T>, List<K>>(resultT, resultK);
        }

        internal T GetItem<T>(CommandDefinition command) where T : new()
        {
            T result = new T();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    result = connection.Query<T>(command)
                        .SingleOrDefault();
                }
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            return result;
        }

        internal Tuple<T, List<K>> GetItem<T, K>(CommandDefinition command)
            where T : new()
            where K : new()
        {
            T resultT = new T();
            List<K> resultK = new List<K>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (GridReader grid = connection.QueryMultiple(command))
                    {
                        resultT = grid.Read<T>().SingleOrDefault();
                        resultK = grid.Read<K>().ToList();
                    }
                }
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
            }
            return Tuple.Create<T, List<K>>(resultT, resultK);
        }

        internal bool Execute(CommandDefinition command)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Execute(command);
                }
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
                return false;
            }
            return true;
        }

        internal bool Execute(CommandDefinition[] commands)
        {
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    foreach (CommandDefinition command in commands)
                        connection.Execute(command.CommandText, command.Parameters, transaction);
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                telemetryClient.TrackException(e);
                return false;
            }
            return true;
        }

    }
}