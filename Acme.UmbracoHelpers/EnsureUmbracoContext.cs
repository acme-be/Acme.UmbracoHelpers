// -----------------------------------------------------------------------
//  <copyright file="EnsureUmbracoContext.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.UmbracoHelpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;

    using Umbraco.Web;

    /// <summary>
    /// Contains the definition of an object of type <see cref="EnsureUmbracoContext" />.
    /// </summary>
    public sealed class EnsureUmbracoContext : IDisposable
    {
        /// <summary>
        /// The worker
        /// </summary>
        private readonly SimpleWorkerRequest worker;

        /// <summary>
        /// The writer
        /// </summary>
        private readonly StringWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnsureUmbracoContext" /> class.
        /// </summary>
        public EnsureUmbracoContext()
        {
            if (UmbracoContext.Current != null)
            {
                return;
            }

            if (HttpContext.Current == null)
            {
                this.writer = new StringWriter();
                this.worker = new SimpleWorkerRequest("EnsureUmbracoContext.axd", string.Empty, this.writer);
                HttpContext.Current = new HttpContext(this.worker);
            }

            // create context 😉
            UmbracoManager.EnsureContext();
        }

        /// <summary>
        /// Cleans fake context if needed.
        /// </summary>
        public void Dispose()
        {
            if (this.worker != null)
            {
                this.worker.EndOfRequest();
                this.writer.Dispose();
                HttpContext.Current = null;
            }
        }
    }
}