// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml.Internal
{
    /// <summary>
    /// A <see cref="IConfigureOptions{TOptions}"/> implementation which will add the
    /// data contract serializer formatters to <see cref="MvcOptions"/>.
    /// </summary>
    public class MvcXmlDataContractSerializerMvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly ILoggerFactory _loggerFactory;

        public MvcXmlDataContractSerializerMvcOptionsSetup(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Adds the data contract serializer formatters to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="MvcOptions"/>.</param>
        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new DataMemberRequiredBindingMetadataProvider());

            var outputFormatterLogger = _loggerFactory.CreateLogger<XmlDataContractSerializerOutputFormatter>();

            options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter(outputFormatterLogger));
            options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(options.SuppressInputFormatterBuffering));

            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider("System.Xml.Linq.XObject"));
            options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider("System.Xml.XmlNode"));
        }
    }
}
