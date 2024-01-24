/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Meta.WitAi.Json;
using UnityEngine;

namespace Meta.WitAi.Composer.Data
{
    [Serializable]
    public class ComposerContextMap
    {
        public const string DYNAMIC_CONTEXT = "dynamic_context";
        
        // Project specific context data
        public WitResponseClass Data { get; private set; }
        
        [NonSerialized]
         private List<string> _runtimeDynamicContext = new List<string>();
         
        public ComposerContextMap()
        {
            Data = new WitResponseClass();
        }
        
        /// <summary>
        /// Constructs a context map from a Json based Wit Response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="errorBuilder"></param>
        public ComposerContextMap(WitResponseNode response, StringBuilder errorBuilder)
        {
            try
            {
                Data = response["context_map"].AsObject;
            }
            catch (Exception e)
            {
                errorBuilder.AppendLine($"Response Parse Failed\n{e.ToString()}");
            }
        }

        // Return true if key exists
        public bool HasData(string key) => Data != null && Data.HasChild(key);

        /// <summary>
        /// Get specific data from the context map
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>(string key, T defaultValue = default(T))
        {
            return Data.GetChild<T>(key, defaultValue);
        }
        
        /// <summary>
        /// Get specific data from the context map
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <typeparam name="T"></typeparam>
        public void SetData<T>(string key, T newValue)
        {
            // Ignore with invalid key
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            // Serialize into token
            Data[key] = JsonConvert.SerializeToken<T>(newValue);
        }

        /// <summary>
        /// Removes the given data from the context map
        /// </summary>
        /// <param name="key"></param>
        public void ClearData(string key)
        {
            // Ignore with invalid key
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            Data.Remove(key);
        }
        /// <summary>
        /// Add a dynamic context value to the set of dynamic context strings. This will be sent to the server as a set
        /// of text lines in a single context map field.
        /// </summary>
        /// <param name="context">The dynamic context value you want to set, generally transient descriptive state.</param>
        public void AddDynamicContext(string context)
        {
            _runtimeDynamicContext.Add(context);
            var dynamicContext = string.Join("\n", _runtimeDynamicContext);
            SetData(DYNAMIC_CONTEXT, dynamicContext);
        }

        /// <summary>
        /// Removes a line of context from the dynamic context state
        /// </summary>
        /// <param name="context"></param>
        public void RemoveDynamicContext(string context)
        {
            _runtimeDynamicContext.Remove(context);
            var dynamicContext = string.Join("\n", _runtimeDynamicContext);
            SetData(DYNAMIC_CONTEXT, dynamicContext);
        }

        /// <summary>
        /// Export the context map as a Json string
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            if (Data == null)
            {
                return "{}";
            }

            try
            {
                return Data.ToString();
            }
            catch (Exception e)
            {
                VLog.E($"Composer Context Map - Decode Failed\n{e}");
            }
            return "{}";
        }
    }
}
