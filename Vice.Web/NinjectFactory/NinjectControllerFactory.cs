using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Vice.Repository;

namespace Vice.Web.NinjectFactory
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            this.ninjectKernel = new StandardKernel();
            AddBinds();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBinds()
        {
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}