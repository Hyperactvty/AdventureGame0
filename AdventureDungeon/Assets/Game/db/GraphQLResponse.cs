// using System;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using UnityEngine;

namespace GraphQL {
    public class GraphQLResponse {
        public string Raw { get; private set; }
        private readonly JObject data;
        public string Exception { get; private set; }

        public GraphQLResponse (string text, string ex = null) {
            Exception = ex;
            Raw = text;
            data = text != null ? JObject.Parse(text) : null;
        }

        public T Get<T> (string key) {
            // Debug.Log($"KEY > {key}");
            Debug.Log($"Get Data 2 (GraphQLResponse) -> {JObject.Parse(GetData().ToString())["data"][key]}"); // FIND WAY TO RETURN THIS!!!

            System.Type typeParameterType = typeof(T);
            string tokenName=""; bool selectMany = false;
            switch (typeParameterType.ToString())
            {
              case "PlayerStats": tokenName="playerStats"; break;
              case "ItemStats": tokenName="itemStats"; selectMany = true; break;
                // return JObject.Parse(GetData().ToString())["data"][key].SelectToken("$.playerStats").ToObject<T>();
                // PlayerStats _ps = JObject.Parse(GetData().ToString())["data"][key].SelectToken("$.playerStats").ToObject<PlayerStats>();
                // break;
              default:
                break;
            }
            return selectMany ? JObject.Parse(GetData().ToString())["data"][key].SelectToken($"$.{tokenName}").ToObject<T>() : JObject.Parse(GetData().ToString())["data"][key].SelectToken($"$.{tokenName}").ToObject<T>();
            // return selectMany ? JObject.Parse(GetData().ToString())["data"][key].SelectTokens($"$.{tokenName}").ToObject<T>() : JObject.Parse(GetData().ToString())["data"][key].SelectToken($"$.{tokenName}").ToObject<T>();

            // return GetData()[key].ToObject<T>();  // Original
        }

        public List<T> GetList<T> (string key) {
            return Get<JArray>(key).ToObject<List<T>>();
        }

        private JObject GetData () {
          // Debug.Log($"Data Result (GraphQLResponse) -> {data}");
            return data == null ? null : JObject.Parse(data.ToString());
            // return data == null ? null : JObject.Parse(data.ToString())["data"];
            // return data == null ? null : JObject.Parse(data["data"].ToString());
        }
    }
}