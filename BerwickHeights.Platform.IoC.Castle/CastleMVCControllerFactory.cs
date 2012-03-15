/*
 * Copyright 2012 Berwick Heights Software, Inc
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in 
 * compliance with the License. You may obtain a copy of the License at 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under the License is 
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
 * See the License for the specific language governing permissions and limitations under the License.
 *  
 */

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace BerwickHeights.Platform.IoC.Castle
{
    /// <summary>
    /// Factory that manages ASP.Net MVC controller instances using a Castle Windsor container.
    /// </summary>
    public class CastleMVCControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        internal CastleMVCControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <inheritDoc/>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
            }

            //
            // Use Castle container to instantiate an instance (the controller type must be registered with the 
            // container as a "transient" instance, not the default singleton)
            //
            return (IController)kernel.Resolve(controllerType);
        }

        /// <inheritDoc/>
        public override void ReleaseController(IController controller)
        {
            //
            // Since controller is a transient instance (required by ASP.Net MVC), we ask the container to release it
            //
            kernel.ReleaseComponent(controller);
        }
    }
}
