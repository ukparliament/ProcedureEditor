﻿using Dapper;
using Parliament.ProcedureEditor.Web.Api.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Parliament.ProcedureEditor.Web.Api
{
    public class BaseApiController : ApiController
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;

        internal HtmlContentActionResult RenderView(string viewName, object model = null)
        {
            return new HtmlContentActionResult(Request, ControllerContext.ControllerDescriptor.ControllerName, viewName, model);
        }

        internal string GetTripleStoreId()
        {
            string id = null;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-Version", ConfigurationManager.AppSettings["ApiVersion"]);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ConfigurationManager.AppSettings["SubscriptionKey"]);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));
                using (HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings["GenerateIdUrl"]).Result)
                {
                    id = response.Content.ReadAsStringAsync().Result;
                }
            }
            id = new Uri(id).Segments.Last();
            return id;
        }

        internal string EMail
        {
            get
            {
                return User.Identity.Name;
            }
        }

        internal List<T> GetItems<T>(CommandDefinition command) where T : new()
        {
            List<T> result = new List<T>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<T>(command)
                    .AsList<T>();
            }
            return result;
        }

        internal Tuple<List<T>, List<K>> GetItems<T, K>(CommandDefinition command)
            where T : new()
            where K : new()
        {
            List<T> resultT = new List<T>();
            List<K> resultK = new List<K>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlMapper.GridReader grid = connection.QueryMultiple(command))
                {
                    resultT = grid.Read<T>().ToList();
                    resultK = grid.Read<K>().ToList();
                }
            }
            return Tuple.Create<List<T>, List<K>>(resultT, resultK);
        }

        internal T GetItem<T>(CommandDefinition command) where T : new()
        {
            T result = new T();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<T>(command)
                    .SingleOrDefault();
            }
            return result;
        }

        internal Tuple<T, List<K>> GetItem<T, K>(CommandDefinition command)
            where T : new()
            where K : new()
        {
            T resultT = new T();
            List<K> resultK = new List<K>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlMapper.GridReader grid = connection.QueryMultiple(command))
                {
                    resultT = grid.Read<T>().SingleOrDefault();
                    resultK = grid.Read<K>().ToList();
                }
            }
            return Tuple.Create<T, List<K>>(resultT, resultK);
        }

        internal bool Execute(CommandDefinition command)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(command);
            }
            return true;
        }

        internal bool Execute(CommandDefinition[] commands)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    foreach (CommandDefinition command in commands)
                        connection.Execute(command.CommandText, command.Parameters, transaction);
                    transaction.Commit();
                }

            }
            return true;
        }

    }
}