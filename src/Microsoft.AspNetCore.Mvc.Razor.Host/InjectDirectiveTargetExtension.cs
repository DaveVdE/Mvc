// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Evolution.CodeGeneration;

namespace Microsoft.AspNetCore.Mvc.Razor.Host
{
    public class InjectDirectiveTargetExtension : IInjectDirectiveTargetExtension
    {
        public void WriteInjectProperty(CSharpRenderingContext context, InjectDirectiveIRNode node)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var property = $"[global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]{Environment.NewLine}public {node.TypeName} {node.MemberName} {{ get; private set; }}";

            if (node.Source.HasValue)
            {
                using (context.Writer.BuildLinePragma(node.Source.Value))
                {
                    context.Writer.WriteLine(property);
                }
            }
            else
            {
                context.Writer.WriteLine(property);
            }
        }
    }
}
