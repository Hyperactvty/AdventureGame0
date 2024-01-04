using System;

using UnityEngine;
using System.Collections;

namespace GraphQL {
    public static class APIGraphQL {
        private const string ApiURL = "http://localhost:5000/graphql";
        public static string Token = null;

        public static bool LoggedIn {
            get { return !Token.Equals(""); } //todo: improve loggedin verification
        } 

        private static readonly GraphQLClient API = new GraphQLClient(ApiURL);

        public static void Query (string query, object variables = null, Action<GraphQLResponse> callback = null) {
            Debug.Log($"Doing Query... -> q: {query} \t vars: {variables}");
            API.Query(query, variables, callback, Token);
        }
    }
}