/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */
using System;

namespace Meta.WitAi.Attributes
{
    /// <summary>
    /// An attribute to show a Button in the inspector for a method in a MonoBehaviour script.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public readonly string displayName;

        public ButtonAttribute(string displayName = null)
        {
            this.displayName = displayName;
        }
    }
}
